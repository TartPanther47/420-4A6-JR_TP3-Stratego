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
    /// Générateur de maréchal
    /// </summary>
    public class GenerateurMarechal : GenerateurPiece
    {
        /// <summary>
        /// Construit un maréchal
        /// </summary>
        public GenerateurMarechal() : base(1) {}

        /// <summary>
        /// Crée un maréchal
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>Le maréchal créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Marechal(couleur);
    }
}
