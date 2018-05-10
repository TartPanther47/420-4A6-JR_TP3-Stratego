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
    /// Implementation de la piece «Éclaireur»
    /// </summary>
   public class Eclaireur : PieceMobile
   {
        /// <summary>
        /// Construit un éclaireur
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public Eclaireur(Couleur couleurPiece) : base(couleurPiece, 2, "eclaireur")
      {                          
      }
   }
}
