// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stratego
{
    /// <summary>
    /// Classe de l'intelligence artificielle, elle peut placer les pièces et décider des coups à jouer.
    /// </summary>
    /// 
    /// <remarks>
    /// Fonctionnement du processus de décision:
    ///     On trouve tous les coups possibles pour la couleur de l'IA.
    ///     On donne une cote à chaque coup selon :
    ///         Si on connait le type de la pièce, sa cote correspond à un poid (qui est associé au type)
    ///             Si la pièce gagne, la cote est positive
    ///             Si la pièce perd, la cote est négative
    ///             Si c'est égalité, la cote est zéro
    ///         Si on ne connait pas le type de la pièce
    ///             On calcule la probabilité que la pièce soit de chaque type
    ///             On prend la meilleure option (s'il y en a plusieurs, on choisi aléatoirement parmis les meilleurs options)
    ///             On applique la même logique que si on connait la pièce, mais avec le type choisi
    ///     On trie les coups en ordre décroissant de cote
    ///     On choisi le meilleur coup (s'il y en a plusieurs, on choisi aléatoirement parmis les meilleurs options)
    ///     On exécute le coup retenu
    ///     
    /// Note : on n'évalue qu'une profondeur de coups.
    /// 
    ///     Le calcule de la cote est dans le fichier : « CoupCote.cs »
    /// 
    /// Fonctionnement du processus de placement des pièces:
    ///     Pour chaque type de pièce, on applique une stratégie itérativement jusqu'à ce qu'on aie placé toutes les pièces du type
    ///     Les stratégies sont :
    ///     
    ///             - Règle de placement #1. -
    ///         Le drapeau est placé sur la première ligne (la plus éloignée de l'ennemi),
    ///         et sous un lac (pour ne pas être directement accessible).
    ///         
    ///             - Règle de placement #2. -
    ///         Le maréchal est placé en arrière d'un lac, comme ça il est disponible rapidement, mais il n'est pas
    ///         directement exposé.
    ///         
    ///             - Règle de placement #3. -
    ///         Trois bombes sont placées à gauche, à droite et en dessous du drapeau,
    ///         et les autres sont placées aléatoirement.
    ///         
    ///             - Règle de placement #4. -
    ///         Les autres pièces sont placées aléatoirement.
    ///         
    ///     Les stratégies sont dans les fichiers :
    ///                                             « StrategiePlacementPieceDrapeau.cs »,
    ///                                             « StrategiePlacementPieceMarechal.cs »,
    ///                                             « StrategiePlacementPieceBombe.cs » et
    ///                                             « StrategiePlacementPieceAleatoire.cs »
    /// </remarks>
    public class IA_Stratego : IObserver<JeuStrategoControl>
   {
        #region Statiques
        private const int LARGEUR_ZONE_DEPART = GrilleJeu.TAILLE_GRILLE_JEU;
        private const int HAUTEUR_ZONE_DEPART = 4;
        #endregion

        #region Code relié au patron observateur

        private IDisposable unsubscriber;

      public void Subscribe(IObservable<JeuStrategoControl> provider)
      {
         unsubscriber = provider.Subscribe(this);
      }

      public void Unsubscribe()
      {
         unsubscriber.Dispose();
      }

      public void OnCompleted()
      {
         // Ne fait rien pour l'instant.
      }

      public void OnError(Exception error)
      {
         // Ne fait rien pour l'instant.
      }

      public void OnNext(JeuStrategoControl g)
      {
         JouerCoup(g);
      }
        #endregion

        #region Attributs
        private JeuStrategoControl Jeu { get; set; }

        private Couleur Couleur { get; set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Construit une intelligence artificielle
        /// </summary>
        /// <param name="jeu">Le contrôle parent</param>
        /// <param name="couleur">La couleur de l'IA</param>
        public IA_Stratego(JeuStrategoControl jeu, Couleur couleur)
        {
           Jeu = jeu;
           Couleur = couleur;

           // Abonner l'IA à l'interface du jeu.
           jeu.Subscribe(this);
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Jouer un coup
        /// </summary>
        /// <param name="jeu"></param>
        private void JouerCoup(JeuStrategoControl jeu)
        {
           List<CoupCote> ListeCoupsPermis;
           Random rnd = new Random(DateTime.Now.Millisecond);
           int choixRnd;

              // On trouve les coups permis et on les ordonne
           ListeCoupsPermis = TrouverCoupsPermis(jeu.GrillePartie);
           ListeCoupsPermis.Sort();

              // Si l'IA ne peut plus jouer, le joueur gagne.
           if(ListeCoupsPermis.Count == 0)
           {
              ((JeuStrategoControl)GestionnaireEcransJeu.GetEcranPresent()).TerminerPartie();

                MessageBox.Show("Vous avez gagné (plus de mouvements possibles)!", "Victoire");
            }

            // On choisi le bon coups et on le joue.

            List<CoupCote> ListeCoupsEgaux = new List<CoupCote>();

           bool bEstPareille = true;
           int iScore = ListeCoupsPermis[0].Cote;
           for(int i = 0; i < ListeCoupsPermis.Count && bEstPareille; ++i)
           {
              if(ListeCoupsPermis[i].Cote != iScore) bEstPareille = false;
              else ListeCoupsEgaux.Add(ListeCoupsPermis[i]);
           }

           choixRnd = rnd.Next(0, ListeCoupsEgaux.Count);
           jeu.ExecuterCoup(ListeCoupsEgaux[choixRnd].Attaquant, ListeCoupsEgaux[choixRnd].Cible);
        }

        /// <summary>
        /// Trouve les coups que l'IA peut jouer
        /// </summary>
        /// <param name="grillePartie">La grille de jeu</param>
        /// <returns>Liste des coups que l'IA peut jouer</returns>
        private List<CoupCote> TrouverCoupsPermis(GrilleJeu grillePartie)
        {
           List<CoupCote> listeCoups = new List<CoupCote>();
           Coordonnee pointDepart, pointCible;

           for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
           {
              for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
              {
                 pointDepart = new Coordonnee(i, j);

                 if (Jeu.GrillePartie.EstCaseOccupee(pointDepart) 
                    && Jeu.GrillePartie.ObtenirCouleurPiece(pointDepart) == Couleur)
                 {
                    if(Jeu.GrillePartie.ObtenirPiece(pointDepart) is Eclaireur)
                    {
                        // Valider les coups vers la gauche.
                        for (int k = pointDepart.X; k >= 0; k--)
                        {
                            pointCible = new Coordonnee(k, pointDepart.Y);
                            if(Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                              listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                        }
                        // Valider les coups vers le haut.
                        for (int k = pointDepart.Y; k >= 0; k--)
                        {
                            pointCible = new Coordonnee(pointDepart.X, k);
                            if(Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                              listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                        }
                        // Valider les coups vers la droite.
                        for (int k = pointDepart.X; k < GrilleJeu.TAILLE_GRILLE_JEU; k++)
                        {
                            pointCible = new Coordonnee(k, pointDepart.Y);
                            if(Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                              listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                        }
                        // Valider les coups vers le bas.
                        for (int k = pointDepart.Y; k < GrilleJeu.TAILLE_GRILLE_JEU; k++)
                        {
                            pointCible = new Coordonnee(k, pointDepart.Y);
                            if(Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                              listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                        }
                    }
                    else
                    {
                        // Valider un coup vers la gauche.
                        pointCible = new Coordonnee(pointDepart.X - 1, pointDepart.Y);
                        if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                           listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));

                        // Valider un coup vers l'avant.
                        pointCible = new Coordonnee(pointDepart.X, pointDepart.Y - 1);
                        if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                           listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));

                        // Valider un coup vers la droite.
                        pointCible = new Coordonnee(pointDepart.X + 1, pointDepart.Y);
                        if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                          listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));

                        // Valider un coup vers l'arrière.
                        pointCible = new Coordonnee(pointDepart.X, pointDepart.Y + 1);
                        if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                          listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                    }
                 }
              }
           }

           return listeCoups;
        }

        /// <summary>
        /// Détermine le placement des pièces (en début de partie)
        /// </summary>
        /// <returns>Liste de pièces à placer</returns>
        public List<Piece> PlacerPieces()
        {
              Piece[,] pieces = new Piece[LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART];

                // Générateurs de pièces
              List<GenerateurPiece> generateursPieces = new List<GenerateurPiece>
              {
                  new GenerateurDrapeau(),
                  new GenerateurMarechal(),
                  new GenerateurBombe(),
                  new GenerateurGeneral(),
                  new GenerateurColonel(),
                  new GenerateurCommandant(),
                  new GenerateurCapitaine(),
                  new GenerateurLieutenant(),
                  new GenerateurSergent(),
                  new GenerateurDemineur(),
                  new GenerateurEclaireur(),
                  new GenerateurEspion()
              };
              
                // Pile de stratégies
              List<StrategiePlacementPiece> strategies = new List<StrategiePlacementPiece>
              {
                  new StrategiePlacementPieceDrapeau(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceMarechal(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceBombe(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                  new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART)
              };
              
                // Exécuter les stratégies en cascade
              for (int i = 0; i < generateursPieces.Count; i++)
              {
                  while(generateursPieces[i].EstGenerable())
                  {
                      Coordonnee position = strategies[i].GetPosition();
                      pieces[position.X, position.Y] = generateursPieces[i].GenererPiece(Couleur);
                  }
              }

                // Copier les pièces dans une liste
              List<Piece> lstPieces = new List<Piece>();
              for (int y = 0; y < HAUTEUR_ZONE_DEPART; y++)
                  for (int x = 0; x < LARGEUR_ZONE_DEPART; x++)
                      lstPieces.Add(pieces[x, y]);

              return lstPieces;
        }
        #endregion
    }
}
