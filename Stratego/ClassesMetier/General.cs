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
    /// Implementation de la piece «Général»
    /// </summary>
   public class General : PieceMobile
   {
        /// <summary>
        /// Construit un général
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public General(Couleur couleurPiece) : base(couleurPiece, 9, "general")
      {     
      }
   }
}
