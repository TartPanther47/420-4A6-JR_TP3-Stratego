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
    /// Générateur de démineur
    /// </summary>
    public class GenerateurDemineur : GenerateurPiece
    {
        /// <summary>
        /// Construit un démineur
        /// </summary>
        public GenerateurDemineur() : base(5) { }

        /// <summary>
        /// Crée un démineur
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>Le démineur créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Demineur(couleur);
    }
}
