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
    class GenerateurPieceAffichable
    {
        public GenerateurPiece Generateur { get; }
        public AffichageGenerateurPiece Affichage { get; }

        public GenerateurPieceAffichable(GenerateurPiece generateur, string nomSprite)
        {
            Generateur = generateur;
            Affichage = new AffichageGenerateurPiece(Generateur, nomSprite);
        }
    }
}
