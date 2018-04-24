using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Stratego
{
    public class SelectionneurPieces
    {

        private Action<Piece> MethodeRetourDemandePiece { get; set; }

        public SelectionneurPieces(Grid grille)
        {
            

            MethodeRetourDemandePiece = null;
        }

        public void DemanderPiece(Action<Piece> methodeRetour) => MethodeRetourDemandePiece = methodeRetour;
    }
}
