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
      public int Force { get; private set; }

      public Piece(Couleur couleurPiece, int forcePiece)
      {
         Couleur = couleurPiece;
         Force = forcePiece;
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
