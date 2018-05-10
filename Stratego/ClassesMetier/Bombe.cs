// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Implementation de la piece «Bombe»
    /// </summary>
   public class Bombe : Piece
   {
        /// <summary>
        /// Construit une bombe
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
      public Bombe(Couleur couleurPiece) : base(couleurPiece, "bombe")
      {                      
      }
   }
}
