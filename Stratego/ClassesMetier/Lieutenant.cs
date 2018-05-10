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
    /// Implementation de la piece «Lieutenant»
    /// </summary>
   public class Lieutenant : PieceMobile
   {
        /// <summary>
        /// Construit un lieutenant
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Lieutenant(Couleur couleurPiece) : base(couleurPiece, 5, "lieutenant")
      {                           
      }
   }
}
