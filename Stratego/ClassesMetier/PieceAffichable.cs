// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Stratego
{
    /// <summary>
    /// Wrapper liant une pièce à un affichage
    /// </summary>
    public class PieceAffichable
    {
        public Piece Piece { get; }
        public Rectangle Affichage { get; }
        public string Nom { get; }

        /// <summary>
        /// Crée une pièce affichable
        /// </summary>
        /// <param name="piece">La pièce</param>
        /// <param name="affichage">L'affichage de la pièce</param>
        /// <param name="nom">Le nom de la pièce (fichier sprite)</param>
        public PieceAffichable(Piece piece, Rectangle affichage, string nom)
        {
            Piece = piece;
            Affichage = affichage;
            Nom = nom;
        }
        
        /// <summary>
        /// Permet de modifier un attribut (ex. l'affichage) en ayant la pièce dans la même portée.
        /// </summary>
        /// <param name="methodeRetour">Méthode qui sera appelée, prend en paramètres les membres du wrapper</param>
        public void Modifier(Action<Piece, Rectangle, string> methodeRetour)
            => methodeRetour(Piece, Affichage, Nom);
    }
}
