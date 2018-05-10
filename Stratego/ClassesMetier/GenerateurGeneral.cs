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
    /// Générateur de général
    /// </summary>
    public class GenerateurGeneral : GenerateurPiece
    {
        /// <summary>
        /// Construit un général
        /// </summary>
        public GenerateurGeneral() : base(1) {}

        /// <summary>
        /// Crée un général
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>Le général créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new General(couleur);
    }
}
