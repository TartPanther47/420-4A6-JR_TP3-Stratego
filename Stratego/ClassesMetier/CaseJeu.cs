// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Stratego
{
    /// <summary>
    /// Case de jeu, contient une pièce occupante et connait ses voisins
    /// </summary>
   public class CaseJeu
   {
        #region Attributs
        private GrilleJeu GrilleJeu { get; set; }

        public CaseJeu VoisinAvant { get; set; }
        public CaseJeu VoisinArriere { get; set; }
        public CaseJeu VoisinGauche { get; set; }
        public CaseJeu VoisinDroite { get; set; }

        public Piece Occupant { get; set; }

        public string TypeCase { get; set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Construit une case de jeu
        /// </summary>
        /// <param name="type">Le type de la case (terrain, lac, etc.)</param>
        /// <param name="grilleJeu">La grille de jeu dans laquelle on met la case</param>
        public CaseJeu(string type, GrilleJeu grilleJeu)
        {
           TypeCase = type;
           GrilleJeu = grilleJeu;
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Vérifier s'il y a une pièce sur la case
        /// </summary>
        /// <returns>Si la case est occupé</returns>
        public bool EstOccupe() => (Occupant != null);

        /// <summary>
        /// Résoudre une attaque (détermine le gagnant et le ou les perdants)
        /// </summary>
        /// <param name="attaquant">Pièce attaquante</param>
        /// <param name="couleurJoueur">Couleur du joueur</param>
        /// <returns>Liste des pièces éliminées</returns>
        public List<Piece> ResoudreAttaque(Piece attaquant, Couleur couleurJoueur)
        {
           List<Piece> piecesEliminees = new List<Piece>();

           if (Occupant != null) // Ne rien faire s'il n'y a pas d'occupant
              {
              if(attaquant is PieceMobile) // Ne rien faire si l'attaquant n'est pas mobile (ce qui ne devrait jamais arriver)
              {
                 if(Occupant is PieceMobile) // Distinguer la mobilité de l'occupant
                 {
                      if (Occupant is Marechal && attaquant is Espion ||
                          Occupant is Espion && attaquant is Marechal)
                       { // Cas spécial : l'espion et le maréchal gagnent s'ils attaquent l'autre
                          piecesEliminees.Add(Occupant);
                          Occupant = attaquant;
                      }
                      else if (((PieceMobile)attaquant).Force < ((PieceMobile)Occupant).Force)
                      {
                          piecesEliminees.Add(attaquant);
                      }
                      else if (((PieceMobile)attaquant).Force > ((PieceMobile)Occupant).Force)
                      {
                          piecesEliminees.Add(Occupant);
                          Occupant = attaquant;
                      }
                      else // Si à forces égales, les deux sont capturées
                      {
                          piecesEliminees.Add(attaquant);
                          piecesEliminees.Add(Occupant);
                          Occupant = null;
                      }
                 }
                 else if(Occupant is Bombe) // Cas spécial : la bombe capture toutes les pièces sauf le démineur
                 {
                      if (attaquant is Demineur)
                      {
                          piecesEliminees.Add(Occupant);
                          Occupant = attaquant;
                      }
                      else
                          piecesEliminees.Add(attaquant);
                 }
                 else if(Occupant is Drapeau) // Si on capture le drapeau, on gagne
                 {
                      GrilleJeu.Parent.TerminerPartie();

                      if(Occupant.EstDeCouleur(couleurJoueur))
                          MessageBox.Show("Vous avez perdu...", "Échec");
                      else
                          MessageBox.Show("Vous avez gagné!", "Victoire");
                 }
              }
           }
           else
           { 
              Occupant = attaquant;
           }

           return piecesEliminees;
        }
          
        /// <summary>
        /// Détermine si case cible est accessible pour l'occupant (plus que les voisins, pour l'éclaireur)
        /// </summary>
        /// <param name="caseCible">La case cible</param>
        /// <param name="direction">La direction à tester</param>
        /// <returns>Si la case cible est accessible</returns>
        public bool EstAccessible(CaseJeu caseCible, Direction direction)
        {
              CaseJeu caseCourante = this;

              while(caseCourante != caseCible &&
                   (caseCourante != null && (!caseCourante.EstOccupe() || caseCourante == this)))
              {
                  switch (direction)
                  {
                      case Direction.Avant:
                          caseCourante = caseCourante.VoisinAvant;
                          break;
                      case Direction.Gauche:
                          caseCourante = caseCourante.VoisinGauche;
                          break;
                      case Direction.Arriere:
                          caseCourante = caseCourante.VoisinArriere;
                          break;
                      case Direction.Droite:
                          caseCourante = caseCourante.VoisinDroite;
                          break;
                  }
              }

              return caseCourante == caseCible;
        }

        /// <summary>
        /// Détermine si la case est voisine d'une case cible
        /// </summary>
        /// <param name="caseCible">La case cible</param>
        /// <returns>Si la case cible est voisine</returns>
        public bool EstVoisineDe(CaseJeu caseCible)
        {
          return (caseCible != null
             && (VoisinGauche == caseCible || VoisinAvant == caseCible
                || VoisinDroite == caseCible || VoisinArriere == caseCible)
             );
        }

        /// <summary>
        /// Détermine si un déplacement est légal (teste selon le type de pièce)
        /// </summary>
        /// <param name="caseCible">La case cible</param>
        /// <returns>Si le déplacement est légal</returns>
        public bool EstDeplacementLegal(CaseJeu caseCible)
        {
           bool resultat = false;

            // Tester voisinage.             Cas spécial: l'éclaireur peut se déplacer de plusieurs cases.
           if (EstVoisineDe(caseCible) || (Occupant is Eclaireur &&
                                          (EstAccessible(caseCible, Direction.Avant) ||
                                           EstAccessible(caseCible, Direction.Gauche) ||
                                           EstAccessible(caseCible, Direction.Arriere) ||
                                           EstAccessible(caseCible, Direction.Droite)))
           ) {
              if (Occupant is PieceMobile &&
                  (!caseCible.EstOccupe()
                  || Occupant.Couleur != caseCible.Occupant.Couleur)) // On ne peut pas attaquer une pièce de la même couleur que soi
              {
                 resultat = true;
              }
           }

           return resultat;
        }
        #endregion
    }
}
