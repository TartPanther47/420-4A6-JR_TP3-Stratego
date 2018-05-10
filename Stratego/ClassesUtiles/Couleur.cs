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
    /// Énumératon des couleurs
    /// </summary>
    public enum Couleur
    {
        Null = -1,
        Rouge = 0,
        Bleu = 1
    }

    /// <summary>
    /// Méthodes de manipulation des couleurs
    /// </summary>
    public static class CouleurMethodes
    {
        /// <summary>
        /// Inverser une couleur
        /// </summary>
        /// <param name="couleur">Couleur à inverser</param>
        /// <returns>L'inverse de la couleur à inverser</returns>
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
