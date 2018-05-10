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
    /// Implementation de la piece «Démineur»
    /// </summary>
   public class Demineur : PieceMobile
   {
        /// <summary>
        /// Construit un démineur
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Demineur(Couleur couleurPiece) : base(couleurPiece, 3, "demineur")
      {
      }
   }
}
