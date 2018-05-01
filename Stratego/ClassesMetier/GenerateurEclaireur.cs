﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class GenerateurEclaireur : GenerateurPiece
    {
        public GenerateurEclaireur() : base(8) { }

        protected override Piece CreerPiece(Couleur couleur) => new Eclaireur(couleur);
    }
}
