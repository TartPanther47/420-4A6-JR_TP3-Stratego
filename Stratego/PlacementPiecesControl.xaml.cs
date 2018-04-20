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
        public PlacementPiecesControl()
        {
            InitializeComponent();
        }

        public void Construire()
        {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GestionnaireEcransJeu.ChangerEcran("Partie");
        }
    }
}
