using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    abstract public class Checker
    {

        protected MPSpell.Dictionaries.Dictionary dictionary;

        public Checker(MPSpell.Dictionaries.Dictionary dict)
        {
            dictionary = dict;
        }

        protected string TrimSpecialChars(string word)
        {
            // nutno predelat do regularu kvuli opakovane aplikaci
            return word.Trim(new char[] { '„', ',', '”', '\'', ':' });
        }

        protected bool HasSentenceEnded(char chr)
        {
            bool end = false;
            switch (chr)
            {
                case '.':
                case '?':
                case '!':
                    end = true;
                    break;
            }

            return end;
        }


    }
}
