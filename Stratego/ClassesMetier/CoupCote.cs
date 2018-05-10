using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class CoupCote : IComparable
    {
        private static readonly Dictionary<Type, int> LIST_IMPORTANCE_PIECES = new Dictionary<Type, int>
        {
            { typeof(Marechal), 10 },
            { typeof(General), 9 },
            { typeof(Colonel), 8 },
            { typeof(Commandant), 7 },
            { typeof(Capitaine), 6 },
            { typeof(Lieutenant), 5 },
            { typeof(Sergent), 4 },
            { typeof(Demineur), 3 },
            { typeof(Eclaireur), 2 },
            { typeof(Espion), 1 },
            { typeof(Drapeau), 11 },
            { typeof(Bombe), 1 }
        };

        private GrilleJeu Jeu { get; set; }
        private Couleur CouleurIA { get; set; }

        private Piece PieceAttaquant { get; set; }
        private Piece PieceCible { get; set; }

        public Coordonnee Attaquant { get; private set; }
        public Coordonnee Cible { get; private set; }
        public int Cote { get; private set; }

        public CoupCote(Coordonnee attaquant, Coordonnee cible, GrilleJeu jeu, Couleur couleurIA)
        {
            Jeu = jeu;
            CouleurIA = couleurIA;

            Attaquant = attaquant;
            Cible = cible;

            PieceAttaquant = jeu.ObtenirPiece(attaquant);
            PieceCible = jeu.ObtenirPiece(cible);

            CalculerCote();
        }

        private bool DeterminerSiGagne(Piece pieceAttaquante, Piece pieceCible)
        {
            if (pieceCible is PieceMobile)
            {
                if (pieceCible is Marechal && pieceAttaquante is Espion ||
                    pieceCible is Espion && pieceAttaquante is Marechal) return true;
                else if (((PieceMobile)pieceAttaquante).Force < ((PieceMobile)pieceCible).Force) return false;
                else if (((PieceMobile)pieceAttaquante).Force > ((PieceMobile)pieceCible).Force) return true;
                else return false;
            }
            else if (pieceCible is Bombe)
            {
                if (pieceAttaquante is Demineur) return true;
                else return false;
            }
            else if (pieceCible is Drapeau) return true;
            return true;
        }

        public void CalculerCote()
        {
            if(PieceCible != null)
            {
                if(PieceCible.EstVisible)
                {
                    if (PieceAttaquant.GetType() == PieceCible.GetType()) Cote = 0;
                    else
                    {
                        if (DeterminerSiGagne(PieceAttaquant, PieceCible))
                            Cote = LIST_IMPORTANCE_PIECES[PieceCible.GetType()];
                        else Cote = -LIST_IMPORTANCE_PIECES[PieceAttaquant.GetType()];
                    }
                }
                else
                {
                    int nombrePieces = Jeu.CalculerNombrePieces(CouleurIA);
                    List<Tuple<Type, double>> probabilites = new List<Tuple<Type, double>>
                    {
                        new Tuple<Type, double> (
                            typeof(Marechal),
                            (2 * Jeu.CalculerNombrePieces(typeof(Marechal), CouleurIA) - 1) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(General),
                            (2 * Jeu.CalculerNombrePieces(typeof(General), CouleurIA) - 1) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Colonel),
                            (2 * Jeu.CalculerNombrePieces(typeof(Colonel), CouleurIA) - 2) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Commandant),
                            (2 * Jeu.CalculerNombrePieces(typeof(Commandant), CouleurIA) - 3) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Capitaine),
                            (2 * Jeu.CalculerNombrePieces(typeof(Capitaine), CouleurIA) - 4) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Lieutenant),
                            (2 * Jeu.CalculerNombrePieces(typeof(Lieutenant), CouleurIA) - 4) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Sergent),
                            (2 * Jeu.CalculerNombrePieces(typeof(Sergent), CouleurIA) - 4) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Demineur),
                            (2 * Jeu.CalculerNombrePieces(typeof(Demineur), CouleurIA) - 5) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Eclaireur),
                            (2 * Jeu.CalculerNombrePieces(typeof(Eclaireur), CouleurIA) - 8) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Espion),
                            (2 * Jeu.CalculerNombrePieces(typeof(Espion), CouleurIA) - 1) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Drapeau),
                            (2 * Jeu.CalculerNombrePieces(typeof(Drapeau), CouleurIA) - 1) / (double)nombrePieces
                        ),
                        new Tuple<Type, double> (
                            typeof(Bombe),
                            (2 * Jeu.CalculerNombrePieces(typeof(Bombe), CouleurIA) - 6) / (double)nombrePieces
                        )
                    };

                    probabilites.Sort((a, b) => { return (int)((b.Item2 - a.Item2) * 100); });

                    List<Tuple<Type, double>> meilleursProbabilites = new List<Tuple<Type, double>>();

                    bool bEstPareil = true;
                    double dScore = probabilites[0].Item2;
                    for(int i = 0; i < probabilites.Count && bEstPareil; i++)
                    {
                        if (probabilites[i].Item2 != dScore) bEstPareil = false;
                        else meilleursProbabilites.Add(probabilites[i]);
                    }

                    Random rnd = new Random(DateTime.Now.Millisecond);

                    Type typePiece = meilleursProbabilites[rnd.Next(meilleursProbabilites.Count)].Item1;

                    Piece pieceCibleTemporaire = (Piece)Activator.CreateInstance(typePiece, new object[] { CouleurIA });

                    if (PieceAttaquant.GetType() == pieceCibleTemporaire.GetType()) Cote = 0;
                    else
                    {
                        if (DeterminerSiGagne(PieceAttaquant, pieceCibleTemporaire))
                            Cote = LIST_IMPORTANCE_PIECES[pieceCibleTemporaire.GetType()];
                        else Cote = -LIST_IMPORTANCE_PIECES[PieceAttaquant.GetType()];
                    }
                }
            }
        }

        public int CompareTo(object obj) => ((CoupCote)obj).Cote - Cote;
    }
}
