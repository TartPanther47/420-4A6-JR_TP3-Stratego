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
    public class GenerateurPieceAffichable
    {
        public GenerateurPiece Generateur { get; }
        public AffichageGenerateurPiece Affichage { get; }
        public string NomSprite { get; }

        public GenerateurPieceAffichable(GenerateurPiece generateur, string nomSprite, Couleur couleur)
        {
            Generateur = generateur;
            Affichage = new AffichageGenerateurPiece(this, nomSprite, couleur);
            NomSprite = nomSprite;
        }
    }
}
