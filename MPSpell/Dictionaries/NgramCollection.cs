using MPSpell.Correction;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{

    public class NgramEvaluation
    {

        public double Probability { get; private set; }
        public int Occurence { get; private set; }


        public NgramEvaluation(double probability, int occurence)
        {
            Probability = probability;
            Occurence = occurence;
        }

    }

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
       
        public NgramEvaluation GetProbability(string[] context)
        {            
            int lastOccurence = this.ngramTree.GetOccurences(context);

            //return (double) ((occurence == 0) ? 1 : occurence) / NgramCount;
            // add one smoothing
            double prop = (double)(lastOccurence + AddOneConstant) / (NgramCount + AddOneConstant * UniqueNgrams);
            return new NgramEvaluation(prop, lastOccurence);
        }

    }
}
