using MPSpell.Dictionaries;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public interface ILanguageModel
    {

        Dictionary<string, double> EvaluateCandidates(MisspelledWord word, Dictionary<string, double> candidates);
        bool FoundAnyCandidateInNgrams();

    }
}
