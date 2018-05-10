// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Règle de placement #3.
    ///     Trois bombes sont placées à gauche, à droite et en dessous du drapeau,
    ///     et les autres sont placées aléatoirement.
    /// </summary>
    public class StrategiePlacementPieceBombe : StrategiePlacementPiece
    {
        private Coordonnee PositionDrapeau { get; set; }
        private bool DrapeauTrouve { get; set; }

        /// <summary>
        /// Construit une strategie de placement de pièces de bombes
        /// </summary>
        /// <param name="pieces">Grille de pièces où placer</param>
        /// <param name="largeur">Largeur de la grille</param>
        /// <param name="hauteur">Hauteur de la grille</param>
        public StrategiePlacementPieceBombe(Piece[,] pieces, int largeur, int hauteur)
            : base(pieces, largeur, hauteur)
        { DrapeauTrouve = false; }

        /// <summary>
        /// Détermine la position d'une pièce selon la stratégie
        /// </summary>
        /// <returns>Coordonnée de la position du coup à faire</returns>
        public override Coordonnee GetPosition()
        {
                // Si on n'a pas trouvé le drapeau, le trouver
            if(!DrapeauTrouve)
            {
                for (int x = 0; x < Largeur; x++)
                {
                    for (int y = 0; y < Hauteur; y++)
                    {
                        if (Pieces[x, y] is Drapeau)
                        {
                            PositionDrapeau = new Coordonnee(x, y);
                            DrapeauTrouve = true;
                        }
                    }
                }
            }

            List<Coordonnee> positionsPossibles = new List<Coordonnee>();
            if (Pieces[PositionDrapeau.X - 1, 0] == null)
                positionsPossibles.Add(new Coordonnee(PositionDrapeau.X - 1, 0));
            if (Pieces[PositionDrapeau.X, 1] == null)
                positionsPossibles.Add(new Coordonnee(PositionDrapeau.X, 1));
            if (Pieces[PositionDrapeau.X + 1, 0] == null)
                positionsPossibles.Add(new Coordonnee(PositionDrapeau.X + 1, 0));

            return (positionsPossibles.Count > 0 ?
                positionsPossibles[Aleatoire.Next(positionsPossibles.Count)] :
                new StrategiePlacementPieceAleatoire(Pieces, Largeur, Hauteur).GetPosition());
        }
    }
}
