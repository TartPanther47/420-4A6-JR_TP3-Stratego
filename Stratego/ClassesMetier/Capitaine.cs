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
    /// Implementation de la piece «Capitaine»
    /// </summary>
   public class Capitaine : PieceMobile
   {
        /// <summary>
        /// Construit un capitaine
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Capitaine(Couleur couleurPiece) : base(couleurPiece, 6, "capitaine")
      {    
      }
   }
}
