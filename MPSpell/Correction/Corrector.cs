using MPSpell.Dictionaries;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public class Corrector
    {

        private IErrorModel errorModel;
        private ILanguageModel languageModel;        

        public Corrector(IErrorModel errorModel, ILanguageModel languageModel)
        {
            this.errorModel = errorModel;
            this.languageModel = languageModel;            
        }

        public MisspelledWord Correct(MisspelledWord misspelling)
        {
            Dictionary<string, double> candidates = this.errorModel.GeneratePossibleWords(misspelling.WrongWord);

            string word = null;
            double accuracy = 0;
            if (candidates.Count > 1)
            {
                double totalProps = 0;
                Dictionary<string, double> probabilities = this.languageModel.EvaluateCandidates(misspelling, candidates);
                foreach (KeyValuePair<string, double> option in candidates)
                {
                    probabilities[option.Key] *= option.Value;
                    totalProps += probabilities[option.Key];
                }
               
                double? max = null;
                foreach (KeyValuePair<string, double> pair in probabilities)
                {
                    if (null == max || pair.Value > max)
                    {
                        max = pair.Value;
                        word = pair.Key;
                        accuracy = (pair.Value * 100) / totalProps;
                    }
                }
            }
            else if(candidates.Count == 1)
            {
                accuracy = 100;
                word = candidates.First().Key;
            }

            if (null != word)
            {
                misspelling.Accuracy = accuracy;
                misspelling.CorrectWord = word;
            }

            return misspelling;
        }

    }
}
