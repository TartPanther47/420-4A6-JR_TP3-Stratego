using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class GenerateurGeneral : GenerateurPiece
    {
        public GenerateurGeneral() : base(1) {}

        protected override Piece CreerPiece(Couleur couleur) => new General(couleur);
    }
}
