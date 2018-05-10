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
    /// Conteneur des pièces capturées
    /// </summary>
    public class ConteneurPiecesCapturees
    {
        #region Attributs
        private List<PieceCapturee> PiecesCapturees { get; set; }
        private StackPanel StpParent { get; set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Construit un conteneur de pièces capturées
        /// </summary>
        /// <param name="stpParent">Lien vers le stack panel dans lequel les pièces seront affichées</param>
        public ConteneurPiecesCapturees(StackPanel stpParent)
        {
            PiecesCapturees = new List<PieceCapturee>();
            StpParent = stpParent;
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Ajouter une pièce (s'il y en a déjà de son type, on incrémente le nombre de pièces, sinon on l'ajouter à la liste)
        /// </summary>
        /// <param name="piece">La pièce à ajouter</param>
        public void AjouterPiece(Piece piece)
        {
            bool bTrouve = false;
            foreach (PieceCapturee p in PiecesCapturees)
            {
                if (p.Piece.GetType() == piece.GetType())
                {
                    p.Incrementer();
                    bTrouve = true;
                }
            }
            if (!bTrouve)
                PiecesCapturees.Add(new PieceCapturee(piece, StpParent));
        }

        /// <summary>
        /// Vider la liste (retire aussi les pièces de l'interface)
        /// </summary>
        public void Vider()
        {
            foreach (PieceCapturee piece in PiecesCapturees) piece.Effacer();
            PiecesCapturees.Clear();
        }
        #endregion
    }
}
