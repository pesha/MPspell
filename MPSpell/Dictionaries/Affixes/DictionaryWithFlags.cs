using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Affixes
{

    public class DictionaryWithFlags : List<DictionaryItemWithFlags>
    {

    }

    public class DictionaryItemWithFlags
    {

        public string Word { get; private set; }
        public string Flags { get; private set; }

        public DictionaryItemWithFlags(string word, string flags)
        {
            Word = word;
            Flags = flags;
        }

        public string[] GetFlags()
        {
            return Flags.ToCharArray().Select(c => c.ToString()).ToArray();
        }

    }

}
