using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Stratego
{
    class GenerateurPieceAffichable
    {
        public GenerateurPiece Generateur { get; }
        public AffichageGenerateurPiece Affichage { get; }

        public GenerateurPieceAffichable(GenerateurPiece generateur, Rectangle control)
        {
            Generateur = generateur;
            Affichage = new AffichageGenerateurPiece(Generateur, control);
        }
    }
}
