using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
   public abstract class Piece
   {
      public Couleur Couleur { get; private set; }
      public string Nom { get; private set; }
      public bool EstVisible { get; set; }

      public Piece(Couleur couleurPiece, string nom)
      {
         Couleur = couleurPiece;
         Nom = nom;
         EstVisible = false;
      }

      public bool EstDeCouleur(Couleur couleur) => Couleur == couleur;
   }
}
