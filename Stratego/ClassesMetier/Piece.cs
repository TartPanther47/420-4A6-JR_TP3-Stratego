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
    /// Classe abstraite de pièce
    /// </summary>
   public abstract class Piece
   {
      public Couleur Couleur { get; private set; }
      public string Nom { get; private set; }
      public bool EstVisible { get; set; }

        /// <summary>
        /// Construit une pièce
        /// </summary>
        /// <param name="couleurPiece">Couleur de la pièce</param>
        /// <param name="nom">Nom de la pièce</param>
      public Piece(Couleur couleurPiece, string nom)
      {
         Couleur = couleurPiece;
         Nom = nom;
         EstVisible = false;
      }

        /// <summary>
        /// Détermine si la pièce est d'une couleur spécifiée
        /// </summary>
        /// <param name="couleur">Couleur spécifiée</param>
        /// <returns>Si la pièce est de la couleur spécifiée</returns>
      public bool EstDeCouleur(Couleur couleur) => Couleur == couleur;
   }
}
