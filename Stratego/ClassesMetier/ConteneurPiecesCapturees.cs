using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Stratego
{
    public class ConteneurPiecesCapturees
    {
        private List<PieceCapturee> PiecesCapturees { get; set; }
        private StackPanel StpParent { get; set; }

        public ConteneurPiecesCapturees(StackPanel stpParent)
        {
            PiecesCapturees = new List<PieceCapturee>();
            StpParent = stpParent;
        }

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

        public void Vider()
        {
            foreach (PieceCapturee piece in PiecesCapturees) piece.Effacer();
            PiecesCapturees.Clear();
        }
    }
}
