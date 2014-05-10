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
    public class WindowTest
    {

        [TestMethod]
        public void ContextTest()
        {
            Window window = new Window();
            window.Add(new Token("i"));
            window.Add(new Token("was"));
            window.Add(new Token("ona", false, new string[] { "", "ona", "" }, 7));
            window.Add(new Token("holiday", true));
            window.Add(new Token("new"));
            MisspelledWord word = window.GetMisspelledWord();
            Assert.AreEqual("ona", word.WrongWord);

            Assert.IsTrue(word.AreNeighborsInContext());

            List<string> lc = word.GetLeftContext();
            List<string> lcCorrect = new List<string>() { "i", "was", "ona" };

            CollectionAssert.AreEqual(lcCorrect, lc);
            
            List<string> rc = word.GetRightContext();
            List<string> rcCorrect = new List<string>() { "ona", "holiday" };

            CollectionAssert.AreEqual(rcCorrect, rc);

            window = new Window();
            window.Add(new Token("is"));
            window.Add(new Token("end", true));
            window.Add(new Token("report", false, new string[] { "", "report", "" }, 7));
            window.Add(new Token("New", true));
            window.Add(new Token("Day"));

            MisspelledWord error = window.GetMisspelledWord();

            List<string> lcACorrect = new List<string>() { "report" };
            List<string> rcACorrect = new List<string>() { "report", "New" };

            var lcA = error.GetLeftContext();
            var rcA = error.GetRightContext();

            CollectionAssert.AreEqual(lcACorrect, lcA);
            CollectionAssert.AreEqual(rcACorrect, rcA);


            window = new Window();
            window.Add(new Token("attention"));
            window.Add(new Token("to"));
            window.Add(new Token("detail", true, new string[] { "", "detail", "" }, 9950));
            window.Add(new Token("New"));
            window.Add(new Token("Day"));

            error = window.GetMisspelledWord();

            var lcB = error.GetLeftContext();
            var rcB = error.GetRightContext();

            List<string> lcBCorrect = new List<string>() { "attention", "to", "detail" };
            List<string> rcBCorrect = new List<string>() { "detail" };

            CollectionAssert.AreEqual(lcBCorrect, lcB);
            CollectionAssert.AreEqual(rcBCorrect, rcB);

        }

        [TestMethod]
        public void ContextEndTest()
        {
            Window window = new Window();
            window.Add(new Token("is"));
            window.Add(new Token("end", true));
            window.Add(new Token("report", true, new string[] { "", "report", "" }, 7));
            window.Add(new Token("New"));
            window.Add(new Token("Day", true));

            MisspelledWord error = window.GetMisspelledWord();
            Assert.IsNull(error);
        }

        [TestMethod]
        public void LanguageDetectionTest()
        {
            Window window = new Window();
            window.Add(new Token("is", false, new string[] { "", "is", "" }, 7));
            window.Add(new Token("end", false, new string[] { "", "end", "" }, 7));
            window.Add(new Token("report", true, new string[] { "", "report", "" }, 7));
            window.Add(new Token("New", false, new string[] { "", "New", "" }, 7));
            window.Add(new Token("Day"));

            Assert.IsNull(window.GetMisspelledWord());

            window.Add(new Token("aa"));

            Assert.IsNotNull(window.GetMisspelledWord());
        }

        [TestMethod]
        public void ErrorsInNeighborsTest()
        {
            Window window = new Window();
            window.Add(new Token("is"));
            window.Add(new Token("end", false, new string[] { "", "end", "" }, 7));
            window.Add(new Token("report", true, new string[] { "", "report", "" }, 7));
            window.Add(new Token("New", false, new string[] { "", "New", "" }, 7));
            window.Add(new Token("Day"));

            Assert.IsNull(window.GetMisspelledWord());

            window = new Window();
            window.Add(new Token("is"));
            window.Add(new Token("end"));
            window.Add(new Token("report", true, new string[] { "", "report", "" }, 7));
            window.Add(new Token("New", false, new string[] { "", "New", "" }, 7));
            window.Add(new Token("Day"));

            MisspelledWord error = window.GetMisspelledWord();
            Assert.IsNotNull(error);
            Assert.IsTrue(error.AreNeighborsInContext());

            window = new Window();
            window.Add(new Token("is"));
            window.Add(new Token("end"));
            window.Add(new Token("New", false, new string[] { "", "New", "" }, 7));
            window.Add(new Token("report", true, new string[] { "", "report", "" }, 7));            
            window.Add(new Token("Day"));

            error = window.GetMisspelledWord();
            Assert.IsNotNull(error);
            Assert.IsTrue(error.AreNeighborsInContext());

            window = new Window();
            window.Add(new Token("is"));
            window.Add(new Token("end"));
            window.Add(new Token("New", false, new string[] { "", "New", "" }, 7));
            window.Add(new Token("report"));
            window.Add(new Token("Day", true, new string[] { "", "report", "" }, 7));
            error = window.GetMisspelledWord();
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void RotationTest()
        {
            MisspelledWord word;
            Window window = new Window();
            window.Add(new Token("test"));
            window.Add(new Token("test"));
            window.Add(new Token("test"));
            window.Add(new Token("test"));
            window.Add(new Token("test"));
            window.Add(new Token("testa", false, new string[] { "", "testa", "" }, 10));
            window.Add(new Token("test"));

            word = window.GetMisspelledWord();
            Assert.IsNull(word);

            window.Add(new Token("test"));
            word = window.GetMisspelledWord();
            Assert.IsNotNull(word);
        }


    }
}
