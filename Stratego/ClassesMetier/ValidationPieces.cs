using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public static class ValidationPieces
    {
        private static readonly Dictionary<Type, int> NOMBRE_MAX_PIECES = new Dictionary<Type, int>
        {
            { typeof(Marechal), 1 },
            { typeof(General), 1 },
            { typeof(Colonel), 2 },
            { typeof(Commandant), 3 },
            { typeof(Capitaine), 4 },
            { typeof(Lieutenant), 4 },
            { typeof(Sergent), 4 },
            { typeof(Demineur), 5 },
            { typeof(Eclaireur), 8 },
            { typeof(Espion), 1 },
            { typeof(Drapeau), 1 },
            { typeof(Bombe), 6 }
        };
        private static int CompterPiece<TypePiece>(List<Piece> pieces)
        {
            int nombrePieces = 0;
            foreach (Piece piece in pieces)
                if (piece is TypePiece) ++nombrePieces;
            return nombrePieces;
        }

        public static bool ValiderNombrePiece<TypePiece>(List<Piece> pieces)
            => CompterPiece<TypePiece>(pieces) == NOMBRE_MAX_PIECES[typeof(TypePiece)];
    }
}
