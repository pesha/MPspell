using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    public class Token
    {

        public string Word { get; private set; }
        public string RawWord { get; private set; }
        public uint? Position { get; private set; }
        public bool ContextEnd { get; private set; }

        public Token(string word, bool contextEnd = false, string rawWord = null, uint? position = null)
        {
            Word = word;
            RawWord = rawWord;
            Position = position;
            ContextEnd = contextEnd;
        }

        public Token(char end, bool contextEnd = true)
        {
            Word = end.ToString();
            ContextEnd = contextEnd;
        }


    }
}
