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
    /// Logique d'interaction pour AccueilControl.xaml
    /// </summary>
    public partial class AccueilControl : UserControl
    {
        /// <summary>
        /// Construire un contrôle d'accueil
        /// </summary>
        public AccueilControl() => InitializeComponent();

        /// <summary>
        /// S'exécute lorsque l'on clique sur le bouton « lancer partie ».
        /// </summary>
        /// <param name="sender">Objet appelant</param>
        /// <param name="e">Arguments</param>
        private void btnLancerPartie_Click(object sender, RoutedEventArgs e)
            => GestionnaireEcransJeu.ChangerEcran("Choix couleur");
    }
}
