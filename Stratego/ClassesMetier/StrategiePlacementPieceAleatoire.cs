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
    /// Règle de placement #4.
    ///     Les autres pièces sont placées aléatoirement.
    /// </summary>
    public class StrategiePlacementPieceAleatoire : StrategiePlacementPiece
    {
        /// <summary>
        /// Construit une strategie de placement de pièces aléatoire
        /// </summary>
        /// <param name="pieces">Grille de pièces où placer</param>
        /// <param name="largeur">Largeur de la grille</param>
        /// <param name="hauteur">Hauteur de la grille</param>
        public StrategiePlacementPieceAleatoire(Piece[,] pieces, int largeur, int hauteur) : base(pieces, largeur, hauteur) {}

        /// <summary>
        /// Détermine la position d'une pièce selon la stratégie
        /// </summary>
        /// <returns>Coordonnée de la position du coup à faire</returns>
        public override Coordonnee GetPosition()
        {
            List<Coordonnee> positionsPossibles = new List<Coordonnee>();

            for (int x = 0; x < Largeur; x++)
                for (int y = 0; y < Hauteur; y++)
                    if (Pieces[x, y] == null)
                        positionsPossibles.Add(new Coordonnee(x, y));

            return (positionsPossibles.Count > 0 ?
                positionsPossibles[Aleatoire.Next(positionsPossibles.Count)] :
                new Coordonnee(-1, -1));
        }
    }
}
