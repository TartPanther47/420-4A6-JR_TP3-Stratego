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
    /// Générateur de capitaine
    /// </summary>
    public class GenerateurCapitaine : GenerateurPiece
    {
        /// <summary>
        /// Construit un capitaine
        /// </summary>
        public GenerateurCapitaine() : base(4) { }

        /// <summary>
        /// Crée un capitaine
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>Le capitaine créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Capitaine(couleur);
    }
}
