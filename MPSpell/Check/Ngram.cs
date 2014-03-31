using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{

    public class Ngram
    {

        private static readonly int Capacity = 3;

        private Queue<string> history = new Queue<string>();
        private string rawWord;
        private uint end;

        public void Add(string word)
        {
            history.Enqueue(word);
            if (history.Count > Ngram.Capacity)
            {
                history.Dequeue();
            }
        }

        public Ngram DeepCopy(string word, uint position)
        {
            Ngram res = (Ngram)this.MemberwiseClone();
            res.history = new Queue<string>(history);
            res.rawWord = word;
            res.end = position;

            return res;
        }

    }

}
