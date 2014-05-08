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
using MPSpell.Correction;

namespace MPSpellTests
{
    [TestClass]
    public class DictionaryTest
    {

        Dictionary dictionary;
        Dictionary csCZ;

        public DictionaryTest()
        {
            dictionary = new Dictionary(new DictionaryLoader(new DefaultDictionaryFileParser()),
                "en_US",
                "gen",
                "abcdefghijklmnopqrstuvwxyz".ToCharArray()
            );

            dictionary.Add("acres");


            Dictionary<char, List<char>> accentPairs = new Dictionary<char, List<char>>();
            accentPairs.Add('a', new List<char>() { 'á' });
            accentPairs.Add('e', new List<char>() { 'é', 'ě' });
            accentPairs.Add('i', new List<char>() { 'í' });
            accentPairs.Add('o', new List<char>() { 'ó' });
            accentPairs.Add('u', new List<char>() { 'ú', 'ů' });
            accentPairs.Add('y', new List<char>() { 'ý' });
            accentPairs.Add('c', new List<char>() { 'č' });
            accentPairs.Add('d', new List<char>() { 'ď' });            
            accentPairs.Add('n', new List<char>() { 'ň' });
            accentPairs.Add('r', new List<char>() { 'ř' });
            accentPairs.Add('s', new List<char>() { 'š' });
            accentPairs.Add('t', new List<char>() { 'ť' });
            accentPairs.Add('z', new List<char>() { 'ž' });


            csCZ = new Dictionary(new DictionaryLoader(new DefaultDictionaryFileParser()),
                "cs_CZ",
                "gen",
                "abcdefghijklmnopqrstuvwxyzáéíóúýčďěňřšťžů".ToCharArray(),
                null,
                "[a-záéíóúýčďěňřšťžů]+",
                accentPairs);

            csCZ.Add("večeře");
            csCZ.Add("véčera");
            csCZ.Add("věci");            
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

        [TestMethod]
        public void AccentTest()
        {
            string word = "vecere";

            AccentModel model = new AccentModel(this.csCZ);
            List<string> result = model.AddAccent(word);

            Assert.AreEqual("večeře", result[0]);
        }



    }
}
