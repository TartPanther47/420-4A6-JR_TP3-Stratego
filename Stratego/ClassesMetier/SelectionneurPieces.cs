using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stratego
{
    public class SelectionneurPieces
    {
        private const int TAILLE_PIECE = 42;

        private List<GenerateurPieceAffichable> generateurs = new List<GenerateurPieceAffichable>
        {
            new GenerateurPieceAffichable(
                new GenerateurMarechal(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/marechal.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurGeneral(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/general.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurColonel(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/colonel.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurGeneral(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/commandant.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurGeneral(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/capitaine.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurGeneral(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/lieutenant.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurMarechal(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/sergent.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurNul(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/demineur.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurNul(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/eclaireur.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurNul(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/espion.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurNul(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/drapeau.png", UriKind.Relative))) }
            ),
            new GenerateurPieceAffichable(
                new GenerateurNul(),
                new Rectangle { Fill = new ImageBrush(new BitmapImage(new Uri("sprites/bombe.png", UriKind.Relative))) }
            )
        };
        private Action<Piece> MethodeRetourDemandePiece { get; set; }

        private Canvas canvasInterface;

        public SelectionneurPieces(Grid grille)
        {
            MethodeRetourDemandePiece = null;

            canvasInterface = new Canvas();
            Canvas.SetZIndex(canvasInterface, 2);

            for (int i = 0; i < generateurs.Count; i++)
            {
                generateurs[i].Affichage.Control.Width = TAILLE_PIECE;
                generateurs[i].Affichage.Control.Height = TAILLE_PIECE;
                generateurs[i].Affichage.Control.Cursor = Cursors.Hand;

                generateurs[i].Affichage.Modifier((GenerateurPiece generateur, Rectangle control) =>
                {
                    control.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
                    {
                        CacherInterface();
                        MethodeRetourDemandePiece(generateur.GenererPiece(ParametresJeu.CouleurJoueur));
                    };
                });

                double indexCyclique = ((double)i / (double)generateurs.Count) * Math.PI * 2;

                double rayon = 128;

                Canvas.SetLeft(generateurs[i].Affichage.Control, (grille.Width / 2 - TAILLE_PIECE / 2) + Math.Cos(indexCyclique) * rayon);
                Canvas.SetTop(generateurs[i].Affichage.Control, (grille.Height / 2 - TAILLE_PIECE / 2) + Math.Sin(indexCyclique) * rayon);

                canvasInterface.Children.Add(generateurs[i].Affichage.Control);
            }

            CacherInterface();
            grille.Children.Add(canvasInterface);
        }

        private void AfficherInterface() => canvasInterface.Visibility = Visibility.Visible;

        private void CacherInterface() => canvasInterface.Visibility = Visibility.Collapsed;

        public void DemanderPiece(Action<Piece> methodeRetour)
        {
            MethodeRetourDemandePiece = methodeRetour;
            AfficherInterface();
        }
    }
}
