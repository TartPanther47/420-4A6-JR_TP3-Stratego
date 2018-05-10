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
    /// Générateur de lieutenant
    /// </summary>
    public class GenerateurLieutenant : GenerateurPiece
    {
        /// <summary>
        /// Construit un lieutenant
        /// </summary>
        public GenerateurLieutenant() : base(4) {}

        /// <summary>
        /// Crée un lieutenant
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>Le lieutenant créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Lieutenant(couleur);
    }
}
