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
    /// Générateur de drapeau
    /// </summary>
    public class GenerateurDrapeau : GenerateurPiece
    {
        /// <summary>
        /// Construit un drapeau
        /// </summary>
        public GenerateurDrapeau() : base(1) { }

        /// <summary>
        /// Crée un drapeau
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>Le drapeau créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Drapeau(couleur);
    }
}
