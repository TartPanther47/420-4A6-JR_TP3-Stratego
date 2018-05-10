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
   /// Implementation de la piece «Colonel»
   /// </summary>
   public class Colonel : PieceMobile
   {
        /// <summary>
        /// Construit un colonel
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Colonel(Couleur couleurPiece) : base(couleurPiece, 8, "colonel")
      {
      }
   }
}
