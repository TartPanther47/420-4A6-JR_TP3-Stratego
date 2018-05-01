using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public abstract class GenerateurPiece
    {
        private int NombreMax { get; set; }
            // Le nombre de pièces restantes de ce type
        public int Nombre { get; private set; }

        public GenerateurPiece(int nombre)
        {
            Nombre = nombre;
            NombreMax = nombre;
        }

        public bool EstGenerable() => Nombre > 0;

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

        public void IncrementerNombre()
        {
            if (Nombre < NombreMax) ++Nombre;
        }
    }
}
