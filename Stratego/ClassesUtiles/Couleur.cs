using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public enum Couleur
    {
        Null = -1,
        Rouge = 0,
        Bleu = 1
    }

    public static class CouleurMethodes
    {
        public static Couleur Inverse(this Couleur couleur)
        {
            if (couleur == Couleur.Bleu)
                return Couleur.Rouge;
            else if (couleur == Couleur.Rouge)
                return Couleur.Bleu;
            else
                return Couleur.Null;
        }
    }
}
