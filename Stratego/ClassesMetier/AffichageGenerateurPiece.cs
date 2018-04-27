using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stratego
{
    public class AffichageGenerateurPiece
    {
        public GenerateurPiece Generateur { get; }
        public Rectangle Control { get; }
        public string Uri { get; }

        public AffichageGenerateurPiece(GenerateurPiece generateur, string nomSprite)
        {
            Generateur = generateur;
            Uri = "sprites/" + nomSprite + ".png";
            Control = new Rectangle
            {
                Fill = new ImageBrush(new BitmapImage(new Uri(Uri, UriKind.Relative)))
            };
        }

        public void Modifier(Action<GenerateurPiece, string, Rectangle> methodeRetour)
            => methodeRetour(Generateur, Uri, Control);
    }
}
