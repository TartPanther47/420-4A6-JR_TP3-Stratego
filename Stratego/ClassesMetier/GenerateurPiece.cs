using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public abstract class GenerateurPiece
    {
            // Le nombre de pièces restantes de ce type
        private int Nombre { get; set; }

        public GenerateurPiece(int nombre)
            => Nombre = nombre;

        public bool EstGenerable() => Nombre >= 0; // TODO: check if > OR >=

        protected abstract Piece CreerPiece(Couleur couleur);

        public Piece GenererPiece(Couleur couleur)
        {
            if(EstGenerable())
            {
                --Nombre;
                return CreerPiece(couleur);
            }
            return new PieceNulle(couleur);
        }
    }
}
