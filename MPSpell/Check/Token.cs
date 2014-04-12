using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    public class Token
    {

        public string[] WordContext { get; private set; }
        public string Word { get; private set; }        
        public int? Position { get; private set; }
        public bool ContextEnd { get; private set; }
        public string RawWord
        {
            get
            {
                return WordContext[1];
            }
        }

        public Token(string word, bool contextEnd = false, string[] wordContext = null, int? position = null)
        {
            Word = word;            
            Position = position;
            ContextEnd = contextEnd;
            WordContext = wordContext;
        }

        public Token(char end, bool contextEnd = true)
        {
            Word = end.ToString();
            ContextEnd = contextEnd;
        }


    }
}
