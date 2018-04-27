using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Stratego
{
    public class PieceAffichable
    {
        public Piece Piece { get; }
        public Rectangle Affichage { get; }

        public PieceAffichable(Piece piece, Rectangle affichage)
        {
            Piece = piece;
            Affichage = affichage;
        }
    }
}
