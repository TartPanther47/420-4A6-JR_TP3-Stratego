// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stratego
{
    /// <summary>
    /// Wrapper contenant un générateur de pièces, ainsi que son affichage (incluant le nombre de pièces restantes).
    /// </summary>
    public class AffichageGenerateurPiece
    {
        #region Attributs
        public GenerateurPieceAffichable Generateur { get; }
        public Rectangle Control { get; }
        public Label LabelNbPieces { get; }
        public string Uri { get; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Construit un conteneur de generateur affichable
        /// </summary>
        /// <param name="generateur">Le générateur de pièces</param>
        /// <param name="nomSprite">Le sprite du type de la pièce</param>
        /// <param name="couleur">La couleur de la pièce</param>
        public AffichageGenerateurPiece(GenerateurPieceAffichable generateur, string nomSprite, Couleur couleur)
        {
            Generateur = generateur;
            Uri = "sprites/" + (couleur == Couleur.Rouge ? "Rouge/" : "Bleu/") + nomSprite + ".png";
            Control = new Rectangle
            {
                Fill = new ImageBrush(new BitmapImage(new Uri(Uri, UriKind.Relative)))
            };
            LabelNbPieces = new Label()
            {
                Foreground = Brushes.White,
                FontSize = 24
            };
            ActualiserLabel();
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Permet de modifier un attribut (ex. l'affichage) en ayant le générateur dans la même portée.
        /// </summary>
        /// <param name="methodeRetour">Methode qui sera appelée, prend en paramètres les membres du wrapper</param>
        public void Modifier(Action<GenerateurPieceAffichable, string, Rectangle> methodeRetour)
            => methodeRetour(Generateur, Uri, Control);

        /// <summary>
        /// Change l'affichage du nombre de pièces pour qu'il corresponde au nombre dans le générateur.
        /// </summary>
        public void ActualiserLabel() => LabelNbPieces.Content = Generateur.Generateur.Nombre;
        #endregion
    }
}
