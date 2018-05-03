using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stratego
{
   public class IA_Stratego : IObserver<JeuStrategoControl>
   {
      #region Code relié au patron observateur

      private IDisposable unsubscriber;

      public void Subscribe(IObservable<JeuStrategoControl> provider)
      {
         unsubscriber = provider.Subscribe(this);
      }

      public void Unsubscribe()
      {
         unsubscriber.Dispose();
      }

      public void OnCompleted()
      {
         // Ne fait rien pour l'instant.
      }

      public void OnError(Exception error)
      {
         // Ne fait rien pour l'instant.
      }

      public void OnNext(JeuStrategoControl g)
      {
         JouerCoup(g);
      }
      #endregion

      private JeuStrategoControl Jeu { get; set; }

      private Couleur Couleur { get; set; }

      public IA_Stratego(JeuStrategoControl jeu, Couleur couleur)
      {
         Jeu = jeu;
         Couleur = couleur;

         // Abonner l'IA à l'interface du jeu.
         jeu.Subscribe(this);
      }

      private void JouerCoup(JeuStrategoControl jeu)
      {
         List<List<Coordonnee>> ListeCoupsPermis;
         Random rnd = new Random(DateTime.Now.Millisecond);
         int choixRnd;

         ListeCoupsPermis = TrouverCoupsPermis(jeu.GrillePartie);

         choixRnd = rnd.Next(0, ListeCoupsPermis.Count);
         jeu.ExecuterCoup(ListeCoupsPermis[choixRnd][0], ListeCoupsPermis[choixRnd][1]);
      }

      private List<List<Coordonnee>> TrouverCoupsPermis(GrilleJeu grillePartie)
      {
         List<List<Coordonnee>> listeCoups = new List<List<Coordonnee>>();
         Coordonnee pointDepart, pointCible;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               pointDepart = new Coordonnee(i, j);

               if (Jeu.GrillePartie.EstCaseOccupee(pointDepart) 
                  && Jeu.GrillePartie.ObtenirCouleurPiece(pointDepart) == Couleur)
               {
                  // Valider un coup vers la gauche.
                  pointCible = new Coordonnee(pointDepart.X - 1, pointDepart.Y);
                  if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                  {
                     listeCoups.Add(new List<Coordonnee>() { pointDepart, pointCible });
                  }

                  // Valider un coup vers l'avant.
                  pointCible = new Coordonnee(pointDepart.X, pointDepart.Y - 1);
                  if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                  {
                     listeCoups.Add(new List<Coordonnee>() { pointDepart, pointCible });
                  }

                  // Valider un coup vers la droite.
                  pointCible = new Coordonnee(pointDepart.X + 1, pointDepart.Y);
                  if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                  {
                     listeCoups.Add(new List<Coordonnee>() { pointDepart, pointCible });
                  }

                  // Valider un coup vers l'arrière.
                  pointCible = new Coordonnee(pointDepart.X, pointDepart.Y + 1);
                  if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                  {
                     listeCoups.Add(new List<Coordonnee>() { pointDepart, pointCible });
                  }
               }
            }
         }

         return listeCoups;
      }

      public List<Piece> PlacerPieces()
      {
        return new List<Piece>() { new Commandant(Couleur), new Lieutenant(Couleur), new Demineur(Couleur), new Demineur(Couleur), new Demineur(Couleur), new Capitaine(Couleur), new Bombe(Couleur), new Sergent(Couleur), new Bombe(Couleur), new Drapeau(Couleur),
                                   new Capitaine(Couleur), new Eclaireur(Couleur), new Capitaine(Couleur), new Sergent(Couleur), new Lieutenant(Couleur), new Eclaireur(Couleur), new Eclaireur(Couleur), new Bombe(Couleur), new Sergent(Couleur), new Bombe(Couleur),
                                   new Eclaireur(Couleur), new Commandant(Couleur), new Eclaireur(Couleur), new Eclaireur(Couleur), new Marechal(Couleur), new Commandant(Couleur), new Capitaine(Couleur), new Demineur(Couleur), new Bombe(Couleur), new Sergent(Couleur),
                                   new Lieutenant(Couleur), new Eclaireur(Couleur), new Colonel(Couleur), new Demineur(Couleur), new Lieutenant(Couleur), new Eclaireur(Couleur), new Colonel(Couleur), new Espion(Couleur), new General(Couleur), new Bombe(Couleur)
        };
      }
   }
}
