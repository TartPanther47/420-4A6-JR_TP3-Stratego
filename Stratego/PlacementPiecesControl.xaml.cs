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
        private bool estSelection;
        private bool EstSelection
        {
            get { return estSelection; }
            set
            {
                preSelection.Visibility = (value ? Visibility.Collapsed : Visibility.Visible);
                estSelection = value;
            }
        }

        private Rectangle preSelection;
        private SelectionneurPieces selectionneurPieces;
        
        public PlacementPiecesControl()
        {
            InitializeComponent();

            DiviserGrille();
            InsererCasesGrille();

            preSelection = new Rectangle();
            grdPlateauJeu.Children.Add(preSelection);
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
                if(i < GrilleJeu.TAILLE_GRILLE_JEU / 2)
                {
                    grdPlateauJeu.RowDefinitions.Add(new RowDefinition());
                }
            }
        }

        private void InsererCasesGrille()
        {
            for (int x = 0; x < GrilleJeu.TAILLE_GRILLE_JEU; x++)
            {
                for (int y = 0; y < GrilleJeu.TAILLE_GRILLE_JEU / 2; y++)
                {
                    Rectangle caseGrille = new Rectangle();
                    caseGrille.Fill = new ImageBrush(new BitmapImage(new Uri("textures/terrain.png", UriKind.Relative)));
                    
                    caseGrille.MouseEnter += (object sender, MouseEventArgs e) =>
                    {
                        preSelection.Fill = new ImageBrush(new BitmapImage(new Uri("textures/preSelector.png", UriKind.Relative)));
                        preSelection.Cursor = Cursors.Hand;

                        preSelection.MouseUp += (object casePreselectionnee, MouseButtonEventArgs arguments) =>
                        {
                            EstSelection = true;
                            selectionneurPieces.DemanderPiece(Piece =>
                            {
                                EstSelection = false;

                            });
                        };

                        Grid.SetColumn(preSelection, Grid.GetColumn((Rectangle)sender));
                        Grid.SetRow(preSelection, Grid.GetRow((Rectangle)sender));
                        if(!EstSelection)
                            preSelection.Visibility = Visibility.Visible;
                    };

                    Grid.SetColumn(caseGrille, x);
                    Grid.SetRow(caseGrille, y);

                    grdPlateauJeu.Children.Add(caseGrille);
                }
            }

            grdPlateauJeu.MouseLeave += (object sender, MouseEventArgs e) =>
            {
                preSelection.Visibility = Visibility.Collapsed;
            };
        }
    }
}
