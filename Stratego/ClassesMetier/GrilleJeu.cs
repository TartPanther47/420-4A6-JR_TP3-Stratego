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
    /// Classe de grille de jeu, contient les cases de jeu.
    /// </summary>
   public class GrilleJeu
   {
        #region Static

        /// <summary>
        /// La taille de la grille de jeu. Assume une grille de jeu carrée (X par X).
        /// </summary>
        public const int TAILLE_GRILLE_JEU = 10;

        #endregion

        #region Attributs
        public JeuStrategoControl Parent { get; private set; }
        private List<List<CaseJeu>> GrilleCases { get; set; }

        private Couleur CouleurJoueur { get; set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Construit une grille de jeu
        /// </summary>
        /// <param name="couleurJoueur">La couleur du joueur</param>
        /// <param name="parent">Le contrôle parent</param>
        public GrilleJeu(Couleur couleurJoueur, JeuStrategoControl parent)
        {
           InitialiserGrille();
           Parent = parent;
           CouleurJoueur = couleurJoueur;
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Initialise la grille de jeu (place les terrains et les lacs au bon endroit)
        /// </summary>
        private void InitialiserGrille()
        {
           List<CaseJeu> colonne;
           GrilleCases = new List<List<CaseJeu>>();

           // Créer les cases et les structurer dans une grille à deux dimensions.
           for (int i = 0; i < TAILLE_GRILLE_JEU; i++)
           {
              colonne = new List<CaseJeu>();

              for (int j = 0; j < TAILLE_GRILLE_JEU; j++)
              {
                 // Coordonnées des lacs : I (2, 3, 6, 7) - J (4, 5)
                 if ((i == 2 || i == 3 || i == 6 || i == 7) && (j == 4 || j == 5))
                 {
                    colonne.Add(new CaseJeu("Lac", this));
                 }
                 else
                 {
                    colonne.Add(new CaseJeu("Terrain", this));
                 }
              }

              GrilleCases.Add(colonne);
           }

           // Créer les liens de voisinage entre les cases de la grille.
           LierCasesGrille();
        }

        /// <summary>
        /// Lie les cases à la grille
        /// </summary>
        private void LierCasesGrille()
        {
           for (int i = 0; i < TAILLE_GRILLE_JEU; i++)
           {
              for (int j = 0; j < TAILLE_GRILLE_JEU; j++)
              {
                 // Les coins.
                 if ((i == 0 || i == TAILLE_GRILLE_JEU - 1) && (j == 0 || j == TAILLE_GRILLE_JEU - 1))
                 {
                    if (i == 0)
                    {
                       GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                    }
                    else
                    {
                       GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                    }

                    if (j == 0)
                    {
                       GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
                    }
                    else 
                    {
                       GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                    }
                 }
                 // Côtés verticaux.
                 else if (i == 0 || i == TAILLE_GRILLE_JEU - 1)
                 {
                    if (i == 0)
                    {
                       GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                       GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                       GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
                    }
                    else
                    {
                       GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                       GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                       GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
                    }
                 }
                 // Côtés horizontaux.
                 else if (j == 0 || j == TAILLE_GRILLE_JEU - 1)
                 {
                    if (j == 0)
                    {
                       GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                       GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                       GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
                    }
                    else
                    {
                       GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                       GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                       GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                    }
                 }
                 else 
                 {
                    GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                    GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                    GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                    GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
                 }
              }
           }
        }

        /// <summary>
        /// Effectue un déplacement
        /// </summary>
        /// <param name="pointDepart">Coordonnée de départ</param>
        /// <param name="pointCible">Coordonnée de cible</param>
        /// <returns>Les pièce éliminées (dans un objet de transport)</returns>
        public ReponseDeplacement ResoudreDeplacement(Coordonnee pointDepart, Coordonnee pointCible)
        {
           ReponseDeplacement reponse = new ReponseDeplacement();

           CaseJeu caseDepart, caseCible;

           if (EstCoordonneeValide(pointDepart) && EstCoordonneeValide(pointCible))
           {
              caseDepart = GrilleCases[pointDepart.X][pointDepart.Y];
              caseCible = GrilleCases[pointCible.X][pointCible.Y];

              if (caseDepart.EstOccupe() && EstDeplacementPermis(pointDepart, pointCible))
              {
                 // Faire le déplacement.
                 reponse.PiecesEliminees = caseCible.ResoudreAttaque(caseDepart.Occupant, CouleurJoueur);
                 caseDepart.Occupant = null;

                 reponse.DeplacementFait = true;
              }
              else
              {
                 reponse.DeplacementFait = false;
              }
           }
           else
           {
              reponse.DeplacementFait = false;
           }

           return reponse;
        }

        /// <summary>
        /// Détermine si un déplacement est permis
        /// </summary>
        /// <param name="pointDepart">Coordonnée de départ</param>
        /// <param name="pointCible">Coordonnée de cible</param>
        /// <returns>Si le déplacement est permis</returns>
        public bool EstDeplacementPermis(Coordonnee pointDepart, Coordonnee pointCible)
        {
           return ( EstCoordonneeValide(pointDepart) && EstCoordonneeValide(pointCible)
                  && !EstCoordonneeLac(pointDepart) && !EstCoordonneeLac(pointCible)
                  && GrilleCases[pointDepart.X][pointDepart.Y].EstDeplacementLegal(GrilleCases[pointCible.X][pointCible.Y])
                  );
        }

        /// <summary>
        /// Détermine si une coordonnée est valide
        /// </summary>
        /// <param name="p">Coordonnée à tester</param>
        /// <returns>Si la coordonnée est valide</returns>
        private bool EstCoordonneeValide(Coordonnee p)
            => ((p.X >= 0 && p.X < TAILLE_GRILLE_JEU) && (p.Y >= 0 && p.Y < TAILLE_GRILLE_JEU));

        /// <summary>
        /// Détermine si une coordonnée est un lac (coordonnées : I (2, 3, 6, 7) - J (4, 5))
        /// </summary>
        /// <param name="p">La coordonnée à tester</param>
        /// <returns>Si la coordonnée est un lac</returns>
        public bool EstCoordonneeLac(Coordonnee p)
            => ((p.X == 2 || p.X == 3 || p.X == 6 || p.X == 7) && (p.Y == 4 || p.Y == 5));

        /// <summary>
        /// Détermine si la case à une coordonnée est occupée
        /// </summary>
        /// <param name="p">La coordonnée de la case à tester</param>
        /// <returns>Si la case est occupée</returns>
        public bool EstCaseOccupee(Coordonnee p)
        {
           return ((GrilleCases[(int)p.X][(int)p.Y]).EstOccupe());
        }

        /// <summary>
        /// Positionne des pièces sur la grille de jeu
        /// </summary>
        /// <param name="lstPieces">Les pièces à positionner</param>
        /// <param name="couleurJoueur">La couleur du joueur</param>
        /// <returns>Si le positionnement est appliqué</returns>
        public bool PositionnerPieces(List<Piece> lstPieces, Couleur couleurJoueur)
        {
           bool positionnementApplique = false;

           int compteur = 0;
           int decallage = 0;
           
           if (couleurJoueur == CouleurJoueur)
           {
              decallage = 6;
           }

           bool estValide = true;
           // Vérifier que les pièces sont de la même couleur
           for (int i = 1; i < lstPieces.Count; i++)
               if (lstPieces[i].Couleur != lstPieces[0].Couleur)
                   estValide = false;

           // Vérifier qu'on a le bon nombre de pièces de chaque type
           if (!ValidationPieces.ValiderNombrePiece<Marechal>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<General>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Colonel>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Commandant>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Capitaine>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Lieutenant>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Sergent>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Demineur>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Eclaireur>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Espion>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Drapeau>(lstPieces)) estValide = false;
           else if (!ValidationPieces.ValiderNombrePiece<Bombe>(lstPieces)) estValide = false;

           if (!PositionnementFait(couleurJoueur) && estValide)
           {
              positionnementApplique = true;

                // Appliquer le positionnement
              for (int j = 0 + decallage; j < 4 + decallage; j++)
              {
                 for (int i = 0; i < TAILLE_GRILLE_JEU; i++)
                 {
                    GrilleCases[i][j].Occupant = lstPieces[compteur];

                    compteur++;
                 }
              }
           }

           return positionnementApplique;
        }

        /// <summary>
        /// Détermine si le positionnement a été fait
        /// </summary>
        /// <param name="couleurJoueur">La couleur du joueur</param>
        /// <returns>Si le positionnement a été fait</returns>
        private bool PositionnementFait(Couleur couleurJoueur)
        {
           bool pieceTrouvee = false;

           for (int i = 0; i < TAILLE_GRILLE_JEU; i++)
           {
              for (int j = 0; j < TAILLE_GRILLE_JEU; j++)
              {
                 if (GrilleCases[i][j].Occupant != null 
                       && ((GrilleCases[i][j].Occupant.EstDeCouleur(Couleur.Rouge) && couleurJoueur == Couleur.Rouge)
                          || (GrilleCases[i][j].Occupant.EstDeCouleur(Couleur.Bleu) && couleurJoueur == Couleur.Bleu)))
                 {
                    pieceTrouvee = true;

                    // Inutile de chercher plus.
                    j = TAILLE_GRILLE_JEU;
                    i = TAILLE_GRILLE_JEU;

                    // ^ Les break, ça existe aussi ! ^
                 }
              }
           }

           return pieceTrouvee;
        }

        /// <summary>
        /// Calcule le nombre de pièces d'un type (pour une couleur) présentes dans la grille
        /// </summary>
        /// <param name="typePiece">Le type de pièce</param>
        /// <param name="couleur">La couleur des pièce</param>
        /// <returns>Le nombre de pièces</returns>
        public int CalculerNombrePieces(Type typePiece, Couleur couleur)
        {
           int nbPieces = 0;
           for(int x = 0; x < TAILLE_GRILLE_JEU; x++)
           {
               for(int y = 0; y < TAILLE_GRILLE_JEU; y++)
               {
                  Piece piece = ObtenirPiece(new Coordonnee(x, y));
                  if (piece != null && piece.EstDeCouleur(couleur) && piece.GetType() == typePiece) nbPieces++;
               }
           }
           return nbPieces;
        }

        /// <summary>
        /// Calcule le nombre de pièces d'une couleur présentes dans la grille
        /// </summary>
        /// <param name="couleur">La couleur des pièces</param>
        /// <returns>Le nombre de pièces</returns>
        public int CalculerNombrePieces(Couleur couleur)
        {
           int nbPieces = 0;
           for(int x = 0; x < TAILLE_GRILLE_JEU; x++)
           {
               for(int y = 0; y < TAILLE_GRILLE_JEU; y++)
               {
                  Piece piece = ObtenirPiece(new Coordonnee(x, y));
                  if (piece != null && piece.EstDeCouleur(couleur)) nbPieces++;
               }
           }
           return nbPieces;
        }

        /// <summary>
        /// Vérifie si le joueur de la couleur spécifiée n'est pas bloqué par des bombes au début de la partie
        /// </summary>
        /// <param name="couleur">Couleur du joueur</param>
        /// <returns>Si le joueur peut se déplacer</returns>
        public bool VerifierDeplacementsBombes(Couleur couleur)
        {
            List<int> lstAbscisses = new List<int>
            {   0, 1,
                4, 5,
                8, 9   };
            int ordonnee = (couleur == CouleurJoueur ? 6 : 3);

            bool bEstDeplacementPossible = false;

            foreach (int abscisse in lstAbscisses)
                if (!(ObtenirPiece(new Coordonnee(abscisse, ordonnee)) is Bombe))
                    bEstDeplacementPossible = true;

            return bEstDeplacementPossible;
        }

        /// <summary>
        /// Obtenir l'occupant d'une case spécifiée
        /// </summary>
        /// <param name="p">La coordonnée de la case</param>
        /// <returns>La pièce occupant la case</returns>
        public Piece ObtenirPiece(Coordonnee p)
        {
           return GrilleCases[p.X][p.Y].Occupant;
        }

        /// <summary>
        /// Obtenir la couleur d'une pièce
        /// </summary>
        /// <param name="p">La coordonnée de la case où se trouve la pièce</param>
        /// <returns>La couleur de la pièce</returns>
        public Couleur ObtenirCouleurPiece(Coordonnee p)
        {
           return GrilleCases[p.X][p.Y].Occupant.Couleur;
        }
        #endregion
    }
}
