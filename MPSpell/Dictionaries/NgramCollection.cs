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
        
        public void Add(Ngram ngram)
        {
            NgramCount += ngram.Frequency;
            UniqueNgrams++;

            ngramTree.Add(ngram);
        }
       
        public double GetProbability(string[] context)
        {            
            int occurence = this.ngramTree.GetOccurences(context);

            // add one smoothing
            return (double) (occurence + AddOneConstant) / (NgramCount + AddOneConstant * UniqueNgrams); 
            //return (double) ((occurence == 0) ? 1 : occurence) / NgramCount;
        }

    }
}
