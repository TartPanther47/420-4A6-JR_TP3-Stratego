using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class ParametresPiecesJoueur : ParametresConstruction
    {
        public List<Piece> Pieces { get; private set; }
        public ParametresPiecesJoueur(PieceAffichable[,] pieces, int largeur, int hauteur)
        {
            Pieces = new List<Piece>();
            for (int y = 0; y < hauteur; y++)
                for (int x = 0; x < largeur; x++)
                    Pieces.Add(pieces[x, y].Piece);
        }
    }
}
