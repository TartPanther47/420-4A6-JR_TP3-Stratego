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
    /// Générateur de bombe
    /// </summary>
    public class GenerateurBombe : GenerateurPiece
    {
        /// <summary>
        /// Construit une bombe
        /// </summary>
        public GenerateurBombe() : base(6) { }

        /// <summary>
        /// Crée une bombe
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>La bombe créée</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Bombe(couleur);
    }
}
