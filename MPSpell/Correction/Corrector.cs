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

        private IErrorModel generator;
        private ILanguageModel languageModel;

        public Corrector(IErrorModel errorModel, ILanguageModel languageModel)
        {
            this.generator = errorModel;
            this.languageModel = languageModel;
        }

        public void Correct(Ngram ngram)
        {
            Dictionary<string, double> candidates = this.generator.GeneratePossibleWords(ngram.GetLastWord());            
            
   

        }

    }
}
