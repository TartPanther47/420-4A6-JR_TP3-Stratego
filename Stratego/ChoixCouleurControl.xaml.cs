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
    /// Logique d'interaction pour ChoixCouleurControl.xaml
    /// </summary>
    public partial class ChoixCouleurControl : UserControl
    {
        public ChoixCouleurControl()
        {
            InitializeComponent();
        }

        private void btnRouge_Click(object sender, RoutedEventArgs e)
        {
            ParametresJeu.CouleurJoueur = Couleur.Rouge;
            GestionnaireEcransJeu.ChangerEcran("Placement des pieces");
        }

        private void btnBleu_Click(object sender, RoutedEventArgs e)
        {
            ParametresJeu.CouleurJoueur = Couleur.Bleu;
            GestionnaireEcransJeu.ChangerEcran("Placement des pieces");
        }
    }
}
