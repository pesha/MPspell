using MPSpell.Correction;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{
    public class NgramCollection : List<Ngram>
    {

        public int NgramCount { get; set; }

        public new void Add(Ngram ngram)
        {
            NgramCount += ngram.Frequency;
            base.Add(ngram);
        }

        public double GetProbability(string[] context)
        {
            int occurence = 1;

            foreach (Ngram item in this)
            {
                if (Enumerable.SequenceEqual(item.Words, context))
                {
                    occurence = item.Frequency;
                    break;
                }
            }

            return (double) occurence / NgramCount;
        }

    }
}
