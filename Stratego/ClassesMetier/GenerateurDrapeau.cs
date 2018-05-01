using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class GenerateurDrapeau : GenerateurPiece
    {
        public GenerateurDrapeau() : base(1) { }

        protected override Piece CreerPiece(Couleur couleur) => new Drapeau(couleur);
    }
}
