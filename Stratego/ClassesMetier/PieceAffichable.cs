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
        public string Nom { get; }

        public PieceAffichable(Piece piece, Rectangle affichage, string nom)
        {
            Piece = piece;
            Affichage = affichage;
            Nom = nom;
        }

        public void Modifier(Action<Piece, Rectangle, string> methodeRetour)
            => methodeRetour(Piece, Affichage, Nom);
    }
}
