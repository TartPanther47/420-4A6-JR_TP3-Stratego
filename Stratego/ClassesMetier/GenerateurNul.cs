using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class GenerateurNul : GenerateurPiece
    {
        public GenerateurNul() : base(0) {}

        protected override Piece CreerPiece(Couleur couleur) => new PieceNulle(couleur);
    }
}
