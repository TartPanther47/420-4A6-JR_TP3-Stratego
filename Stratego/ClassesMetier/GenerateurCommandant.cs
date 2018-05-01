using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class GenerateurCommandant : GenerateurPiece
    {
        public GenerateurCommandant() : base(3) {}

        protected override Piece CreerPiece(Couleur couleur) => new Commandant(couleur);
    }
}
