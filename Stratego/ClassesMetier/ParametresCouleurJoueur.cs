using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class ParametresCouleurJoueurs : ParametresConstruction
    {
        private Couleur couleurJoueur = Couleur.Null;

        public Couleur CouleurJoueur
        {
            get { return couleurJoueur; }
            set { couleurJoueur = value; }
        }
        public Couleur CouleurIA
        {
            get { return couleurJoueur.Inverse(); }
            set { couleurJoueur = value.Inverse(); }
        }

        public ParametresCouleurJoueurs(Couleur couleurJoueur) => CouleurJoueur = couleurJoueur;
    }
}
