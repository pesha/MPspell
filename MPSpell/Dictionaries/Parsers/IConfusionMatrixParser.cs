using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Parsers
{
    public interface IConfusionMatrixParser
    {

        ConfusionMatrix ParseMatrix(string file);

    }
}
