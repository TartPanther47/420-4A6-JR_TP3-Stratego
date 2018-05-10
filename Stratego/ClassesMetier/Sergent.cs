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
    /// Implementation de la piece «Sergent»
    /// </summary>
   public class Sergent : PieceMobile
   {
        /// <summary>
        /// Construit un sergent
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Sergent(Couleur couleurPiece) : base(couleurPiece, 4, "sergent")
      {                        
      }
   }
}
