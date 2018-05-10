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
    /// Classe de transport pour les couleurs du joueur et de l'IA
    /// </summary>
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

        /// <summary>
        /// Construit une instance de paramètres de couleurs de joueurs
        /// </summary>
        /// <param name="couleurJoueur">La couleur du joueur</param>
        public ParametresCouleurJoueurs(Couleur couleurJoueur) => CouleurJoueur = couleurJoueur;
    }
}
