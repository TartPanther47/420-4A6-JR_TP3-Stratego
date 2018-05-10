// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stratego
{
    /// <summary>
    /// Wrapper de générateur de pièce, lié avec un affichage
    /// </summary>
    public class GenerateurPieceAffichable
    {
        public GenerateurPiece Generateur { get; }
        public AffichageGenerateurPiece Affichage { get; }
        public string NomSprite { get; }

        /// <summary>
        /// Construit un générateur de pièce affichable
        /// </summary>
        /// <param name="generateur">Le générateur de pièces</param>
        /// <param name="nomSprite">Le nom du sprite</param>
        /// <param name="couleur">La couleur de la pièce</param>
        public GenerateurPieceAffichable(GenerateurPiece generateur, string nomSprite, Couleur couleur)
        {
            Generateur = generateur;
            Affichage = new AffichageGenerateurPiece(this, nomSprite, couleur);
            NomSprite = nomSprite;
        }
    }
}
