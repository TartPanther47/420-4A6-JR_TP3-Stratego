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
    /// Générateur de commandant
    /// </summary>
    public class GenerateurCommandant : GenerateurPiece
    {
        /// <summary>
        /// Construit un commandant
        /// </summary>
        public GenerateurCommandant() : base(3) {}

        /// <summary>
        /// Crée un commandant
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>Le commandant créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Commandant(couleur);
    }
}
