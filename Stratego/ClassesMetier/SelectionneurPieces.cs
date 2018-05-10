// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

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
    /// <summary>
    /// Contrôleur qui gère l'interface de choix des pièces.
    /// </summary>
    public class SelectionneurPieces
    {
        #region Statiques / constantes
        public const int NB_TYPES_PIECES = 12;
        private const int TAILLE_PIECE = 42;
        private const int RAYON_CERCLE = 128;
        private const int NB_FRAMES_ANIMATION_CERCLE = 8;
        #endregion

        #region Attributs
        private List<GenerateurPieceAffichable> generateurs = new List<GenerateurPieceAffichable>();
        private List<GenerateurPieceAffichable> generateursVides { get; set; }

        Couleur CouleurJoueur { get; set; }
        private Rectangle FondEcranModal { get; set; }

        private Action<Piece, Rectangle, string> MethodeRetourDemandePiece { get; set; }

        private Grid grilleParente;
        private Canvas canvasInterface;

        private int framePresenteAnimation;
        private DispatcherTimer timerAnimationCercle = new DispatcherTimer();
        #endregion

        #region Constructeur
        /// <summary>
        /// Construit un contrôleur de placement de pièces
        /// </summary>
        /// <param name="grille">Grille dans laquelle placer les pièces</param>
        /// <param name="couleurJoueur">La couleur du joueur</param>
        public SelectionneurPieces(Grid grille, Couleur couleurJoueur)
        {
            CouleurJoueur = couleurJoueur;

            generateurs = new List<GenerateurPieceAffichable>
            {
                new GenerateurPieceAffichable(new GenerateurMarechal(), "marechal", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurGeneral(), "general", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurColonel(), "colonel", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurCommandant(), "commandant", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurCapitaine(), "capitaine", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurLieutenant(), "lieutenant", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurSergent(), "sergent", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurDemineur(), "demineur", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurEclaireur(), "eclaireur", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurEspion(), "espion", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurDrapeau(), "drapeau", couleurJoueur),
                new GenerateurPieceAffichable(new GenerateurBombe(), "bombe", couleurJoueur)
            };

            grilleParente = grille;
            MethodeRetourDemandePiece = null;
            framePresenteAnimation = 0;
            generateursVides = new List<GenerateurPieceAffichable>();
            FondEcranModal = new Rectangle
            {
                Fill = Brushes.DarkGray,
                Opacity = 0.75,
                Width = grille.Width,
                Height = grille.Height
            };
            FondEcranModal.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) => { CacherInterface(); };

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
                        // Évènement de clic sur une pièce
                    control.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
                    {
                        CacherInterface();
                            // On appel la méthode de callback
                        MethodeRetourDemandePiece(generateur.Generateur.GenererPiece(CouleurJoueur), new Rectangle
                        {
                            Fill = new ImageBrush(new BitmapImage(new Uri(uriSprite, UriKind.Relative))),
                            Cursor = Cursors.Hand
                        }, generateur.NomSprite);

                            // On cache le générateur s'il est vide
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
        #endregion

        #region Methodes
        /// <summary>
        /// Détermine le nombre de types de pièces qu'il reste à placer
        /// </summary>
        /// <returns>Le nombre de types de pièces qu'il reste à placer</returns>
        public int NombresTypesPiecesRestants() => generateurs.Count;

        /// <summary>
        /// Actualiser l'animation de l'interface (quand on ouvre le sélectionneur de pièces)
        /// </summary>
        /// <param name="sender">L'objet qui appelle la fonction</param>
        /// <param name="e">Objet de transport des arguments</param>
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

                generateurs[i].Affichage.Control.Opacity = framePresenteAnimation / (double)NB_FRAMES_ANIMATION_CERCLE;
            }
            if (framePresenteAnimation >= NB_FRAMES_ANIMATION_CERCLE)
                timerAnimationCercle.Stop();
            else
                ++framePresenteAnimation;
        }

        /// <summary>
        /// Affiche l'interface (lance l'animation)
        /// </summary>
        private void AfficherInterface()
        {
                // Actualiser l'affichage des nombres de pièces à générer
            foreach (GenerateurPieceAffichable generateur in generateurs)
                generateur.Affichage.ActualiserLabel();

                // Lancer l'animation
            framePresenteAnimation = 0;
            timerAnimationCercle.Start();

                // Afficher l'interface
            canvasInterface.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Cache l'interface
        /// </summary>
        public void CacherInterface() => canvasInterface.Visibility = Visibility.Collapsed;

        /// <summary>
        /// Demander une pièce
        /// </summary>
        /// <param name="methodeRetour">Methode de callback qui sera appelée lorsque le joueur aura cliqué sur une pièce</param>
        public void DemanderPiece(Action<Piece, Rectangle, string> methodeRetour)
        {
            MethodeRetourDemandePiece = methodeRetour;
            AfficherInterface();
        }

        /// <summary>
        /// Demander une pièce aléatoire
        /// </summary>
        /// <param name="couleur">Couleur de la pièce</param>
        /// <returns></returns>
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

        /// <summary>
        /// Remets une pièce dans le générateur concerné, ou remet le générateur dans l'interface (s'il n'y avait plus de pièces à générer)
        /// </summary>
        /// <param name="nom"></param>
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
        #endregion
    }
}
