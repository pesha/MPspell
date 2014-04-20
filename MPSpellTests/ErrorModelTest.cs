using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Extensions;
using MPSpell.Check;
using MPSpell.Dictionaries;
using MPSpell.Tools.ErrorModel;
using MPSpell.Correction;

namespace MPSpellTests
{
    [TestClass]
    public class ErrorModelTest
    {

        private IErrorModel errorModel;
        private SimpleDictionary dictionary;

        public ErrorModelTest()
        {
            dictionary = new SimpleDictionary();
            dictionary.Add("actress");
            dictionary.Add("cress");
            dictionary.Add("caress");
            dictionary.Add("access");
            dictionary.Add("across");
            dictionary.Add("acres");            

            errorModel = new ErrorModel(dictionary);
        }

        [TestMethod]
        public void GenerationTest()
        {
            Dictionary<string, double> words = errorModel.GeneratePossibleWords("acress");

            List<string> generatedWords = new List<string>();
            foreach (var pair in words)
            {
                generatedWords.Add(pair.Key);
            }

            CollectionAssert.AreEquivalent(dictionary, generatedWords);            
        }

        


    }
}
