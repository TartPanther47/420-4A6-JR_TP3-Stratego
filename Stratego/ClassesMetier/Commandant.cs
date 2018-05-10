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
    /// Implementation de la piece «Commandant»
    /// </summary>
   public class Commandant : PieceMobile
   {
        /// <summary>
        /// Construit un commandant
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Commandant(Couleur couleurPiece) : base(couleurPiece, 7, "commandant")
      {                           
      }
   }
}
