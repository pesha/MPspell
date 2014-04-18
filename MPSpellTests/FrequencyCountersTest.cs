using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Extensions;
using MPSpell.Check;
using MPSpell.Tools;
using MPSpell.Dictionaries;

namespace MPSpellTests
{
    [TestClass]
    public class FrequencyCounterTest
    {

        private CharFrequencyCounter charCounter;
        private TwoCharFrequencyCounter twoCharCounter;

        public FrequencyCounterTest()
        {
            DictionaryManager manager = new DictionaryManager(@"C:\dev\git\Pspell\SpellCheckerConsole\bin\Debug\dictionaries");
            Dictionary enUs = manager.GetDictionary("en_US");

            charCounter = new CharFrequencyCounter(enUs.GetAlphabetForErrorModel(true));
            twoCharCounter = new TwoCharFrequencyCounter(enUs.GetAlphabetForErrorModel(true));
        }

        [TestMethod]
        public void FrequencyCountTest()
        {
            string text = "I was there waiting for quite some time.";

            for (int i = 0; i < text.Length; i++)
            {
                charCounter.HandleOutput(text[i].ToString());
                twoCharCounter.HandleOutput(text[i].ToString());
            }
            Dictionary<string, int> frq = charCounter.GetFrequences();

            Assert.AreEqual(7, frq[" "]);
            Assert.AreEqual(16, frq.Count);
            Assert.AreEqual(4, frq["t"]);

            Dictionary<string, int> twoFrq = twoCharCounter.GetFrequences();
            Assert.AreEqual(1, twoFrq["as"]);
        }



    }
}
