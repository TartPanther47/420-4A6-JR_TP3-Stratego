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
    public class AffichageGenerateurPiece
    {
        public GenerateurPieceAffichable Generateur { get; }
        public Rectangle Control { get; }
        public Label LabelNbPieces { get; }
        public string Uri { get; }

        public AffichageGenerateurPiece(GenerateurPieceAffichable generateur, string nomSprite)
        {
            Generateur = generateur;
            Uri = "sprites/" + nomSprite + ".png";
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

        public void Modifier(Action<GenerateurPieceAffichable, string, Rectangle> methodeRetour)
            => methodeRetour(Generateur, Uri, Control);

        public void ActualiserLabel() => LabelNbPieces.Content = Generateur.Generateur.Nombre;
    }
}
