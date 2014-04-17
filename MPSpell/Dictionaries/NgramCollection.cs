using MPSpell.Correction;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{
    public class NgramCollection
    {

        public int NgramCount { get; set; }

        NgramNode ngramTree = new NgramNode();

        public void Add(Ngram ngram)
        {
            NgramCount += ngram.Frequency;
            ngramTree.Add(ngram);
        }
       
        public double GetProbability(string[] context)
        {
            int occurence = this.ngramTree.GetOccurences(context);
 
            return (double) ((occurence == 0) ? 1 : occurence) / NgramCount;
        }

    }
}
