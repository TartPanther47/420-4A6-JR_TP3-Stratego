﻿// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Classe de transport pour l'ordre des pièces choisie
    /// </summary>
    public class ParametresPiecesJoueur : ParametresConstruction
    {
        public List<Piece> Pieces { get; private set; }

        /// <summary>
        /// Construit une instance de paramètres d'ordre des pièces
        /// </summary>
        /// <param name="pieces">Liste des pièces</param>
        /// <param name="largeur">Largeur de la grille</param>
        /// <param name="hauteur">Hauteur de la grille</param>
        public ParametresPiecesJoueur(PieceAffichable[,] pieces, int largeur, int hauteur)
        {
            Pieces = new List<Piece>();
            for (int y = 0; y < hauteur; y++)
                for (int x = 0; x < largeur; x++)
                    Pieces.Add(pieces[x, y].Piece);
        }
    }
}
