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
    public partial class PlacementPiecesControl : UserControl, IConstructible
    {
        private const int HAUTEUR_GRILLE_DISPONIBLE = 4;
        
        private bool EstSelection { get; set; }

        private Rectangle selection;
        private SelectionneurPieces selectionneurPieces;

        private PieceAffichable[,] pieces;
        
        public PlacementPiecesControl()
        {
            InitializeComponent();

            DiviserGrille();
            InsererCasesGrille();

            pieces = new PieceAffichable[GrilleJeu.TAILLE_GRILLE_JEU, HAUTEUR_GRILLE_DISPONIBLE];
            selection = new Rectangle();
            grdPlateauJeu.Children.Add(selection);
            EstSelection = false;
        }

        public void Construire()
        {
            selectionneurPieces = new SelectionneurPieces(grdPlateauJeu);
        }

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
                                    Affichage.MouseLeftButtonUp += (object pieceAffichable, MouseButtonEventArgs evenement) =>
                                    {
                                        selectionneurPieces.RedonnerPiece(Nom);
                                        grdPlateauJeu.Children.Remove(Affichage);
                                    };
                                });

                                Grid.SetColumn(pieces[X, Y].Affichage, X);
                                Grid.SetRow(pieces[X, Y].Affichage, Y);

                                grdPlateauJeu.Children.Add(pieces[X, Y].Affichage);
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

            grdPlateauJeu.MouseLeave += (object sender, MouseEventArgs e) =>
            {
                if(!EstSelection)
                    selection.Visibility = Visibility.Collapsed;
            };
        }

        private void btnAleatoire_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < GrilleJeu.TAILLE_GRILLE_JEU; x++)
            {
                for (int y = 0; y < HAUTEUR_GRILLE_DISPONIBLE; y++)
                {
                    if (pieces[x, y] == null)
                    {
                        ReponseGenerateurPiece piece = selectionneurPieces.GenererPieceAleatoire(ParametresJeu.CouleurJoueur);

                        EstSelection = false;

                        pieces[x, y] = new PieceAffichable(piece.Piece, piece.Affichage, piece.Nom);
                        pieces[x, y].Modifier((Piece Piece, Rectangle Affichage, string Nom) =>
                        {
                            Affichage.MouseLeftButtonUp += (object pieceAffichable, MouseButtonEventArgs evenement) =>
                            {
                                selectionneurPieces.RedonnerPiece(Nom);
                                grdPlateauJeu.Children.Remove(Affichage);
                            };
                        });

                        Grid.SetColumn(pieces[x, y].Affichage, x);
                        Grid.SetRow(pieces[x, y].Affichage, y);

                        grdPlateauJeu.Children.Add(pieces[x, y].Affichage);
                    }
                }
            }
        }

        private void btnJouer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
