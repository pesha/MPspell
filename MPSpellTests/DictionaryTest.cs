using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Extensions;
using MPSpell.Check;
using MPSpell.Dictionaries;
using System.Text.RegularExpressions;
using MPSpell.Dictionaries.Parsers;

namespace MPSpellTests
{
    [TestClass]
    public class DictionaryTest
    {

        Dictionary dictionary;

        public DictionaryTest()
        {
            dictionary = new Dictionary(new DictionaryLoader(new DefaultDictionaryFileParser()),
                "en_US",
                "gen",
                "abcdefghijklmnopqrstuvwxyz".ToCharArray()
            );

            dictionary.Add("acres");
        }

        [TestMethod]
        public void SearchTest()
        {
            bool res = dictionary.FindWord("acr");
            Assert.IsFalse(res);

            bool res2 = dictionary.FindWord("acres");
            Assert.IsTrue(res2);

            bool res3 = dictionary.FindWord("super");
            Assert.IsFalse(res3);
        }



    }
}
