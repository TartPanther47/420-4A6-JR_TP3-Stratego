using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stratego
{
   /// <summary>
   /// Logique d'interaction pour JeuStrategoControl.xaml
   /// </summary>
   public partial class JeuStrategoControl : UserControl, IConstructibleParametre, IDestructible
   {
      #region Static

      public const int TAILLE_CASES_GRILLE = 48;

      #endregion

      public GrilleJeu GrillePartie { get; private set; }

      private List<List<Rectangle>> GrillePieces { get; set; }

      private ConteneurPiecesCapturees ConteneurPiecesCapturees { get; set; }

      private Rectangle SelectionActive { get; set; }

      private ParametresCouleurJoueurs CouleurJoueurs { get; set; }

      public Couleur TourJeu { get; private set; }

      #region Code relié au patron observateur

      List<IObserver<JeuStrategoControl>> observers;

      // Oui, une classe privée (et interne).
      private class Unsubscriber : IDisposable
      {
         private List<IObserver<JeuStrategoControl>> _observers;
         private IObserver<JeuStrategoControl> _observer;

         public Unsubscriber(List<IObserver<JeuStrategoControl>> observers, IObserver<JeuStrategoControl> observer)
         {
            this._observers = observers;
            this._observer = observer;
         }

         public void Dispose()
         {
            if (!(_observer == null)) _observers.Remove(_observer);
         }
      }

      public IDisposable Subscribe(IObserver<JeuStrategoControl> observer)
      {
         if (!observers.Contains(observer))
            observers.Add(observer);

         return new Unsubscriber(observers, observer);
      }

      private void Notify()
      {
         foreach (IObserver<JeuStrategoControl> ob in observers)
         {
            ob.OnNext(this);
         }
      }
      #endregion

      private IA_Stratego IA { get; set; }

      public JeuStrategoControl()
      {
         InitializeComponent();
      }

        /// <summary>
        /// Cette méthode existe principalement pour que le jeu soit testable.
        /// On ne veut évidemment pas toujours commencer une partie avec exactement les même positions.
        /// </summary>
        /// <param name="pieces">Les pièces à positionner</param>
        /// <returns>Si les pièces à positionner sont valides (bon nombre de chaque et bonnes couleurs)</returns>
      private bool PositionnerPieces(List<Piece> pieces)
      {
         return GrillePartie.PositionnerPieces(pieces, CouleurJoueurs.CouleurJoueur) &&
                GrillePartie.PositionnerPieces(IA.PlacerPieces(), CouleurJoueurs.CouleurIA);
      }

      private void DiviserGrilleJeu()
      {
         ColumnDefinition colonneDef;
         RowDefinition ligneDef;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            colonneDef = new ColumnDefinition();
            colonneDef.Width = new GridLength(TAILLE_CASES_GRILLE);
            grdPartie.ColumnDefinitions.Add(colonneDef);

            ligneDef = new RowDefinition();
            ligneDef.Height = new GridLength(TAILLE_CASES_GRILLE);
            grdPartie.RowDefinitions.Add(ligneDef);
         }
      }

      private void ColorerGrilleJeu()
      {
         Rectangle ligne;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            grdPartie.Children.Add(CreerLigneGrille(i, true));

            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               grdPartie.Children.Add(CreerFondCase(i, j));

               if (i == 0)
               {
                  grdPartie.Children.Add(CreerLigneGrille(j, false));
               }
            }
         }

         ligne = CreerLigneGrille(0, true);
         ligne.HorizontalAlignment = HorizontalAlignment.Left;
         grdPartie.Children.Add(ligne);

         ligne = CreerLigneGrille(0, false);
         ligne.VerticalAlignment = VerticalAlignment.Top;
         grdPartie.Children.Add(ligne);
      }

      private Rectangle CreerLigneGrille(int position, bool estColonne)
      {
         Rectangle ligne = new Rectangle();
         ligne.Fill = Brushes.Gainsboro;
         Grid.SetZIndex(ligne, 1);

         if (estColonne)
         {
            ligne.Width = 1;
            ligne.Height = 10 * TAILLE_CASES_GRILLE;
            ligne.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetColumn(ligne, position);
            Grid.SetRow(ligne, 0);
            Grid.SetRowSpan(ligne, 10);
         }
         else
         {
            ligne.Width = 10 * TAILLE_CASES_GRILLE;
            ligne.Height = 1;
            ligne.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(ligne, 0);
            Grid.SetColumnSpan(ligne, 10);
            Grid.SetRow(ligne, position);
         }

         return ligne;
      }

      private Rectangle CreerFondCase(int colonne, int rangee)
      {
         Rectangle rect = new Rectangle();

         rect.Width = TAILLE_CASES_GRILLE;
         rect.Height = TAILLE_CASES_GRILLE;

         if (GrillePartie.EstCoordonneeLac(new Coordonnee(colonne, rangee)))
         {
            rect.Fill = new ImageBrush(new BitmapImage(new Uri("textures/lake.png", UriKind.Relative)));
         }
         else
         {
            rect.Fill = new ImageBrush(new BitmapImage(new Uri("textures/terrain.png", UriKind.Relative)));
         }

         Grid.SetZIndex(rect, 0);
         Grid.SetColumn(rect, colonne);
         Grid.SetRow(rect, rangee);

         return rect;
      }

      private void DefinirZoneSelectionGrille()
      {
         Rectangle rect;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               rect = new Rectangle();

               rect.Width = TAILLE_CASES_GRILLE;
               rect.Height = TAILLE_CASES_GRILLE;
               rect.Fill = Brushes.Transparent;
               Grid.SetZIndex(rect, 5);
               Grid.SetColumn(rect, i);
               Grid.SetRow(rect, j);

               grdPartie.Children.Add(rect);

               rect.MouseLeftButtonUp += ResoudreSelectionCase;
            }

         }

      }

      private void InitialiserSelectionActive()
      {
         SelectionActive = new Rectangle();

         SelectionActive.Width = TAILLE_CASES_GRILLE;
         SelectionActive.Height = TAILLE_CASES_GRILLE;
         SelectionActive.Fill = new ImageBrush(new BitmapImage(new Uri("textures/selector.png", UriKind.Relative)));
            Grid.SetZIndex(SelectionActive, 0);
      }

      private void InitialiserAffichagePieces()
      {
         Coordonnee position;
         Rectangle imageAffichage;

         GrillePieces = new List<List<Rectangle>>();

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            GrillePieces.Add(new List<Rectangle>());

            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               position = new Coordonnee(i, j);

               if (GrillePartie.EstCaseOccupee(position))
               {
                  imageAffichage = CreerAffichagePiece(GrillePartie.ObtenirPiece(position));

                  Grid.SetColumn(imageAffichage, i);
                  Grid.SetRow(imageAffichage, j);

                  grdPartie.Children.Add(imageAffichage);

                  GrillePieces[i].Add(imageAffichage);
               }
               else
               {
                  GrillePieces[i].Add(null);
               }
            }
         }
      }

      private Rectangle CreerAffichagePiece(Piece pieceAffichage)
      {
         Rectangle imageAffichage = new Rectangle();

         imageAffichage.Fill = new ImageBrush(
             new BitmapImage(
                 new Uri(
                     "sprites/" +
                     (pieceAffichage.Couleur == Couleur.Rouge ? "Rouge/" : "Bleu/") +
                     /*(pieceAffichage.Couleur == CouleurJoueurs.CouleurJoueur ?*/ pieceAffichage.Nom /*: "dos-carte")*/ +
                     ".png",
                     UriKind.Relative
                 )
             )
         );

         Grid.SetZIndex(imageAffichage, 2);

         return imageAffichage;
      }

      private void ResoudreSelectionCase(object sender, MouseButtonEventArgs e)
      {
         Rectangle caseSelectionnee = (Rectangle)sender;

         Coordonnee pointSelectionne = new Coordonnee(Grid.GetColumn(caseSelectionnee), Grid.GetRow(caseSelectionnee));
         Coordonnee pointActif;

         ReponseDeplacement reponse;

         if (TourJeu == CouleurJoueurs.CouleurJoueur)
         {
            if (grdPartie.Children.Contains(SelectionActive))
            {
               pointActif = new Coordonnee(Grid.GetColumn(SelectionActive), Grid.GetRow(SelectionActive));

               if (pointSelectionne == pointActif)
               {
                  grdPartie.Children.Remove(SelectionActive);
               }
               else
               {
                  reponse = ExecuterCoup(pointActif, pointSelectionne);

                  if (reponse.DeplacementFait)
                  {
                     grdPartie.Children.Remove(SelectionActive);
                  }
               }
            }
            else
            {
               if (GrillePartie.EstCaseOccupee(pointSelectionne)
                  && GrillePartie.ObtenirCouleurPiece(pointSelectionne) == CouleurJoueurs.CouleurJoueur)
                    {
                  Grid.SetColumn(SelectionActive, pointSelectionne.X);
                  Grid.SetRow(SelectionActive, pointSelectionne.Y);

                  grdPartie.Children.Add(SelectionActive);
               }
            }
         }
      }

      public ReponseDeplacement ExecuterCoup(Coordonnee caseDepart, Coordonnee caseCible)
      {
         Thread executionIA = new Thread(LancerIA);

         ReponseDeplacement reponse = new ReponseDeplacement();

         Piece attaquant;
         Rectangle affichageAttaquant;

         Piece cible;
         Rectangle affichageCible;

         if (caseCible != caseDepart)
         {
            // Prendre les informations avant de faire le coup.
            attaquant = GrillePartie.ObtenirPiece(caseDepart);
            affichageAttaquant = GrillePieces[caseDepart.X][caseDepart.Y];

            cible = GrillePartie.ObtenirPiece(caseCible);
            affichageCible = GrillePieces[caseCible.X][caseCible.Y];

            if(cible != null && attaquant != null && !attaquant.EstVisible)
            {
               affichageAttaquant.Fill = new ImageBrush(
                   new BitmapImage(
                       new Uri(
                           "sprites/" + (attaquant.EstDeCouleur(Couleur.Rouge) ? "Rouge/" : "Bleu/") + attaquant.Nom + ".png",
                           UriKind.Relative
                       )
                   )
               );
               attaquant.EstVisible = true;
            }
            if(cible != null && !cible.EstVisible)
            {
                affichageCible.Fill = new ImageBrush(
                    new BitmapImage(
                        new Uri(
                            "sprites/" + (cible.EstDeCouleur(Couleur.Rouge) ? "Rouge/" : "Bleu/") + cible.Nom + ".png",
                            UriKind.Relative
                        )
                    )
                );
                cible.EstVisible = true;
            }

            reponse = GrillePartie.ResoudreDeplacement(caseDepart, caseCible);

            foreach (Piece piece in reponse.PiecesEliminees)
                if(piece.EstDeCouleur(CouleurJoueurs.CouleurIA))
                    ConteneurPiecesCapturees.AjouterPiece(piece);

            if (reponse.DeplacementFait)
            {
                // Retrait de la pièce attaquante de sa position d'origine.
               grdPartie.Children.Remove(affichageAttaquant);
               GrillePieces[caseDepart.X][caseDepart.Y] = null;

               if (reponse.PiecesEliminees.Count == 2)
               {
                  // Retrait de la pièce attaquée.
                  grdPartie.Children.Remove(GrillePieces[caseCible.X][caseCible.Y]);
                  GrillePieces[caseCible.X][caseCible.Y] = null;
               }
               else if (reponse.PiecesEliminees.Count == 1 && reponse.PiecesEliminees[0] != attaquant
                       || reponse.PiecesEliminees.Count == 0)
               {
                  // Remplacement de la pièce attaquée par la pièce attaquante.
                  grdPartie.Children.Remove(GrillePieces[caseCible.X][caseCible.Y]);
                  GrillePieces[caseCible.X][caseCible.Y] = null;

                  GrillePieces[caseCible.X][caseCible.Y] = affichageAttaquant;

                  Grid.SetColumn(affichageAttaquant, caseCible.X);
                  Grid.SetRow(affichageAttaquant, caseCible.Y);
                  grdPartie.Children.Add(affichageAttaquant);
               }

               // Permet de faire jouer l'IA.
               if (TourJeu == CouleurJoueurs.CouleurJoueur)
                  executionIA.Start();
               ChangerTourJeu();
            }
         }
         else
         {
            reponse.DeplacementFait = false;
         }

         return reponse;
      }

      private void LancerIA()
      {
         // Pause d'une seconde, pour permettre à l'humain de mieux comprendre le déroulement.
         Thread.Sleep(1000);

         Dispatcher.Invoke(() =>
         {
            Notify();
         });
      }

      public void ChangerTourJeu()
      { 
         if (TourJeu == Couleur.Rouge)
         {
            TourJeu = Couleur.Bleu;
         }
         else
         {
            TourJeu = Couleur.Rouge;
         }
      }

        private void btnRecommencerPartie_Click(object sender, RoutedEventArgs e)
        {
            // TODO: ne demander que lorsque la partie est déjà commencée.
            if(MessageBox.Show("Voulez-vous vraiment recommencer la partie ?",
                               "Recommencer la partie",
                               MessageBoxButton.YesNo,
                               MessageBoxImage.Question,
                               MessageBoxResult.No) == MessageBoxResult.Yes)
                GestionnaireEcransJeu.ChangerEcran("Choix couleur");
        }

        public void Construire(Dictionary<string, ParametresConstruction> parametres)
        {
            CouleurJoueurs = (ParametresCouleurJoueurs)parametres["Couleur joueurs"];

            GrillePartie = new GrilleJeu(CouleurJoueurs.CouleurJoueur);

            ConteneurPiecesCapturees = new ConteneurPiecesCapturees(stpPiecesCapturees);

            // Initialise la liste d'observateurs.
            observers = new List<IObserver<JeuStrategoControl>>();

            // Initialiser l'IA.
            IA = new IA_Stratego(this, CouleurJoueurs.CouleurIA);

            DiviserGrilleJeu();
            ColorerGrilleJeu();
            DefinirZoneSelectionGrille();
            InitialiserSelectionActive();

            if(!PositionnerPieces(((ParametresPiecesJoueur)parametres["Pieces"]).Pieces))
            {
                MessageBox.Show("Nombre de pièces ou couleurs invalide(s).", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                GestionnaireEcransJeu.ChangerEcran("Accueil");
            }
            InitialiserAffichagePieces();

            #region Tests

            // Code des tests initiaux.
            /*
            ReponseDeplacement deplacement;

            deplacement = GrillePartie.ResoudreDeplacement(new Point(0, 6), new Point(0, 5)); // Deplacement

            deplacement = GrillePartie.ResoudreDeplacement(new Point(0, 5), new Point(-1, 5)); // Coord invalide
            deplacement = GrillePartie.ResoudreDeplacement(new Point(2, 6), new Point(2, 5)); // Lac

            deplacement = GrillePartie.ResoudreDeplacement(new Point(2, 6), new Point(3, 6)); // Piece vs sa propre couleur

            deplacement = GrillePartie.ResoudreDeplacement(new Point(1, 6), new Point(1, 5));
            deplacement = GrillePartie.ResoudreDeplacement(new Point(1, 5), new Point(1, 4));
            deplacement = GrillePartie.ResoudreDeplacement(new Point(1, 4), new Point(1, 3)); // Prise par attaquant

            deplacement = GrillePartie.ResoudreDeplacement(new Point(1, 3), new Point(1, 2));
            deplacement = GrillePartie.ResoudreDeplacement(new Point(1, 2), new Point(1, 1));
            // deplacement = GrillePartie.ResoudreDeplacement(new Point(1, 1), new Point(1, 0)); // 2 pièces éliminées
            deplacement = GrillePartie.ResoudreDeplacement(new Point(1, 1), new Point(2, 1));
            deplacement = GrillePartie.ResoudreDeplacement(new Point(2, 1), new Point(2, 0)); // Attaquant éliminé
            */

            #endregion

            TourJeu = Couleur.Rouge;

            // Lancer l'IA
            if(CouleurJoueurs.CouleurIA == Couleur.Rouge)
            {
                new Thread(LancerIA).Start();
            }
        }

        public void Detruire()
        {
            ConteneurPiecesCapturees.Vider();
            grdPartie.Children.Clear();
        }
    }
}
