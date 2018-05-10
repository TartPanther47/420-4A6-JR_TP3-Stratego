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
    /// Classe de transport qui contient une pièce qui a été générée, son affichage et le nom de son sprite
    /// </summary>
    public class ReponseGenerateurPiece
    {
        public Piece Piece { get; set; }
        public Rectangle Affichage { get; set; }
        public string Nom { get; set; }

        /// <summary>
        /// Construit une réponse de générateur de pièces
        /// </summary>
        /// <param name="piece">La pièce générée</param>
        /// <param name="affichage">L'affichage de la pièce générée</param>
        /// <param name="nom">Le nom de la pièce (fichier sprite)</param>
        public ReponseGenerateurPiece(Piece piece, Rectangle affichage, string nom)
        {
            Piece = piece;
            Affichage = affichage;
            Nom = nom;
        }
    }
}
