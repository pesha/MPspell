using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{
    public interface IDictionary
    {

        bool FindWord(string word);

    }
}
