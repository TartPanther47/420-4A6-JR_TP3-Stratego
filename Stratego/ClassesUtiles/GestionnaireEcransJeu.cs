// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Stratego
{
    /// <summary>
    /// Gestionnaire d'écrans de jeu
    /// </summary>
    public class GestionnaireEcransJeu
    {
        #region Statiques
        private static GestionnaireEcransJeu instance;

        /// <summary>
        /// Créer le gestionnaire
        /// </summary>
        /// <param name="ecransJeu">Liste des écrans</param>
        /// <param name="collectionAffichage">Affichage où afficher les écrans</param>
        /// <param name="premierEcran">Nom du premier écran à afficher</param>
        public static void Creer(Dictionary<string, UserControl> ecransJeu, UIElementCollection collectionAffichage, string premierEcran)
            => instance = new GestionnaireEcransJeu(ecransJeu, collectionAffichage, premierEcran);

        /// <summary>
        /// Changer d'écran de jeu
        /// </summary>
        /// <param name="ecran">Nom de l'écran</param>
        /// <param name="parametres">Liste des paramètres à passer à l'écran</param>
        public static void ChangerEcran(string ecran, Dictionary<string, ParametresConstruction> parametres = null)
        {
            if (instance != null && instance.EstNomValide(ecran))
            {
                if(instance.EstNomValide(instance.EcranSelectionne))
                {
                    instance.CollectionAffichage.Remove(instance.EcransDeJeu[instance.EcranSelectionne]);
                        // Détruire si applicable
                    if (instance.EcransDeJeu[instance.EcranSelectionne] is IDestructible)
                        ((IDestructible)instance.EcransDeJeu[instance.EcranSelectionne]).Detruire();
                }
                instance.EcranSelectionne = ecran;

                    // Construire si applicable (avec ou sans paramètres)
                if (instance.EcransDeJeu[instance.EcranSelectionne] is IConstructibleParametre && parametres != null)
                    ((IConstructibleParametre)instance.EcransDeJeu[instance.EcranSelectionne]).Construire(parametres);
                else if (instance.EcransDeJeu[instance.EcranSelectionne] is IConstructible)
                    ((IConstructible)instance.EcransDeJeu[instance.EcranSelectionne]).Construire();
                instance.CollectionAffichage.Add(instance.EcransDeJeu[instance.EcranSelectionne]);
            }
        }

        /// <summary>
        /// Obtenir l'écran présent
        /// </summary>
        /// <returns>Le contrôle présentement affiché</returns>
        public static UserControl GetEcranPresent() => instance.EcransDeJeu[instance.EcranSelectionne];

        /// <summary>
        /// Ajouter un écran
        /// </summary>
        /// <param name="nom">Nom</param>
        /// <param name="ecran">Contrôle</param>
        public static void AjouterEcran(string nom, UserControl ecran) => instance.EcransDeJeu.Add(nom, ecran);
        #endregion

        #region Attributs
        private Dictionary<string, UserControl> EcransDeJeu { get; set; }
        private string EcranSelectionne { get; set; }
        UIElementCollection CollectionAffichage { get; set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Construire le gestionnaire
        /// </summary>
        /// <param name="ecransJeu">Liste des écrans</param>
        /// <param name="collectionAffichage">Affichage où afficher les écrans</param>
        /// <param name="premierEcran">Nom du premier écran à afficher</param>
        private GestionnaireEcransJeu(Dictionary<string, UserControl> ecransJeu, UIElementCollection collectionAffichage, string premierEcran)
        {
            EcransDeJeu = ecransJeu;
            EcranSelectionne = premierEcran;
            CollectionAffichage = collectionAffichage;

            if (EstNomValide(EcranSelectionne))
            {
                if (EcransDeJeu[EcranSelectionne] is IConstructible)
                    ((IConstructible)EcransDeJeu[EcranSelectionne]).Construire();
                CollectionAffichage.Add(EcransDeJeu[EcranSelectionne]);
            }
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Déterminer s'il existe un écran portant un nom spécifié
        /// </summary>
        /// <param name="nomEcran">Le nom</param>
        /// <returns>S'il en existe</returns>
        public bool EstNomValide(string nomEcran) => EcransDeJeu.ContainsKey(nomEcran);
        #endregion
    }
}
