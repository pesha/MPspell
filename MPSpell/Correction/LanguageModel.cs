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

            NgramType type = NgramType.Unigram;
            switch (leftContext.Count)
            {
                case 2:
                    type = NgramType.Digram;
                    break;

                case 3:
                    type = NgramType.Trigram;
                    break;
            }

            Dictionary<string, double> probability = new Dictionary<string, double>();
            string[] lcArray = leftContext.ToArray();
            foreach (KeyValuePair<string, double> option in candidates)
            {
                lcArray[leftContext.Count - 1] = option.Key;
                probability.Add(option.Key, this.dictionary.GetNgramCollection(type).GetProbability(lcArray));
            }

            List<string> rightContext = word.GetRightContext();            
            switch (rightContext.Count)
            {
                case 2:
                    type = NgramType.Digram;
                    break;

                case 3:
                    type = NgramType.Trigram;
                    break;      
          
                default:
                    type = NgramType.Unigram;
                    break;
            }

            string[] rcArray = rightContext.ToArray();
            foreach (KeyValuePair<string, double> option in candidates)
            {
                rcArray[0] = option.Key;
                probability[option.Key] *= this.dictionary.GetNgramCollection(type).GetProbability(rcArray);
            }

            return probability;
        }

    }
}
