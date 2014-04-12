using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Extensions;
using MPSpell.Check;
using MPSpell.Dictionaries;

namespace MPSpellTests
{
    [TestClass]
    public class NgramCollectionTest
    {

        private NgramCollection collection;

        public NgramCollectionTest()
        {
            collection = new NgramCollection();

            collection.Add(new Ngram(new string[] { 
            "test", "love"
            }, 10));
            collection.Add(new Ngram(new string[] { 
            "raw", "world"
            }, 15));
            collection.Add(new Ngram(new string[] { 
            "new", "order"
            }, 5));
        }



        [TestMethod]
        public void ProbabilityTest()
        {
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual(30, collection.NgramCount);

            double probability = collection.GetProbability(new string[] { "raw", "world" });
            Assert.AreEqual((double)0.5, probability);
        }


    }
}
