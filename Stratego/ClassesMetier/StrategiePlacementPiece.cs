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
    /// Classe abstraite de stratégie de placement de pièce.
    /// </summary>
    public abstract class StrategiePlacementPiece
    {
        protected int Largeur { get; set; }
        protected int Hauteur{ get; set; }
        protected Piece[,] Pieces { get; set; }

        protected Random Aleatoire { get; set; }

        /// <summary>
        /// Construit une stratégie de placement de pièce
        /// </summary>
        /// <param name="pieces">Grille des pièces où placer déterminer les positions de pièces</param>
        /// <param name="largeur">Largeur de la grille</param>
        /// <param name="hauteur">Hauteur de la grille</param>
        public StrategiePlacementPiece(Piece[,] pieces, int largeur, int hauteur)
        {
            Pieces = pieces;
            Largeur = largeur;
            Hauteur = hauteur;
            Aleatoire = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Détermine la position d'une pièce selon la stratégie (fonction abstraite)
        /// </summary>
        /// <returns>Coordonnée de la position du coup à faire</returns>
        public abstract Coordonnee GetPosition();
    }
}
