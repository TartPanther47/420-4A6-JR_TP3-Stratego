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
        public const int NB_TYPES_PIECES = 12;
        private const int TAILLE_PIECE = 42;
        private const int RAYON_CERCLE = 128;
        private const int NB_FRAMES_ANIMATION_CERCLE = 8;

        private List<GenerateurPieceAffichable> generateurs = new List<GenerateurPieceAffichable>
        {
            new GenerateurPieceAffichable(new GenerateurMarechal(), "marechal"),
            new GenerateurPieceAffichable(new GenerateurGeneral(), "general"),
            new GenerateurPieceAffichable(new GenerateurColonel(), "colonel"),
            new GenerateurPieceAffichable(new GenerateurCommandant(), "commandant"),
            new GenerateurPieceAffichable(new GenerateurCapitaine(), "capitaine"),
            new GenerateurPieceAffichable(new GenerateurLieutenant(), "lieutenant"),
            new GenerateurPieceAffichable(new GenerateurSergent(), "sergent"),
            new GenerateurPieceAffichable(new GenerateurDemineur(), "demineur"),
            new GenerateurPieceAffichable(new GenerateurEclaireur(), "eclaireur"),
            new GenerateurPieceAffichable(new GenerateurEspion(), "espion"),
            new GenerateurPieceAffichable(new GenerateurDrapeau(), "drapeau"),
            new GenerateurPieceAffichable(new GenerateurBombe(), "bombe")
        };
        private List<GenerateurPieceAffichable> generateursVides { get; set; }

        Couleur CouleurJoueur { get; set; }
        private Rectangle FondEcranModal { get; set; }

        private Action<Piece, Rectangle, string> MethodeRetourDemandePiece { get; set; }

        private Grid grilleParente;
        private Canvas canvasInterface;

        private int framePresenteAnimation;
        private DispatcherTimer timerAnimationCercle = new DispatcherTimer();

        public SelectionneurPieces(Grid grille, Couleur couleurJoueur)
        {
            CouleurJoueur = couleurJoueur;

            grilleParente = grille;
            MethodeRetourDemandePiece = null;
            framePresenteAnimation = 0;
            generateursVides = new List<GenerateurPieceAffichable>();
            FondEcranModal = new Rectangle
            {
                Fill = Brushes.DarkGray,
                Opacity = 0.5,
                Width = grille.Width,
                Height = grille.Height
            };

            canvasInterface = new Canvas();
            Canvas.SetZIndex(canvasInterface, 2);

            canvasInterface.Children.Add(FondEcranModal);

            for (int i = 0; i < generateurs.Count; i++)
            {
                generateurs[i].Affichage.Control.Width = TAILLE_PIECE;
                generateurs[i].Affichage.Control.Height = TAILLE_PIECE;
                generateurs[i].Affichage.Control.Cursor = Cursors.Hand;

                generateurs[i].Affichage.Modifier((GenerateurPieceAffichable generateur, string uriSprite, Rectangle control) =>
                {
                    control.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
                    {
                        CacherInterface();
                        MethodeRetourDemandePiece(generateur.Generateur.GenererPiece(CouleurJoueur), new Rectangle
                        {
                            Fill = new ImageBrush(new BitmapImage(new Uri(uriSprite, UriKind.Relative))),
                            Cursor = Cursors.Hand
                        }, generateur.NomSprite);

                        if (generateur.Generateur.Nombre == 0)
                        {
                            generateur.Affichage.Control.Visibility = Visibility.Collapsed;
                            generateur.Affichage.LabelNbPieces.Visibility = Visibility.Collapsed;

                            generateursVides.Add(generateur);
                            generateurs.Remove(generateur);
                        }
                    };
                });

                canvasInterface.Children.Add(generateurs[i].Affichage.Control);

                canvasInterface.Children.Add(generateurs[i].Affichage.LabelNbPieces);
            }

            timerAnimationCercle.Interval = TimeSpan.FromMilliseconds(16 + ((double)2 / 3));
            timerAnimationCercle.Tick += ActualiserAnimationInterface;

            CacherInterface();
            grille.Children.Add(canvasInterface);
        }

        public int NombresTypesPiecesRestants() => generateurs.Count;

        private void ActualiserAnimationInterface(object sender, EventArgs e)
        {
            for (int i = 0; i < generateurs.Count; i++)
            {
                double indexCyclique = ((double)i / (((double)framePresenteAnimation / NB_FRAMES_ANIMATION_CERCLE) * generateurs.Count)) * Math.PI * 2;

                if (Double.IsNaN(indexCyclique))
                    indexCyclique = 0;

                double x = (grilleParente.Width / 2 - TAILLE_PIECE / 2) + Math.Cos(indexCyclique) * RAYON_CERCLE;
                double y = (grilleParente.Height / 2 - TAILLE_PIECE / 2) + Math.Sin(indexCyclique) * RAYON_CERCLE;

                Canvas.SetLeft(generateurs[i].Affichage.Control, x);
                Canvas.SetTop(generateurs[i].Affichage.Control, y);

                Canvas.SetLeft(generateurs[i].Affichage.LabelNbPieces, x - TAILLE_PIECE / 2);
                Canvas.SetTop(generateurs[i].Affichage.LabelNbPieces, y);
            }
            if (framePresenteAnimation >= NB_FRAMES_ANIMATION_CERCLE)
                timerAnimationCercle.Stop();
            else
                ++framePresenteAnimation;
        }

        private void AfficherInterface()
        {
            foreach (GenerateurPieceAffichable generateur in generateurs)
                generateur.Affichage.ActualiserLabel();
            framePresenteAnimation = 0;
            timerAnimationCercle.Start();
            canvasInterface.Visibility = Visibility.Visible;
        }

        public void CacherInterface() => canvasInterface.Visibility = Visibility.Collapsed;

        public void DemanderPiece(Action<Piece, Rectangle, string> methodeRetour)
        {
            MethodeRetourDemandePiece = methodeRetour;
            AfficherInterface();
        }

        public ReponseGenerateurPiece GenererPieceAleatoire(Couleur couleur)
        {
            Random aleatoire = new Random(Guid.NewGuid().GetHashCode());
            int index = aleatoire.Next(generateurs.Count);

            ReponseGenerateurPiece reponse = new ReponseGenerateurPiece(generateurs[index].Generateur.GenererPiece(couleur), new Rectangle()
            {
                Fill = new ImageBrush(new BitmapImage(new Uri(generateurs[index].Affichage.Uri, UriKind.Relative))),
                Cursor = Cursors.Hand
            }, generateurs[index].NomSprite);

            if (generateurs[index].Generateur.Nombre == 0)
            {
                generateurs[index].Affichage.Control.Visibility = Visibility.Collapsed;
                generateurs[index].Affichage.LabelNbPieces.Visibility = Visibility.Collapsed;

                generateursVides.Add(generateurs[index]);
                generateurs.Remove(generateurs[index]);
            }

            return reponse;
        }

        public void RedonnerPiece(string nom)
        {
                // Si toutes les instances de la pièce n'ont pas été placées.
            foreach (GenerateurPieceAffichable generateur in generateurs)
            {
                if (generateur.NomSprite == nom)
                    generateur.Generateur.IncrementerNombre();
            }
                // Si toutes les instances de la pièce ont été placées.
            foreach (GenerateurPieceAffichable generateur in generateursVides)
            {
                if (generateur.NomSprite == nom)
                {
                    generateur.Affichage.Control.Visibility = Visibility.Visible;
                    generateur.Affichage.LabelNbPieces.Visibility = Visibility.Visible;

                    generateur.Generateur.IncrementerNombre();
                    generateurs.Add(generateur);
                    generateursVides.Remove(generateur);

                    return;
                }
            }
        }
    }
}
