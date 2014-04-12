using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Extensions;
using MPSpell.Check;
using MPSpell.Dictionaries;
using System.Text.RegularExpressions;

namespace MPSpellTests
{
    [TestClass]
    public class TokenizerTest
    {

        private Tokenizer tokenizer;

        public TokenizerTest()
        {
            SimpleDictionary dict = new SimpleDictionary();
            dict.Add("testing");            


            tokenizer = new Tokenizer(dict);
        }

        [TestMethod]
        public void HandleCharTest()
        {
            string text = "Testing error 'detection'";

            List<MisspelledWord> mistakes = new List<MisspelledWord>();
            MisspelledWord mistake;
            for (int i = 0; i < text.Length; i++)
            {
                mistake = this.tokenizer.HandleChar(text[i]);
                if (null != mistake)
                {
                    mistakes.Add(mistake);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                mistake = this.tokenizer.HandleChar('.', true);
                if (null != mistake)
                {
                    mistakes.Add(mistake);
                }
            }

            Assert.AreEqual(8, mistakes[0].GetPosition());
            Assert.AreEqual(14, mistakes[1].GetPosition());

            Assert.AreEqual("'", mistakes[1].WordContext[0]);
            Assert.AreEqual("'", mistakes[1].WordContext[2]);
            Assert.AreEqual("'detection'", mistakes[1].RawWord);                

            List<string> errors = new List<string>();
            foreach (MisspelledWord word in mistakes)
            {
                errors.Add(word.WrongWord);
            }            

            List<string> testErrors = new List<string>() { "error", "detection" };
            CollectionAssert.AreEqual(testErrors, errors);            
        }

    }
}
