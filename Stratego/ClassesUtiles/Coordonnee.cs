using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
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

        public Coordonnee(int x, int y)
        {
           X = x;
           Y = y;
        }

        #endregion

        #region Methodes

        public override bool Equals(object obj)
        {
            var coordonnee = obj as Coordonnee;
            return coordonnee != null &&
                   X == coordonnee.X &&
                   Y == coordonnee.Y;
        }

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
