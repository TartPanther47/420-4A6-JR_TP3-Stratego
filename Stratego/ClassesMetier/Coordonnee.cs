using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego.ClassesMetier
{
   public class Coordonnee
   {
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
   }
}
