using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{

    public class Dictionary : List<string>
    {

        public string FileName { get; set; }
        public string SuffixFileName { get; set; }        
        public string Location { get; set; }

        public List<string> ConfusionMatrixesFiles = new List<string>();
        public List<string> OtherFiles = new List<string>();

        public List<ConfusionMatrix> ConfusionMatrixes = new List<ConfusionMatrix>();

        public bool FindWord(string word, bool caseSensitive = false)
        {
            return this.Contains(word) ? true : (this.Contains(word.ToLowerInvariant()) ? true : false);
        }

        public string[] GetAlphabet()
        {
            return Dictionary.GetAlphabetStatic();
        }

        public static string[] GetAlphabetStatic()
        {
            return new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        }

    }

}
