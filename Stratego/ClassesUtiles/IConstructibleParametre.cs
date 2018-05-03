using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public interface IConstructibleParametre
    {
        void Construire(Dictionary<string, ParametresConstruction> parametres);
    }
}
