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
    /// Classe abstraite « Générateur de pièce » permet de créer des pièces tant qu'il y en reste
    /// </summary>
    public abstract class GenerateurPiece
    {
        #region Attributs
        private int NombreMax { get; set; }
            // Le nombre de pièces restantes de ce type
        public int Nombre { get; private set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Construit un générateur de pièces
        /// </summary>
        /// <param name="nombre">Le nombre de pièces disponible en tout</param>
        public GenerateurPiece(int nombre)
        {
            Nombre = nombre;
            NombreMax = nombre;
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Détermine si on peut générer une pièce
        /// </summary>
        /// <returns></returns>
        public bool EstGenerable() => Nombre > 0;

        /// <summary>
        /// Crée une pièce (méthode abstraite)
        /// </summary>
        /// <param name="couleur">Couleur de la pièce</param>
        /// <returns>La pièce créée</returns>
        protected abstract Piece CreerPiece(Couleur couleur);

        /// <summary>
        /// Crée une pièce (méthode publique), si on peut en générer une (sinon, une pièce nulle)
        /// </summary>
        /// <param name="couleur">Couleur de la pièce</param>
        /// <returns>La pièce créée ou une pièce nulle selon si on peut générer ou non</returns>
        public Piece GenererPiece(Couleur couleur)
        {
            if(EstGenerable())
            {
                --Nombre;
                return CreerPiece(couleur);
            }
            return new PieceNulle(couleur);
        }

        /// <summary>
        /// Incrémenter le nombre de pièces disponibles (quand on redonne une pièce générée)
        /// </summary>
        public void IncrementerNombre()
        {
            if (Nombre < NombreMax) ++Nombre;
        }
        #endregion
    }
}
