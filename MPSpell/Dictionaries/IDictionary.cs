using MPSpell.Correction;
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
        string[] GetAlphabetAsString();
        string[] GetAlphabetForErrorModel(bool withSpace = false);
        char[] GetSpecialCharsInsideWord();
        int GetConfusionFrequency(char x, char y, EditOperation operation);
        int GetOneCharFrequency(string str);
        int GetTwoCharFrequency(string str);
        string GetWordBoundaryRegex();

    }
}
