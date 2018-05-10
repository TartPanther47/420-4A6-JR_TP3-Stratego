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
    /// Générateur d'éclaireur
    /// </summary>
    public class GenerateurEclaireur : GenerateurPiece
    {
        /// <summary>
        /// Construit un éclaireur
        /// </summary>
        public GenerateurEclaireur() : base(8) { }

        /// <summary>
        /// Crée un éclaireur
        /// </summary>
        /// <param name="couleur">La couleur de la pièce</param>
        /// <returns>L'éclaireur créé</returns>
        protected override Piece CreerPiece(Couleur couleur) => new Eclaireur(couleur);
    }
}
