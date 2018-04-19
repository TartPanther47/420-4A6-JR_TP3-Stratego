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

        public static void ChangerEcran(string ecran)
        {
            if (instance != null && instance.EstIdValide(ecran))
            {
                if(instance.EstIdValide(instance.EcranSelectionne))
                    instance.CollectionAffichage.Remove(instance.EcransDeJeu[instance.EcranSelectionne]);
                instance.EcranSelectionne = ecran;
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
                CollectionAffichage.Add(EcransDeJeu[EcranSelectionne]);
        }

        public bool EstIdValide(string nomEcran) => EcransDeJeu.ContainsKey(nomEcran);
    }
}
