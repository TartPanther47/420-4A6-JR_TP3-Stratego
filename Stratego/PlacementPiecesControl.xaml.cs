// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stratego
{
    /// <summary>
    /// Logique d'interaction pour PlacerPiecesControl.xaml
    /// </summary>
    public partial class PlacementPiecesControl : UserControl, IConstructibleParametre, IDestructible
    {
        #region Statiques / constantes
        private const int HAUTEUR_GRILLE_DISPONIBLE = 4;
        #endregion

        #region Attributs
        private bool EstSelection { get; set; }

        private Rectangle selection;
        private SelectionneurPieces selectionneurPieces;

        private PieceAffichable[,] pieces;

        private ParametresCouleurJoueurs ParametresCouleursJoueurs { get; set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Construit le contrôle
        /// </summary>
        public PlacementPiecesControl()
        {
            InitializeComponent();

            DiviserGrille();
            InsererCasesGrille();

            selection = new Rectangle();
            grdPlateauJeu.Children.Add(selection);
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Initialise le contrôle quand on change d'écran de jeu
        /// </summary>
        /// <param name="parametres"></param>
        public void Construire(Dictionary<string, ParametresConstruction> parametres)
        {
            ParametresCouleursJoueurs = (ParametresCouleurJoueurs) parametres["Couleur joueurs"];
            selectionneurPieces = new SelectionneurPieces(grdPlateauJeu, ParametresCouleursJoueurs.CouleurJoueur);
            pieces = new PieceAffichable[GrilleJeu.TAILLE_GRILLE_JEU, HAUTEUR_GRILLE_DISPONIBLE];
            EstSelection = false;
        }

        /// <summary>
        /// Divise la grille de placement en colonnes et en rangées
        /// </summary>
        private void DiviserGrille()
        {
            for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
            {
                grdPlateauJeu.ColumnDefinitions.Add(new ColumnDefinition());
                if(i < HAUTEUR_GRILLE_DISPONIBLE)
                {
                    grdPlateauJeu.RowDefinitions.Add(new RowDefinition());
                }
            }
        }

        /// <summary>
        /// Crée et insère les cases avec les textures
        /// </summary>
        private void InsererCasesGrille()
        {
            for (int x = 0; x < GrilleJeu.TAILLE_GRILLE_JEU; x++)
            {
                for (int y = 0; y < HAUTEUR_GRILLE_DISPONIBLE; y++)
                {
                    Rectangle caseGrille = new Rectangle();
                    caseGrille.Fill = new ImageBrush(new BitmapImage(new Uri("textures/terrain.png", UriKind.Relative)));
                    
                    caseGrille.MouseEnter += (object sender, MouseEventArgs e) =>
                    {
                        selection.Fill = new ImageBrush(new BitmapImage(new Uri("textures/preSelector.png", UriKind.Relative)));
                        selection.Cursor = Cursors.Hand;

                        selection.MouseUp += (object casePreselectionnee, MouseButtonEventArgs arguments) =>
                        {
                            EstSelection = true;
                            selectionneurPieces.DemanderPiece((piece, affichage, nom) =>
                            {
                                EstSelection = false;
                                int X = Grid.GetColumn((Rectangle)sender);
                                int Y = Grid.GetRow((Rectangle)sender);

                                pieces[X, Y] = new PieceAffichable(piece, affichage, nom);
                                pieces[X, Y].Modifier((Piece Piece, Rectangle Affichage, string Nom) =>
                                {
                                     // Évènement du clic du sélectionneur
                                    Affichage.MouseLeftButtonUp += (object pieceAffichable, MouseButtonEventArgs evenement) =>
                                    {
                                        selectionneurPieces.RedonnerPiece(Nom);
                                        grdPlateauJeu.Children.Remove(Affichage);

                                        pieces[Grid.GetColumn((Rectangle)pieceAffichable),
                                               Grid.GetRow((Rectangle)pieceAffichable)] = null;
                                        btnJouer.IsEnabled = false;
                                        if (selectionneurPieces.NombresTypesPiecesRestants() == SelectionneurPieces.NB_TYPES_PIECES)
                                            btnVider.IsEnabled = false;
                                        btnAleatoire.IsEnabled = true;
                                    };
                                });

                                Grid.SetColumn(pieces[X, Y].Affichage, X);
                                Grid.SetRow(pieces[X, Y].Affichage, Y);

                                grdPlateauJeu.Children.Add(pieces[X, Y].Affichage);

                                if (selectionneurPieces.NombresTypesPiecesRestants() == 1)
                                {
                                    btnJouer.IsEnabled = true;
                                    btnAleatoire.IsEnabled = false;
                                }
                                btnVider.IsEnabled = true;
                            });
                        };

                        Grid.SetColumn(selection, Grid.GetColumn((Rectangle)sender));
                        Grid.SetRow(selection, Grid.GetRow((Rectangle)sender));
                        if(!EstSelection)
                            selection.Visibility = Visibility.Visible;
                    };

                    Grid.SetColumn(caseGrille, x);
                    Grid.SetRow(caseGrille, y);

                    grdPlateauJeu.Children.Add(caseGrille);
                }
            }

                // Lorsque l'on quitte la zone des pièces, on cache le sélecteur
            grdPlateauJeu.MouseLeave += (object sender, MouseEventArgs e) =>
            {
                if(!EstSelection)
                    selection.Visibility = Visibility.Collapsed;
            };
        }

        /// <summary>
        /// Vide les cases (enlève les pièces de l'interface et les redonne au sélecteur)
        /// </summary>
        private void ViderCases()
        {
            for (int x = 0; x < GrilleJeu.TAILLE_GRILLE_JEU; x++)
            {
                for (int y = 0; y < HAUTEUR_GRILLE_DISPONIBLE; y++)
                {
                    if (pieces[x, y] != null)
                    {
                        selectionneurPieces.RedonnerPiece(pieces[x, y].Nom);
                        grdPlateauJeu.Children.Remove(pieces[x, y].Affichage);
                        pieces[x, y] = null;
                    }
                }
            }

            btnVider.IsEnabled = false;
            btnJouer.IsEnabled = false;
            btnAleatoire.IsEnabled = true;
        }

        /// <summary>
        /// S'exécute lorsque l'on clique sur le bouton « Aléatoire ».
        /// </summary>
        /// <param name="sender">Objet appelant</param>
        /// <param name="e">Arguments</param>
        private void btnAleatoire_Click(object sender, RoutedEventArgs e)
        {
            btnAleatoire.IsEnabled = false;
            for (int x = 0; x < GrilleJeu.TAILLE_GRILLE_JEU; x++)
            {
                for (int y = 0; y < HAUTEUR_GRILLE_DISPONIBLE; y++)
                {
                    if (pieces[x, y] == null) // On ajoute une pièce aléatoire à chaque case vide
                    {
                        ReponseGenerateurPiece piece = selectionneurPieces.GenererPieceAleatoire(ParametresCouleursJoueurs.CouleurJoueur);

                        EstSelection = false;
                        selectionneurPieces.CacherInterface();

                        pieces[x, y] = new PieceAffichable(piece.Piece, piece.Affichage, piece.Nom);
                        pieces[x, y].Modifier((Piece Piece, Rectangle Affichage, string Nom) =>
                        {
                            Affichage.MouseLeftButtonUp += (object pieceAffichable, MouseButtonEventArgs evenement) =>
                            {
                                selectionneurPieces.RedonnerPiece(Nom);
                                grdPlateauJeu.Children.Remove(Affichage);

                                pieces[Grid.GetColumn((Rectangle)pieceAffichable),
                                       Grid.GetRow((Rectangle)pieceAffichable)] = null;
                                btnJouer.IsEnabled = false;
                                if (selectionneurPieces.NombresTypesPiecesRestants() == SelectionneurPieces.NB_TYPES_PIECES)
                                    btnVider.IsEnabled = false;
                                btnAleatoire.IsEnabled = true;
                            };
                        });

                        Grid.SetColumn(pieces[x, y].Affichage, x);
                        Grid.SetRow(pieces[x, y].Affichage, y);

                        grdPlateauJeu.Children.Add(pieces[x, y].Affichage);
                    }
                }
            }

            btnJouer.IsEnabled = true;
            btnVider.IsEnabled = true;
        }

        /// <summary>
        /// S'exécute lorsque l'on clique sur le bouton « Jouer ».
        /// </summary>
        /// <param name="sender">Objet appelant</param>
        /// <param name="e">Arguments</param>
        private void btnJouer_Click(object sender, RoutedEventArgs e)
        {
            GestionnaireEcransJeu.ChangerEcran(
                "Partie", new Dictionary<string, ParametresConstruction>
                {
                    { "Pieces", new ParametresPiecesJoueur(pieces, GrilleJeu.TAILLE_GRILLE_JEU, HAUTEUR_GRILLE_DISPONIBLE) },
                    { "Couleur joueurs", ParametresCouleursJoueurs }
                }
            );
        }

        /// <summary>
        /// S'exécute lorsque l'on clique sur le bouton « Vider ».
        /// </summary>
        /// <param name="sender">Objet appelant</param>
        /// <param name="e">Arguments</param>
        private void btnVider_Click(object sender, RoutedEventArgs e) => ViderCases();

        /// <summary>
        /// Vider les cases à la destruction
        /// </summary>
        public void Detruire() => ViderCases();
        #endregion
    }
}
