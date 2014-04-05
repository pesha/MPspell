using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    public class Checker
    {

        private MPSpell.Dictionaries.Dictionary dictionary;

        public Checker(MPSpell.Dictionaries.Dictionary dict)
        {
            dictionary = dict;
        }

        public bool CheckWord(string word)
        {            
            string wordLowerCase = word.ToLowerInvariant();
            bool found = false;
            foreach (string token in dictionary)
            {
                if (token == word || token == wordLowerCase)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        protected string TrimSpecialChars(string word)
        {
            // nutno predelat do regularu kvuli opakovane aplikaci
            return word.Trim(new char[] { '„', '.', ',', '”', ':' });
        }


    }
}
