using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionary.Affixes;
using MPSpell.Extensions;

namespace MPSpellTests
{
    [TestClass]
    public class OpenOfficeDictionaryTest
    {
        [TestMethod]
        public void ParseDescriptionTest()
        {
            OpenOfficeDictionaries dictionaries = new OpenOfficeDictionaries("dict-en.oxt");
            List<OpenOfficeDictionaryItem> list = dictionaries.GetAvailableDictionaries();
            Assert.AreEqual(5, list.Count);

            CollectionAssert.AreEqual(new string[] { "en-GB" }, list[0].Locales);
            CollectionAssert.AreEqual(new string[] { "en_GB.aff", "en_GB.dic" }, list[0].Locations);
        }
    }
}
