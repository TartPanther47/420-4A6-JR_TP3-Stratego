using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Stratego
{
    public class ReponseGenerateurPiece
    {
        public Piece Piece { get; set; }
        public Rectangle Affichage { get; set; }
        public string Nom { get; set; }

        public ReponseGenerateurPiece(Piece piece, Rectangle affichage, string nom)
        {
            Piece = piece;
            Affichage = affichage;
            Nom = nom;
        }
    }
}
