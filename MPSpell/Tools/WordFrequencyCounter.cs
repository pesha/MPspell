using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public class WordFrequencyCounter : BaseCounter<string>, ICorporaOutputHandler
    {

        private string word = String.Empty;

        public new void HandleOutput(string value)
        {
            if (value != " ")
            {
                word += value;
                return;
            }
            else if (value == " " && word != "")
            {
                word = TrimSpecialChars(word);
            }
            else
            {
                return;
            }

            word = word.ToLowerInvariant();
            base.HandleOutput(word);
            word = "";
        }

        private string TrimSpecialChars(string value)
        {
            return value.Trim(new char[] { '„', '.', ',', '”', ':', '?', '!', '"', ';' });
        }


    }
}
