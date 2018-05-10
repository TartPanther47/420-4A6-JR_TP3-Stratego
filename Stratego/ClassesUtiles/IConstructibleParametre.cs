// Auteur: Clément Gassmann-Prince
// Date de dernière modification: 2018-05-10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Interface de construction avec paramètres
    /// </summary>
    public interface IConstructibleParametre
    {
        /// <summary>
        /// Construire avec des paramètres
        /// </summary>
        /// <param name="parametres"></param>
        void Construire(Dictionary<string, ParametresConstruction> parametres);
    }
}
