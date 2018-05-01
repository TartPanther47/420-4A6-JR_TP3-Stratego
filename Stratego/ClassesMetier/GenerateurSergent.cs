using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class GenerateurSergent : GenerateurPiece
    {
        public GenerateurSergent() : base(4) {}

        protected override Piece CreerPiece(Couleur couleur) => new Sergent(couleur);
    }
}
