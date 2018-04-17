using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
   public class CaseJeu
   {
      public CaseJeu VoisinAvant { get; set; }
      public CaseJeu VoisinArriere { get; set; }
      public CaseJeu VoisinGauche { get; set; }
      public CaseJeu VoisinDroite { get; set; }

      public Piece Occupant { get; set; }

      public string TypeCase { get; set; }

      public CaseJeu(string type)
      {
         TypeCase = type;
      }

      public bool EstOccupe()
      {
         return (Occupant != null);
      }

      public List<Piece> ResoudreAttaque(Piece attaquant)
      {
         List<Piece> piecesEliminees = new List<Piece>();

         if (Occupant != null)
         {
            if (Occupant is PieceMobile &&
                attaquant is PieceMobile &&
                ((PieceMobile) attaquant).Force < ((PieceMobile) Occupant).Force)
            {
               piecesEliminees.Add(attaquant);
            }
            else if (Occupant is PieceMobile &&
                     attaquant is PieceMobile &&
                     ((PieceMobile)attaquant).Force > ((PieceMobile)Occupant).Force)
            {
               piecesEliminees.Add(Occupant);
               Occupant = attaquant;
            }
            else
            {
               piecesEliminees.Add(attaquant);
               piecesEliminees.Add(Occupant);
               Occupant = null;
            }
         }
         else
         { 
            Occupant = attaquant;
         }

         return piecesEliminees;
      }
        
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

      public bool EstVoisineDe(CaseJeu caseCible)
      {
        return (caseCible != null
           && (VoisinGauche == caseCible || VoisinAvant == caseCible
              || VoisinDroite == caseCible || VoisinArriere == caseCible)
           );
      }

      public bool EstDeplacementLegal(CaseJeu caseCible)
      {
         bool resultat = false;

         if (EstVoisineDe(caseCible) || (Occupant is Eclaireur &&
                                        (EstAccessible(caseCible, Direction.Avant) ||
                                         EstAccessible(caseCible, Direction.Gauche) ||
                                         EstAccessible(caseCible, Direction.Arriere) ||
                                         EstAccessible(caseCible, Direction.Droite)))
         ) {
            if (Occupant is PieceMobile &&
                (!caseCible.EstOccupe()
                || Occupant.Couleur != caseCible.Occupant.Couleur))
            {
               resultat = true;
            }
         }

         return resultat;
      }
   }
}
