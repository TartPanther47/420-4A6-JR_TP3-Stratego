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
    /// Logique d'interaction pour AccueilControl.xaml
    /// </summary>
    public partial class AccueilControl : UserControl
    {
        public AccueilControl()
        {
            InitializeComponent();
        }

        private void btnLancerPartie_Click(object sender, RoutedEventArgs e)
        {
            GestionnaireEcransJeu.ChangerEcran("Partie");
        }
    }
}
