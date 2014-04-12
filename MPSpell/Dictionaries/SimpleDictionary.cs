using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{
    public class SimpleDictionary : List<string>, IDictionary
    {

        public bool FindWord(string word)
        {
            return this.Contains(word);
        }
    }
}
