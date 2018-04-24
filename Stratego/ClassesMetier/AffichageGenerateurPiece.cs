using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Stratego
{
    public class AffichageGenerateurPiece
    {
        public GenerateurPiece Generateur { get; }
        public Rectangle Control { get; }

        public AffichageGenerateurPiece(GenerateurPiece generateur, Rectangle control)
        {
            Generateur = generateur;
            Control = control;
        }
    }
}
