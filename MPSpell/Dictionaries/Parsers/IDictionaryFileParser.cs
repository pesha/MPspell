using MPSpell.Dictionaries.Affixes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Parsers
{

    public interface IDictionaryFileParser
    {

        DictionaryWithFlags Parse(string file, Encoding encoding = null);

    }

}
