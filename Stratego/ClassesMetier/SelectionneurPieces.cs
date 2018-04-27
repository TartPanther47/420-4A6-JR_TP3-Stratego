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
using System.Windows.Threading;

namespace Stratego
{
    public class SelectionneurPieces
    {
        private const int TAILLE_PIECE = 42;
        private const int RAYON_CERCLE = 128;
        private const int NB_FRAMES_ANIMATION_CERCLE = 24;

        private List<GenerateurPieceAffichable> generateurs = new List<GenerateurPieceAffichable>
        {
            new GenerateurPieceAffichable(new GenerateurMarechal(), "marechal"),
            new GenerateurPieceAffichable(new GenerateurGeneral(), "general"),
            new GenerateurPieceAffichable(new GenerateurColonel(), "colonel"),
            new GenerateurPieceAffichable(new GenerateurNul(), "commandant"),
            new GenerateurPieceAffichable(new GenerateurNul(), "capitaine"),
            new GenerateurPieceAffichable(new GenerateurNul(), "lieutenant"),
            new GenerateurPieceAffichable(new GenerateurNul(), "sergent"),
            new GenerateurPieceAffichable(new GenerateurNul(), "demineur"),
            new GenerateurPieceAffichable(new GenerateurNul(), "eclaireur"),
            new GenerateurPieceAffichable(new GenerateurNul(), "espion"),
            new GenerateurPieceAffichable(new GenerateurNul(), "drapeau"),
            new GenerateurPieceAffichable(new GenerateurNul(), "bombe")
        };
        private Action<Piece, Rectangle> MethodeRetourDemandePiece { get; set; }

        private Grid grilleParente;
        private Canvas canvasInterface;

        private int framePresenteAnimation;
        private DispatcherTimer timerAnimationCercle = new DispatcherTimer();

        public SelectionneurPieces(Grid grille)
        {
            grilleParente = grille;
            MethodeRetourDemandePiece = null;
            framePresenteAnimation = 0;

            canvasInterface = new Canvas();
            Canvas.SetZIndex(canvasInterface, 2);

            for (int i = 0; i < generateurs.Count; i++)
            {
                generateurs[i].Affichage.Control.Width = TAILLE_PIECE;
                generateurs[i].Affichage.Control.Height = TAILLE_PIECE;
                generateurs[i].Affichage.Control.Cursor = Cursors.Hand;

                generateurs[i].Affichage.Modifier((GenerateurPiece generateur, string uriSprite, Rectangle control) =>
                {
                    control.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
                    {
                        CacherInterface();
                        MethodeRetourDemandePiece(generateur.GenererPiece(ParametresJeu.CouleurJoueur), new Rectangle
                        {
                            Fill = new ImageBrush(new BitmapImage(new Uri(uriSprite, UriKind.Relative)))
                        });
                    };
                });

                canvasInterface.Children.Add(generateurs[i].Affichage.Control);
            }

            timerAnimationCercle.Interval = TimeSpan.FromMilliseconds(16 + ((double)2 / 3));
            timerAnimationCercle.Tick += ActualiserAnimationInterface;

            CacherInterface();
            grille.Children.Add(canvasInterface);
        }

        private void ActualiserAnimationInterface(object sender, EventArgs e)
        {
            for (int i = 0; i < generateurs.Count; i++)
            {
                double indexCyclique = ((double)i / (((double)framePresenteAnimation / NB_FRAMES_ANIMATION_CERCLE) * generateurs.Count)) * Math.PI * 2;

                Canvas.SetLeft(generateurs[i].Affichage.Control, (grilleParente.Width / 2 - TAILLE_PIECE / 2) + Math.Cos(indexCyclique) * RAYON_CERCLE);
                Canvas.SetTop(generateurs[i].Affichage.Control, (grilleParente.Height / 2 - TAILLE_PIECE / 2) + Math.Sin(indexCyclique) * RAYON_CERCLE);
            }
            if (framePresenteAnimation >= NB_FRAMES_ANIMATION_CERCLE)
                timerAnimationCercle.Stop();
            else
                ++framePresenteAnimation;
        }

        private void AfficherInterface()
        {
            canvasInterface.Visibility = Visibility.Visible;
            framePresenteAnimation = 0;
            timerAnimationCercle.Start();
        }

        private void CacherInterface() => canvasInterface.Visibility = Visibility.Collapsed;

        public void DemanderPiece(Action<Piece, Rectangle> methodeRetour)
        {
            MethodeRetourDemandePiece = methodeRetour;
            AfficherInterface();
        }
    }
}
