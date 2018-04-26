using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class GenerateurColonel : GenerateurPiece
    {
        public GenerateurColonel() : base(2) { }

        protected override Piece CreerPiece(Couleur couleur) => new Colonel(couleur);
    }
}
