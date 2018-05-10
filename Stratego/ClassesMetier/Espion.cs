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
    /// Implementation de la piece «Espion»
    /// </summary>
   public class Espion : PieceMobile
   {
        /// <summary>
        /// Construit un espion
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Espion(Couleur couleurPiece) : base(couleurPiece, 1, "espion")
      {                       
      }
   }
}
