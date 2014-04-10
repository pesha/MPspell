using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    public class WindowItem
    {

        public string Word { get; private set; }
        public string RawWord { get; private set; }
        public uint? Position { get; private set; }
        public bool SentenceEnd { get; private set; }

        public WindowItem(string word, string rawWord = null, uint? position = null)
        {
            Word = word;
            RawWord = rawWord;
            Position = position;
        }

        public WindowItem(char end, bool sentenceEnd = true)
        {
            Word = end.ToString();
            SentenceEnd = sentenceEnd;
        }


    }
}
