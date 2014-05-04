using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSpell.Extensions;

namespace MPSpell.Dictionaries
{

    // toto by melo byt asi primo v testech
    public class SimpleDictionary : List<string>, IDictionary
    {

        public bool FindWord(string word)
        {
            return this.Contains(word);
        }

        public string[] GetAlphabetAsString()
        {
            return "abcdefghijklmnopqrstuvwxyz".ToCharArray().ToStringArray();
        }

        public int GetConfusionFrequency(char x, char y, Correction.EditOperation operation)
        {
            return 1;
        }

        public int GetOneCharFrequency(string str)
        {
            return 1;
        }

        public int GetTwoCharFrequency(string str)
        {
            return 1;
        }

        public string GetWordBoundaryRegex()
        {
            return null;
        }

    }
}
