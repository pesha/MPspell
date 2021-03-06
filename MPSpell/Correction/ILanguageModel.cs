﻿using MPSpell.Dictionaries;
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

        LanguageModelEvaluation EvaluateCandidates(MisspelledWord word, Dictionary<string, double> candidates);        

    }
}
