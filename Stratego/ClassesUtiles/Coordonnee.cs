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
    /// Coordonnée bidimensionnelle entière
    /// </summary>
   public class Coordonnee
   {
        #region Statiques

        public static bool operator ==(Coordonnee a, Coordonnee b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Coordonnee a, Coordonnee b) => a.X != b.X || a.Y != b.Y;

        #endregion

        #region Attributs

        public int X { get; set; }
        public int Y { get; set; }

        #endregion

        #region Contructeur

        /// <summary>
        /// Construit une coordonnée
        /// </summary>
        /// <param name="x">Abscisse</param>
        /// <param name="y">Ordonnée</param>
        public Coordonnee(int x, int y)
        {
           X = x;
           Y = y;
        }

        #endregion

        #region Methodes

        /// <summary>
        /// Comparaison d'égalité
        /// </summary>
        /// <param name="obj">Coordonée avec laquelle comparer</param>
        /// <returns>Si les deux coordonnées sont égales</returns>
        public override bool Equals(object obj)
        {
            var coordonnee = obj as Coordonnee;
            return coordonnee != null &&
                   X == coordonnee.X &&
                   Y == coordonnee.Y;
        }

        /// <summary>
        /// Obtenir hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        #endregion
    }
}
