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
    /// Générateur de colonel
    /// </summary>
    public class GenerateurColonel : GenerateurPiece
    {
        /// <summary>
        /// Construit un colonel
        /// </summary>
        public GenerateurColonel() : base(2) { }

        /// <summary>
        /// Crée un colonel
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>Le colonel créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Colonel(couleur);
    }
}
