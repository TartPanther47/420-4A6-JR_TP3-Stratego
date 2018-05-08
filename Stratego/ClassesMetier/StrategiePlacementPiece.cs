using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public abstract class StrategiePlacementPiece
    {
        protected int Largeur { get; set; }
        protected int Hauteur{ get; set; }
        protected Piece[,] Pieces { get; set; }

        protected Random Aleatoire { get; set; }

        public abstract Coordonnee GetPosition();

        public StrategiePlacementPiece(Piece[,] pieces, int largeur, int hauteur)
        {
            Pieces = pieces;
            Largeur = largeur;
            Hauteur = hauteur;
            Aleatoire = new Random(DateTime.Now.Millisecond);
        }
    }
}
