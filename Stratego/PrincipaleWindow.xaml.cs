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
    /// Logique d'interaction pour PrincipaleWindow.xaml
    /// </summary>
    public partial class PrincipaleWindow : Window
    {
        /// <summary>
        /// Fenêtre principale, contient une grille où on affiche les contrôle
        /// 
        ///     Bonus : le système de sélection des pièces.
        ///         Je ne sais pas si cela se qualifie comme fonctionnalité bonus, car ce n'est pas tant une fonctionnalité pour utilisateur
        ///         (il fallait de toute façon qu'il puisse choisir les pièces). Cependant, je considère que c'est une fonctionnalité
        ///         supplémentaire quand même; c'est plutôt une fonctionnalité au niveau des mechanismes qui sont employés:
        ///             • La classe « SelectionneurPieces » est un contrôleur auquel on peut demander de nous donner une pièce ou d'en
        ///               reprendre une.
        ///             • Pour chaque type de pièce, il y a un générateur de pièces qui gère lui-même le nombre de pièces
        ///               qu'on peut encore placer.
        ///             • La structure de générateurs est une factory, ce qui permet de traiter toutes les pièces de la même façon,
        ///               en évitant d'avoir à évaluer des conditions pour déterminer laquelle créer (ex. une switch).
        ///               C'est aussi très flexible et réutilisable; la factory est aussi utilisée dans l'AI, par exemple.
        ///             • J'ai créé des wrappers qui lient la factory à des affichages et qui gère l'affichage du nombre
        ///               de pièces restantes selon la valeur que leur indiquent les générateur.
        ///             • J'ai aussi créé un système d'animation qui place l'affichage des générateurs de pièces en cercle (trigonométrie)
        ///             • Et j'ai implémenté un système de callback qui rappelle une fonction, donnée lors de la requête de pièce,
        ///               au moment où on clique sur un générateur de pièces. Dans ce callback, on passe la pièce fraîchement créée.
        ///             • Finalement, on peut aussi demander un pièce aléatoire, parmis ceux qui sont disponibles. Cette fonctionalité
        ///               est utilisée lorsque l'on clique sur le bouton "aléatoire" : le programme détermine alors les cases où il n'y
        ///               a pas de pièce et y met une pièce aléatoire. Ainsi, on peut placer les pièces importantes (ex. le drapeau
        ///               entouré de bombes) et laisser le programme placer les autres aléatoirement dans les cases à combler.
        ///               C'est pratique pour tester des stratégies sans avoir à placer toutes les pièces manuellement.
        /// </summary>
        public PrincipaleWindow()
         {
            InitializeComponent();

            GestionnaireEcransJeu.Creer(new Dictionary<string, UserControl>
            {
                { "Accueil", new AccueilControl() },
                { "Choix couleur", new ChoixCouleurControl() },
                { "Placement des pieces", new PlacementPiecesControl() },
                { "Partie", new JeuStrategoControl() }
            }, grdPrincipale.Children, "Accueil");
         }

        /// <summary>
        /// Quand on ferme la fenêtre, on arrête tous les threads (sinon, quand on ferme la fenêtre lorsque l'IA joue, on aurait une exception)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            => Environment.Exit(Environment.ExitCode);
    }
}