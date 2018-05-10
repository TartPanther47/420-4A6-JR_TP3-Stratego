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
    /// Générateur d'espion
    /// </summary>
    public class GenerateurEspion : GenerateurPiece
    {
        /// <summary>
        /// Construit un espion
        /// </summary>
        public GenerateurEspion() : base(1) { }

        /// <summary>
        /// Crée un espion
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>L'espion créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Espion(couleur);
    }
}
