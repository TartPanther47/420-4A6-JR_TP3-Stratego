using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class GenerateurBombe : GenerateurPiece
    {
        public GenerateurBombe() : base(6) { }

        protected override Piece CreerPiece(Couleur couleur) => new Bombe(couleur);
    }
}
