using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionaries.Affixes;

namespace MPSpellTests
{
    [TestClass]
    public class AffixRulesTest
    {
        [TestMethod]
        public void WordGeneratorTest()
        {
            
            Rule ruleD = new Rule("D", Combinable.Y, RuleType.SFX);
            ruleD.Add(new RuleItem("0","d","e"));
            ruleD.Add(new RuleItem("y", "ied", "[^aeiou]y"));
            ruleD.Add(new RuleItem("0", "ed", "[^ey]"));
            ruleD.Add(new RuleItem("0", "ed", "[aeiou]y"));

            Rule ruleG = new Rule("G", Combinable.Y, RuleType.SFX);
            ruleG.Add(new RuleItem("e", "ing", "e"));
            ruleG.Add(new RuleItem("0", "ing", "[^e]"));

            AffixRules rules = new AffixRules();
            rules.Add(ruleD);
            rules.Add(ruleG);

            DictionaryItemWithFlags item = new DictionaryItemWithFlags("create", "DG");

            List<string> words = rules.GetPossibleWords(item);

            List<string> correctWords = new List<string>()
            {
                "create", "created", "creating"
            };

            CollectionAssert.AreEqual(correctWords, words);

            Rule ruleH = new Rule("H", Combinable.Y, RuleType.SFX);
            ruleH.Add(new RuleItem("0", "u", "[^ey]"));
            ruleH.Add(new RuleItem("0", "e", "[^eyghkc]"));
            ruleH.Add(new RuleItem("0", "em", "[^ey]"));
            ruleH.Add(new RuleItem("0", "y", "[^ey]"));
            ruleH.Add(new RuleItem("0", "ů", "[^ey]"));
            ruleH.Add(new RuleItem("0", "ům", "[^ey]"));
            ruleH.Add(new RuleItem("0", "ech", "[^eyghk]"));
            ruleH.Add(new RuleItem("g", "zích", "g"));
            ruleH.Add(new RuleItem("h", "zích", "[^c]h"));
            ruleH.Add(new RuleItem("ch", "ších", "ch"));
            ruleH.Add(new RuleItem("k", "cích", "k"));
            ruleH.Add(new RuleItem("e", "u", "e"));
            ruleH.Add(new RuleItem("0", "m", "e"));
            ruleH.Add(new RuleItem("e", "y", "e"));
            ruleH.Add(new RuleItem("e", "ů", "e"));
            ruleH.Add(new RuleItem("e", "ům", "e"));
            ruleH.Add(new RuleItem("e", "ech", "e"));
            ruleH.Add(new RuleItem("y", "ů", "y"));
            ruleH.Add(new RuleItem("y", "ům", "y"));
            ruleH.Add(new RuleItem("y", "ech", "[^ghk]y"));
            ruleH.Add(new RuleItem("gy", "zích", "gy"));
            ruleH.Add(new RuleItem("hy", "zích", "[^c]hy"));
            ruleH.Add(new RuleItem("chy", "ších", "chy"));
            ruleH.Add(new RuleItem("ky", "cích", "ky"));

            rules = new AffixRules();
            rules.Add(ruleH);

            words = rules.GetPossibleWords(new DictionaryItemWithFlags("abakus", "H"));

            correctWords = new List<string>()
            {
                "abakus", 
                "abakusu",
                "abakuse",
                "abakusem",
                "abakusy",
                "abakusů",
                "abakusům",
                "abakusech"
            };

            CollectionAssert.AreEqual(correctWords, words);
        }
    }
}
