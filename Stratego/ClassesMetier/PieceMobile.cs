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
    /// Une pièce mobile est une pièce, mais elle a une force et elle peut se déplacer
    /// </summary>
    public abstract class PieceMobile : Piece
    {
        public int Force { get; private set; }

        /// <summary>
        /// Construit une pièce mobile
        /// </summary>
        /// <param name="couleurPiece">La couleur de la pièce</param>
        /// <param name="forcePiece">La force de la pièce</param>
        /// <param name="nom">Le nom de la pièce (fichier sprite)</param>
        public PieceMobile(Couleur couleurPiece, int forcePiece, string nom) : base(couleurPiece, nom)
        {
            Force = forcePiece;
        }
    }
}
