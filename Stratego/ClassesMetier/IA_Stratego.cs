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
      private const int LARGEUR_ZONE_DEPART = GrilleJeu.TAILLE_GRILLE_JEU;
      private const int HAUTEUR_ZONE_DEPART = 4;

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
         List<CoupCote> ListeCoupsPermis;
         Random rnd = new Random(DateTime.Now.Millisecond);
         int choixRnd;

         ListeCoupsPermis = TrouverCoupsPermis(jeu.GrillePartie);
         ListeCoupsPermis.Sort();

         List<CoupCote> ListeCoupsEgaux = new List<CoupCote>();

         bool bEstPareille = true;
         int iScore = ListeCoupsPermis[0].Cote;
         for(int i = 0; i < ListeCoupsPermis.Count && bEstPareille; ++i)
         {
            if(ListeCoupsPermis[i].Cote != iScore) bEstPareille = false;
            else ListeCoupsEgaux.Add(ListeCoupsPermis[i]);
         }

         choixRnd = rnd.Next(0, ListeCoupsEgaux.Count);
         jeu.ExecuterCoup(ListeCoupsEgaux[choixRnd].Attaquant, ListeCoupsEgaux[choixRnd].Cible);
      }

      private List<CoupCote> TrouverCoupsPermis(GrilleJeu grillePartie)
      {
         List<CoupCote> listeCoups = new List<CoupCote>();
         Coordonnee pointDepart, pointCible;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               pointDepart = new Coordonnee(i, j);

               if (Jeu.GrillePartie.EstCaseOccupee(pointDepart) 
                  && Jeu.GrillePartie.ObtenirCouleurPiece(pointDepart) == Couleur)
               {
                  if(Jeu.GrillePartie.ObtenirPiece(pointDepart) is Eclaireur)
                  {
                      // Valider les coups vers la gauche.
                      for (int k = pointDepart.X; k >= 0; k--)
                      {
                          pointCible = new Coordonnee(k, pointDepart.Y);
                          if(Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                            listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                      }
                      // Valider les coups vers le haut.
                      for (int k = pointDepart.Y; k >= 0; k--)
                      {
                          pointCible = new Coordonnee(pointDepart.X, k);
                          if(Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                            listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                      }
                      // Valider les coups vers la droite.
                      for (int k = pointDepart.X; k < GrilleJeu.TAILLE_GRILLE_JEU; k++)
                      {
                          pointCible = new Coordonnee(k, pointDepart.Y);
                          if(Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                            listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                      }
                      // Valider les coups vers le bas.
                      for (int k = pointDepart.Y; k < HAUTEUR_ZONE_DEPART; k++)
                      {
                          pointCible = new Coordonnee(k, pointDepart.Y);
                          if(Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                            listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                      }
                  }
                  else
                  {
                      // Valider un coup vers la gauche.
                      pointCible = new Coordonnee(pointDepart.X - 1, pointDepart.Y);
                      if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                         listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));

                      // Valider un coup vers l'avant.
                      pointCible = new Coordonnee(pointDepart.X, pointDepart.Y - 1);
                      if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                         listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));

                      // Valider un coup vers la droite.
                      pointCible = new Coordonnee(pointDepart.X + 1, pointDepart.Y);
                      if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                        listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));

                      // Valider un coup vers l'arrière.
                      pointCible = new Coordonnee(pointDepart.X, pointDepart.Y + 1);
                      if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                        listeCoups.Add(new CoupCote(pointDepart, pointCible, grillePartie, Couleur));
                  }
               }
            }
         }

         return listeCoups;
      }

      public List<Piece> PlacerPieces()
      {
            Piece[,] pieces = new Piece[LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART];

            List<GenerateurPiece> generateursPieces = new List<GenerateurPiece>
            {
                new GenerateurDrapeau(),
                new GenerateurMarechal(),
                new GenerateurBombe(),
                new GenerateurGeneral(),
                new GenerateurColonel(),
                new GenerateurCommandant(),
                new GenerateurCapitaine(),
                new GenerateurLieutenant(),
                new GenerateurSergent(),
                new GenerateurDemineur(),
                new GenerateurEclaireur(),
                new GenerateurEspion()
            };
            
            List<StrategiePlacementPiece> strategies = new List<StrategiePlacementPiece>
            {
                new StrategiePlacementPieceDrapeau(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceMarechal(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceBombe(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART),
                new StrategiePlacementPieceAleatoire(pieces, LARGEUR_ZONE_DEPART, HAUTEUR_ZONE_DEPART)
            };
            
            for (int i = 0; i < generateursPieces.Count; i++)
            {
                while(generateursPieces[i].EstGenerable())
                {
                    Coordonnee position = strategies[i].GetPosition();
                    pieces[position.X, position.Y] = generateursPieces[i].GenererPiece(Couleur);
                }
            }

            List<Piece> lstPieces = new List<Piece>();
            for (int y = 0; y < HAUTEUR_ZONE_DEPART; y++)
                for (int x = 0; x < LARGEUR_ZONE_DEPART; x++)
                    lstPieces.Add(pieces[x, y]);

            return lstPieces;
      }
   }
}
