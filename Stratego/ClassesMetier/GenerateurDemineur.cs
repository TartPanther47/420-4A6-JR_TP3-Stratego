using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class GenerateurDemineur : GenerateurPiece
    {
        public GenerateurDemineur() : base(5) { }

        protected override Piece CreerPiece(Couleur couleur) => new Demineur(couleur);
    }
}
