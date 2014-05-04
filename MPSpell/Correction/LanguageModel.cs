using MPSpell.Dictionaries;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public class LanguageModel : ILanguageModel
    {

        private Dictionary dictionary;

        public LanguageModel(Dictionary dict)
        {
            dictionary = dict;
        }

        public Dictionary<string, double> EvaluateCandidates(MisspelledWord word, Dictionary<string, double> candidates)
        {
            List<string> leftContext = word.GetLeftContext();

            
            NgramType type = this.dictionary.GetHighestAvailableNgramCollection(leftContext.Count);

            Dictionary<string, double> probability = new Dictionary<string, double>();
            string[] lcArray = this.GetLeftContext(leftContext, type);            
            foreach (KeyValuePair<string, double> option in candidates)
            {
                lcArray[leftContext.Count - 1] = option.Key;
                probability.Add(option.Key, this.dictionary.GetNgramCollection(type).GetProbability(lcArray));
            }

            List<string> rightContext = word.GetRightContext();
            NgramType secType = this.dictionary.GetHighestAvailableNgramCollection(rightContext.Count);

            if (type == NgramType.Unigram && type == NgramType.Unigram)
            {
                // do nothing
            }
            else
            {
                string[] rcArray = this.GetRightContext(rightContext, secType);
                foreach (KeyValuePair<string, double> option in candidates)
                {
                    rcArray[0] = option.Key;
                    probability[option.Key] *= this.dictionary.GetNgramCollection(secType).GetProbability(rcArray);
                }
            }

            return probability;
        }

        private string[] GetLeftContext(List<string> context, NgramType type)
        {
            if (type == NgramType.Digram && context.Count == 3)
            {
                context.RemoveAt(0);
            }

            if (type == NgramType.Unigram && context.Count == 3)
            {                
                context.RemoveRange(0, 2);
            }

            if (type == NgramType.Unigram && context.Count == 2)
            {
                context.RemoveAt(0);
            }

            return context.ToArray();
        }

        public string[] GetRightContext(List<string> context, NgramType type)
        {
            if (type == NgramType.Digram && context.Count == 3)
            {
                context.RemoveAt(2);
            }

            if (type == NgramType.Unigram && context.Count == 3)
            {
                context.RemoveRange(1, 2);
            }

            if (type == NgramType.Unigram && context.Count == 2)
            {
                context.RemoveAt(1);
            }

            return context.ToArray();
        }

    }
}
