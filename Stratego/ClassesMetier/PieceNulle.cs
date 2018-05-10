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
    /// Implementation d'une la piece «Nulle»
    /// </summary>
    public class PieceNulle : Piece
    {
        /// <summary>
        /// Construit une pièce nulle
        /// </summary>
        /// <param name="couleurPiece">Couleur du joueur</param>
        public PieceNulle(Couleur couleurPiece) : base(couleurPiece, "nulle") {}
    }
}
