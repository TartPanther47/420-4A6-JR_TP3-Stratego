using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Règle de placement #1.
    ///     Le drapeau est placé sur la première ligne (la plus éloignée de l'ennemi),
    ///     et sous un lac (pour ne pas être directement accessible).
    /// </summary>
    public class StrategiePlacementPieceDrapeau : StrategiePlacementPiece
    {
        public StrategiePlacementPieceDrapeau(Piece[,] pieces, int largeur, int hauteur) : base(pieces, largeur, hauteur) {}

        public override Coordonnee GetPosition()
        {
            List<Coordonnee> positionsPossibles = new List<Coordonnee>
            {
                new Coordonnee(2, 0),
                new Coordonnee(3, 0),
                new Coordonnee(6, 0),
                new Coordonnee(7, 0)
            };

            return (positionsPossibles.Count > 0 ?
                positionsPossibles[Aleatoire.Next(positionsPossibles.Count)] :
                new StrategiePlacementPieceAleatoire(Pieces, Largeur, Hauteur).GetPosition());
        }
    }
}
