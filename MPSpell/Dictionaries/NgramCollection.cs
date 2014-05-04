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

        private readonly int AddOneConstant = 1;

        public int NgramCount { get; private set; }
        public int UniqueNgrams { get; private set; }

        NgramNode ngramTree = new NgramNode();
        
        private int lastOccurence;
        
        public void Add(Ngram ngram)
        {
            NgramCount += ngram.Frequency;
            UniqueNgrams++;

            ngramTree.Add(ngram);
        }
       
        public double GetProbability(string[] context)
        {            
            lastOccurence = this.ngramTree.GetOccurences(context);

            // add one smoothing
            return (double)(lastOccurence + AddOneConstant) / (NgramCount + AddOneConstant * UniqueNgrams); 
            //return (double) ((occurence == 0) ? 1 : occurence) / NgramCount;
        }

        public int GetLastOccurence()
        {
            return lastOccurence;
        }

    }
}
