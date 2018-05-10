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
    /// Implementation de la piece «Maréchal»
    /// </summary>
   public class Marechal : PieceMobile
   {
        /// <summary>
        /// Construit un maréchal
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Marechal(Couleur couleurPiece) : base(couleurPiece, 10, "marechal")
      {
      }
      
   }
}
