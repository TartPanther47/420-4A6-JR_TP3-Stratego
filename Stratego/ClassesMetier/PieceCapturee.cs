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
    public class PieceCapturee
    {
        private const int TAILLE_PIECES = 42;

        private int NombrePieces { get; set; }
        private Label LabelNbPieces { get; set; }
        public Piece Piece { get; private set; }
        
        public PieceCapturee(Piece piece, StackPanel stpParent)
        {
            Piece = piece;

            Grid grdCellule = new Grid();
            grdCellule.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(TAILLE_PIECES) });
            grdCellule.ColumnDefinitions.Add(new ColumnDefinition());
            grdCellule.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(TAILLE_PIECES) });

            grdCellule.Children.Add(new Rectangle
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
            grdCellule.Children.Add(LabelNbPieces);

            stpParent.Children.Add(grdCellule);
        }

        public void Incrementer() => LabelNbPieces.Content = (int)LabelNbPieces.Content + 1;
    }
}
