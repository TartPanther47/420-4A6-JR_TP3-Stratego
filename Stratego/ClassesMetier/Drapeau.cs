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
    /// Implementation de la piece «Drapeau»
    /// </summary>
   public class Drapeau : Piece
   {
        /// <summary>
        /// Construit un drapeau
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Drapeau(Couleur couleurPiece) : base(couleurPiece, "drapeau")
      {                        
      }
   }
}
