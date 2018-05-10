// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

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

        #region Attributs
        public GrilleJeu GrillePartie { get; private set; }

        private List<List<Rectangle>> GrillePieces { get; set; }

        private ConteneurPiecesCapturees ConteneurPiecesCapturees { get; set; }

        private Rectangle SelectionActive { get; set; }

        private ParametresCouleurJoueurs CouleurJoueurs { get; set; }

        private bool EstPartieTerminee { get; set; }

        public bool EstDebutPartie { get; set; }

        public Couleur TourJeu { get; private set; }

        private IA_Stratego IA { get; set; }
        #endregion

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

        #region Construire
        /// <summary>
        /// Construit le contrôle
        /// </summary>
        public JeuStrategoControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Methodes
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

        /// <summary>
        /// Divise la grille en cases
        /// </summary>
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

        /// <summary>
        /// Place les textures dans la grille
        /// </summary>
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

        /// <summary>
        /// Crée une ligne à mettre dans la grille
        /// </summary>
        /// <param name="position">Position de la ligne</param>
        /// <param name="estColonne">Vrai: colonne, Faux: rangée</param>
        /// <returns></returns>
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

        /// <summary>
        /// Crée une texture à mettre au fond d'une case
        /// </summary>
        /// <param name="colonne">Position abscisse</param>
        /// <param name="rangee">Position ordonnée</param>
        /// <returns>La texture</returns>
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

        /// <summary>
        /// Ajoute les évènements de clic sur les cases
        /// </summary>
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

        /// <summary>
        /// Initialise le sélecteur
        /// </summary>
        private void InitialiserSelectionActive()
        {
           SelectionActive = new Rectangle();

           SelectionActive.Width = TAILLE_CASES_GRILLE;
           SelectionActive.Height = TAILLE_CASES_GRILLE;
           SelectionActive.Fill = new ImageBrush(new BitmapImage(new Uri("textures/selector.png", UriKind.Relative)));
              Grid.SetZIndex(SelectionActive, 0);
        }

        /// <summary>
        /// Crée les affichages des pièces
        /// </summary>
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

        /// <summary>
        /// Crée une texture de pièce
        /// </summary>
        /// <param name="pieceAffichage">La pièce à afficher</param>
        /// <returns>La texture</returns>
        private Rectangle CreerAffichagePiece(Piece pieceAffichage)
        {
           Rectangle imageAffichage = new Rectangle();

           imageAffichage.Fill = new ImageBrush(
               new BitmapImage(
                   new Uri(
                       "sprites/" +
                       (pieceAffichage.Couleur == Couleur.Rouge ? "Rouge/" : "Bleu/") +
                       (pieceAffichage.Couleur == CouleurJoueurs.CouleurJoueur ? pieceAffichage.Nom : "dos-carte") +
                       ".png",
                       UriKind.Relative
                   )
               )
           );

           Grid.SetZIndex(imageAffichage, 2);

           return imageAffichage;
        }

        /// <summary>
        /// S'exécute lorsque l'on clique sur une case.
        /// </summary>
        /// <param name="sender">Objet appelant</param>
        /// <param name="e">Arguments</param>
        private void ResoudreSelectionCase(object sender, MouseButtonEventArgs e)
        {
           Rectangle caseSelectionnee = (Rectangle)sender;

           Coordonnee pointSelectionne = new Coordonnee(Grid.GetColumn(caseSelectionnee), Grid.GetRow(caseSelectionnee));
           Coordonnee pointActif;

           ReponseDeplacement reponse;

            // Tour du joueur
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

        /// <summary>
        /// Exécuter un coup
        /// </summary>
        /// <param name="caseDepart">Case de départ</param>
        /// <param name="caseCible">Case de cible</param>
        /// <returns></returns>
        public ReponseDeplacement ExecuterCoup(Coordonnee caseDepart, Coordonnee caseCible)
        {
           if(EstPartieTerminee) return new ReponseDeplacement();

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

              if(GrillePartie.EstDeplacementPermis(caseDepart, caseCible) && cible != null && attaquant != null && !attaquant.EstVisible)
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
              if(GrillePartie.EstDeplacementPermis(caseDepart, caseCible) && cible != null && !cible.EstVisible)
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

                 if (EstDebutPartie)
                      EstDebutPartie = false;

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

             // Si le joueur ne peut plus jouer, l'IA gagne.
           if(!JoueurACoupsPermis())
           {
              ((JeuStrategoControl)GestionnaireEcransJeu.GetEcranPresent()).TerminerPartie();
              MessageBox.Show("Vous avez perdu (plus de mouvements possibles)...", "Échec");
           }

           return reponse;
        }

        /// <summary>
        /// Détermine si le joueur peut jouer (s'il a des coups disponibles)
        /// </summary>
        /// <returns>Si le joueur peut jouer</returns>
        private bool JoueurACoupsPermis()
        {
            List<CoupCote> listeCoups = new List<CoupCote>();
            Coordonnee pointDepart, pointCible;

            for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
            {
                for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
                {
                    pointDepart = new Coordonnee(i, j);

                    if (GrillePartie.EstCaseOccupee(pointDepart)
                       && GrillePartie.ObtenirCouleurPiece(pointDepart) == CouleurJoueurs.CouleurJoueur)
                    {
                        if (GrillePartie.ObtenirPiece(pointDepart) is Eclaireur)
                        {
                            // Valider les coups vers la gauche.
                            for (int k = pointDepart.X; k >= 0; k--)
                            {
                                pointCible = new Coordonnee(k, pointDepart.Y);
                                if (GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                                    listeCoups.Add(new CoupCote(pointDepart, pointCible, GrillePartie, CouleurJoueurs.CouleurJoueur));
                            }
                            // Valider les coups vers le haut.
                            for (int k = pointDepart.Y; k >= 0; k--)
                            {
                                pointCible = new Coordonnee(pointDepart.X, k);
                                if (GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                                    listeCoups.Add(new CoupCote(pointDepart, pointCible, GrillePartie, CouleurJoueurs.CouleurJoueur));
                            }
                            // Valider les coups vers la droite.
                            for (int k = pointDepart.X; k < GrilleJeu.TAILLE_GRILLE_JEU; k++)
                            {
                                pointCible = new Coordonnee(k, pointDepart.Y);
                                if (GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                                    listeCoups.Add(new CoupCote(pointDepart, pointCible, GrillePartie, CouleurJoueurs.CouleurJoueur));
                            }
                            // Valider les coups vers le bas.
                            for (int k = pointDepart.Y; k < GrilleJeu.TAILLE_GRILLE_JEU; k++)
                            {
                                pointCible = new Coordonnee(k, pointDepart.Y);
                                if (GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                                    listeCoups.Add(new CoupCote(pointDepart, pointCible, GrillePartie, CouleurJoueurs.CouleurJoueur));
                            }
                        }
                        else
                        {
                            // Valider un coup vers la gauche.
                            pointCible = new Coordonnee(pointDepart.X - 1, pointDepart.Y);
                            if (GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                                listeCoups.Add(new CoupCote(pointDepart, pointCible, GrillePartie, CouleurJoueurs.CouleurJoueur));

                            // Valider un coup vers l'avant.
                            pointCible = new Coordonnee(pointDepart.X, pointDepart.Y - 1);
                            if (GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                                listeCoups.Add(new CoupCote(pointDepart, pointCible, GrillePartie, CouleurJoueurs.CouleurJoueur));

                            // Valider un coup vers la droite.
                            pointCible = new Coordonnee(pointDepart.X + 1, pointDepart.Y);
                            if (GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                                listeCoups.Add(new CoupCote(pointDepart, pointCible, GrillePartie, CouleurJoueurs.CouleurJoueur));

                            // Valider un coup vers l'arrière.
                            pointCible = new Coordonnee(pointDepart.X, pointDepart.Y + 1);
                            if (GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                                listeCoups.Add(new CoupCote(pointDepart, pointCible, GrillePartie, CouleurJoueurs.CouleurJoueur));
                        }
                    }
                }
            }

            return listeCoups.Count > 0;
        }

        /// <summary>
        /// Lance le l'IA, qui fait un coup
        /// </summary>
        private void LancerIA()
        {
           // Pause d'une seconde, pour permettre à l'humain de mieux comprendre le déroulement.
           Thread.Sleep(1000);

           Dispatcher.Invoke(() =>
           {
              Notify();
           });
        }

        /// <summary>
        /// Changer la couleur du tour
        /// </summary>
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

        /// <summary>
        /// Termine la partie: on ne peut plus effectuer de coup
        /// </summary>
        public void TerminerPartie()
        {
            EstPartieTerminee = true;
            grdPartie.IsEnabled = false;
        }

        /// <summary>
        /// S'exécute lorsque l'on clique sur le bouton « Recommencer partie ».
        /// </summary>
        /// <param name="sender">Objet appelant</param>
        /// <param name="e">Arguments</param>
        private void btnRecommencerPartie_Click(object sender, RoutedEventArgs e)
        {
            bool bDecision = true;
            if(!EstDebutPartie && MessageBox.Show("Voulez-vous vraiment recommencer la partie ?",
                               "Recommencer la partie",
                               MessageBoxButton.YesNo,
                               MessageBoxImage.Question,
                               MessageBoxResult.No) == MessageBoxResult.No)
                bDecision = false;
            if(bDecision)
                GestionnaireEcransJeu.ChangerEcran("Choix couleur");
        }

        /// <summary>
        /// Construction du contrôleur
        /// </summary>
        /// <param name="parametres">Paramètres en provenance du contrôleur précédent</param>
        public void Construire(Dictionary<string, ParametresConstruction> parametres)
        {
            CouleurJoueurs = (ParametresCouleurJoueurs)parametres["Couleur joueurs"];

            grdPartie.IsEnabled = true;

            EstPartieTerminee = false;
            EstDebutPartie = true;

            GrillePartie = new GrilleJeu(CouleurJoueurs.CouleurJoueur, this);

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

            if(!GrillePartie.VerifierDeplacementsBombes(CouleurJoueurs.CouleurJoueur))
            {
                TerminerPartie();
                MessageBox.Show("Vous avez perdu (aucun mouvement possible)...", "Échec");
            }

            if (!GrillePartie.VerifierDeplacementsBombes(CouleurJoueurs.CouleurIA))
            {
                TerminerPartie();
                MessageBox.Show("Vous avez gagné (aucun mouvement possible)!", "Victoire");
            }

            // Lancer l'IA
            if (CouleurJoueurs.CouleurIA == Couleur.Rouge)
            {
                new Thread(LancerIA).Start();
            }
        }

        /// <summary>
        /// Destruction du contrôleur: retire les pièces de l'interface (de la grille et de la liste de pièces capturées)
        /// </summary>
        public void Detruire()
        {
            ConteneurPiecesCapturees.Vider();
            grdPartie.Children.Clear();
        }
        #endregion
    }
}
