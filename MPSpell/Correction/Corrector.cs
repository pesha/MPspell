using MPSpell.Dictionaries;
using MPSpell.Check;
using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public class Corrector
    {

        private bool skipCandidatesMissingInNgrams = false;

        private IErrorModel errorModel;
        private ILanguageModel languageModel;
        private IAccentModel accentModel;

        public Corrector(IErrorModel errorModel, ILanguageModel languageModel, IAccentModel accentModel = null, bool skipCandidatesMissingInNgrams = false)
        {
            this.errorModel = errorModel;
            this.languageModel = languageModel;
            this.accentModel = accentModel;

            this.skipCandidatesMissingInNgrams = skipCandidatesMissingInNgrams;
        }

        public void Correct(MisspelledWord misspelling)
        {
            List<string> candidatesAccent = null;
            string word = null;
            double accuracy = 0;

            if (this.accentModel != null)
            {                
                candidatesAccent = this.accentModel.AddAccent(misspelling.WrongWord);
            }

            Dictionary<string, double> candidates;
            if(null != candidatesAccent && candidatesAccent.Count > 0){
                misspelling.CorrectedBy = CorrectedBy.AccentModel;
                candidates = candidatesAccent.ToDictionary();
            } else {
                misspelling.CorrectedBy = CorrectedBy.ErrorModel;
                candidates = this.errorModel.GeneratePossibleWords(misspelling.WrongWord);
            }

            if (candidates.Count > 1)
            {
                double totalProps = 0;
                Dictionary<string, double> probabilities = this.languageModel.EvaluateCandidates(misspelling, candidates);

                if (skipCandidatesMissingInNgrams && !this.languageModel.FoundAnyCandidateInNgrams())
                {
                    misspelling.RevokedByLm = true;
                    return;
                }

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

                misspelling.CorrectedBy = misspelling.CorrectedBy == CorrectedBy.ErrorModel ? CorrectedBy.ErrorAndLanguageModel : CorrectedBy.AccentAndLanguageModel;
            }
            else if (candidates.Count == 1)
            {                
                accuracy = 100;
                word = candidates.First().Key;
            }


            if (null != word)
            {
                misspelling.Accuracy = accuracy;
                misspelling.CorrectWord = word;
            }
        }

    }
}
