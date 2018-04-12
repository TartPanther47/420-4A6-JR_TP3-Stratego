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

      public Piece(Couleur couleurPiece)
      {
         Couleur = couleurPiece;
      }

      public bool EstRouge()
      {
         return (Couleur == Couleur.Rouge);
      }

      public bool EstBleu()
      {
         return !EstRouge();
      }

   }
}
