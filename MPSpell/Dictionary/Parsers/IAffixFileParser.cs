using MPSpell.Dictionary.Affixes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell
{

    public interface IAffixFileParser
    {

        AffixRules Parse(string file, Encoding encoding = null);

    }

}
