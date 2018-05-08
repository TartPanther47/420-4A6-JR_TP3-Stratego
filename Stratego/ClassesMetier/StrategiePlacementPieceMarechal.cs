using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Règle de placement #3.
    ///     Le maréchal est placé en arrière d'un lac, comme ça il est disponible rapidement, mais il n'est pas
    ///     directement exposé.
    /// </summary>
    public class StrategiePlacementPieceMarechal : StrategiePlacementPiece
    {
        public StrategiePlacementPieceMarechal(Piece[,] pieces, int largeur, int hauteur) : base(pieces, largeur, hauteur) {}

        public override Coordonnee GetPosition()
        {
            List<Coordonnee> positionsPossibles = new List<Coordonnee>
            {
                new Coordonnee(2, 3),
                new Coordonnee(3, 3),
                new Coordonnee(6, 3),
                new Coordonnee(7, 3)
            };

            return (positionsPossibles.Count > 0 ?
                positionsPossibles[Aleatoire.Next(positionsPossibles.Count)] :
                new StrategiePlacementPieceAleatoire(Pieces, Largeur, Hauteur).GetPosition());
        }
    }
}
