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
    /// Générateur de pièce nulle
    /// </summary>
    public class GenerateurNul : GenerateurPiece
    {
        /// <summary>
        /// Construit une pièce nulle
        /// </summary>
        public GenerateurNul() : base(0) {}

        /// <summary>
        /// Crée une pièce nulle
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>La pièce nulle créée</returns>
        protected override Piece CreerPiece(Couleur couleur) => new PieceNulle(couleur);
    }
}
