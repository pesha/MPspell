using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Extensions;
using MPSpell.Check;

namespace MPSpellTests
{
    [TestClass]
    public class MisspelledWordTest
    {
        [TestMethod]
        public void ContextTest()
        {
            Window window = new Window();
            window.Add(new Token("i"));
            window.Add(new Token("was"));
            window.Add(new Token("ona", false, "ona", 7));
            window.Add(new Token("holiday"));
            window.Add(new Token('.', true));

            MisspelledWord word = window.GetMisspelledWord();
            Assert.AreEqual("ona", word.WrongWord);

            List<string> lc = word.GetLeftContext();
            List<string> lcCorrect = new List<string>() { "i", "was", "ona" };

            CollectionAssert.AreEqual(lcCorrect, lc);
            
            List<string> rc = word.GetRightContext();
            List<string> rcCorrect = new List<string>() { "ona", "holiday" };

            CollectionAssert.AreEqual(rcCorrect, rc);            
        }
    }
}
