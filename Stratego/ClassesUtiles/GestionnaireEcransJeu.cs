using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Stratego
{
    public class GestionnaireEcransJeu
    {
        private static GestionnaireEcransJeu instance;

        public static void Creer(Dictionary<string, UserControl> ecransJeu, UIElementCollection collectionAffichage, string premierEcran)
            => instance = new GestionnaireEcransJeu(ecransJeu, collectionAffichage, premierEcran);

        public static void ChangerEcran(string ecran, Dictionary<string, ParametresConstruction> parametres = null)
        {
            if (instance != null && instance.EstIdValide(ecran))
            {
                if(instance.EstIdValide(instance.EcranSelectionne))
                {
                    instance.CollectionAffichage.Remove(instance.EcransDeJeu[instance.EcranSelectionne]);
                    if (instance.EcransDeJeu[instance.EcranSelectionne] is IDestructible)
                        ((IDestructible)instance.EcransDeJeu[instance.EcranSelectionne]).Detruire();
                }
                instance.EcranSelectionne = ecran;

                if (instance.EcransDeJeu[instance.EcranSelectionne] is IConstructibleParametre && parametres != null)
                    ((IConstructibleParametre)instance.EcransDeJeu[instance.EcranSelectionne]).Construire(parametres);
                else if (instance.EcransDeJeu[instance.EcranSelectionne] is IConstructible)
                    ((IConstructible)instance.EcransDeJeu[instance.EcranSelectionne]).Construire();
                instance.CollectionAffichage.Add(instance.EcransDeJeu[instance.EcranSelectionne]);
            }
        }

        public static void AjouterEcran(string nom, UserControl ecran) => instance.EcransDeJeu.Add(nom, ecran);

        private Dictionary<string, UserControl> EcransDeJeu { get; set; }
        private string EcranSelectionne { get; set; }
        UIElementCollection CollectionAffichage { get; set; }

        private GestionnaireEcransJeu(Dictionary<string, UserControl> ecransJeu, UIElementCollection collectionAffichage, string premierEcran)
        {
            EcransDeJeu = ecransJeu;
            EcranSelectionne = premierEcran;
            CollectionAffichage = collectionAffichage;

            if (EstIdValide(EcranSelectionne))
            {
                if (EcransDeJeu[EcranSelectionne] is IConstructible)
                    ((IConstructible)EcransDeJeu[EcranSelectionne]).Construire();
                CollectionAffichage.Add(EcransDeJeu[EcranSelectionne]);
            }
        }

        public bool EstIdValide(string nomEcran) => EcransDeJeu.ContainsKey(nomEcran);
    }
}
