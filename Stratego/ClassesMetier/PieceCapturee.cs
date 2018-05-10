// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stratego
{
    /// <summary>
    /// Wrapper de pièce capturée, contient une pièce, un nombre de pièces et un affichage
    /// </summary>
    public class PieceCapturee
    {
        #region Statiques / constantes
        private const int TAILLE_PIECES = 42;
        #endregion

        #region Attributs
        private int NombrePieces { get; set; }
        private Label LabelNbPieces { get; set; }
        private Grid GrdCellule { get; set; }
        private StackPanel StpParent { get; set; }
        public Piece Piece { get; private set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Construit une « pièce capturée »
        /// </summary>
        /// <param name="piece">Pièce capturée</param>
        /// <param name="stpParent">Stack panel dans lequel la pièce est mise</param>
        public PieceCapturee(Piece piece, StackPanel stpParent)
        {
            StpParent = stpParent;
            Piece = piece;

                // On crée une grille, dans laquelle on met l'image de la pièce et un label (pour le nombre de pièces)

            GrdCellule = new Grid();
            GrdCellule.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(TAILLE_PIECES) });
            GrdCellule.ColumnDefinitions.Add(new ColumnDefinition());
            GrdCellule.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(TAILLE_PIECES) });
            
            GrdCellule.Children.Add(new Rectangle
            {
                Fill = new ImageBrush(new BitmapImage(new Uri(
                    "sprites/" + (piece.EstDeCouleur(Couleur.Rouge) ? "Rouge/" : "Bleu/") + piece.Nom + ".png",
                    UriKind.Relative)))
            });

            LabelNbPieces = new Label()
            {
                Content = 1,
                FontSize = 12
            };
            Grid.SetColumn(LabelNbPieces, 1);
            GrdCellule.Children.Add(LabelNbPieces);

            stpParent.Children.Add(GrdCellule);
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Incrémente le nombre de pièces (si on a déjà capturé une, deux ou plusieurs pièces du type de la pièce contenue dans le wrapper)
        /// </summary>
        public void Incrementer() => LabelNbPieces.Content = (int)LabelNbPieces.Content + 1;

        /// <summary>
        /// Retire les éléments graphiques de l'interface 
        /// </summary>
        public void Effacer() => StpParent.Children.Remove(GrdCellule);
        #endregion
    }
}
