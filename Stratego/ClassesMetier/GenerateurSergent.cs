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
    /// Générateur de sergent
    /// </summary>
    public class GenerateurSergent : GenerateurPiece
    {
        /// <summary>
        /// Construit un sergent
        /// </summary>
        public GenerateurSergent() : base(4) {}

        /// <summary>
        /// Crée un sergent
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>Le sergent créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Sergent(couleur);
    }
}
