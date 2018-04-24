using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class ParametresJeu
    {
        private static Couleur couleurJoueur = Couleur.Null;

        public static Couleur CouleurJoueur
        {
            get { return couleurJoueur; }
            set { couleurJoueur = value; }
        }
        public static Couleur CouleurIA
        {
            get { return couleurJoueur.Inverse(); }
            set { couleurJoueur = value.Inverse(); }
        }
    }
}
