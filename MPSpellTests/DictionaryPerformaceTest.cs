using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Extensions;
using MPSpell.Check;
using MPSpell.Dictionaries;
using System.Diagnostics;
using System.IO;
using System.Text;
using MPSpell.Correction;

namespace MPSpellTests
{
    [TestClass]
    public class DictionaryPerformanceTest
    {

        public TestContext TestContext { get; set; }

        private List<string> testWords = new List<string>();

        public DictionaryPerformanceTest()
        {
            using (StreamReader reader = new StreamReader("sample.txt", Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    testWords.Add(reader.ReadLine());
                }
            }
        }

        //[TestMethod]
        public void CorrectionTest()
        {
            DictionaryManager manager = new DictionaryManager(@"C:\dev\git\Pspell\SpellCheckerConsole\bin\Debug\dictionaries");
            Dictionary enUs = manager.GetDictionary("en_US");

            enUs.PreloadDictionaries();

            Corrector corrector = new Corrector(new ErrorModel(enUs), new LanguageModel(enUs));

            Stopwatch mistakesTime = Stopwatch.StartNew();
            List<MisspelledWord> mistakes = new List<MisspelledWord>();
            using (FileChecker checker = new FileChecker("testarticle.txt", enUs))
            {
                MisspelledWord error;
                while ((error = checker.GetNextMisspelling()) != null)
                {
                    mistakes.Add(error);
                }
            }
            mistakesTime.Stop();

            Stopwatch correctionTime = Stopwatch.StartNew();
            foreach (MisspelledWord word in mistakes)
            {
                corrector.Correct(word);
            }
            correctionTime.Stop();

            TestContext.WriteLine("Mistakes search time: " + mistakesTime.ElapsedMilliseconds + " ms");
            TestContext.WriteLine("Correction time: " + correctionTime.ElapsedMilliseconds + " ms");
        }

        //[TestMethod]
        public void LookupTest()
        {
            Stopwatch loadTime = Stopwatch.StartNew();
            DictionaryManager manager = new DictionaryManager(@"C:\dev\git\Pspell\SpellCheckerConsole\bin\Debug\dictionaries");
            Dictionary enUs = manager.GetDictionary("en_US");            
            enUs.PreloadDictionaries();
            loadTime.Stop();

            TestContext.WriteLine("Dictionary load time: " + loadTime.ElapsedMilliseconds + " ms");

            Stopwatch searchTime = Stopwatch.StartNew();
            foreach (string word in testWords)
            {
                enUs.FindWord(word);
            }
            searchTime.Stop();

            TestContext.WriteLine("Search time: " + searchTime.ElapsedMilliseconds + " ms");
        }


        public void GenerateSampleWordlist()
        {
            DictionaryManager manager = new DictionaryManager(@"C:\dev\git\Pspell\SpellCheckerConsole\bin\Debug\dictionaries");
            Dictionary enUs = manager.GetDictionary("en_US");
            enUs.PreloadDictionaries();

            /*for (int i = 0; i < enUs.Count; i += 100)
            {
                testWords.Add(enUs[i]);
            }

            FileStream stream = new FileStream("sample.txt", FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (string testWord in testWords)
                {
                    writer.WriteLine(testWord);
                }
            }*/
        }


    }
}
