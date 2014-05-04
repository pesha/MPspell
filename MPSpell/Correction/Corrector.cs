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
        private IAccentModel accentModel;

        public Corrector(IErrorModel errorModel, ILanguageModel languageModel, IAccentModel accentModel = null)
        {
            this.errorModel = errorModel;
            this.languageModel = languageModel;
            this.accentModel = accentModel;
        }

        public MisspelledWord Correct(MisspelledWord misspelling)
        {
            string word = null;
            double accuracy = 0;
            if (this.accentModel != null)
            {                
                word = this.accentModel.AddAccent(misspelling.WrongWord);
            }

            if (word == null)
            {
                Dictionary<string, double> candidates = this.errorModel.GeneratePossibleWords(misspelling.WrongWord);

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
                else if (candidates.Count == 1)
                {
                    accuracy = 100;
                    word = candidates.First().Key;
                }
            }
            else
            {
                accuracy = 100;
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
