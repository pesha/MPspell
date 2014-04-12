using MPSpell;
using MPSpell.Correction;
using MPSpell.Dictionaries;
using MPSpell.Dictionaries.Parsers;
using MPSpell.Extensions;
using MPSpell.Check;
using MPSpell.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellCheckerConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            //NgramParser parser = new NgramParser();
            //parser.ParseNgrams("w2_.txt");

            
            //TwoCharFrequencyCounter counter = new TwoCharFrequencyCounter(Dictionary.GetAlphabetStatic());
            //WordFrequencyCounter counter = new WordFrequencyCounter();
            //CorporaReader reader = new CorporaReader(new HCLineParser(), counter);
            //reader.ProcessFile("eng_news.txt");
            //counter.Save("word_freq.txt");
            


            DictionaryManager manager = new DictionaryManager("dictionaries");            
            Dictionary enUs = manager.GetDictionary("en_US");

            enUs.PreloadDictionaries();

            Corrector corrector = new Corrector(new ErrorModel(enUs), new LanguageModel(enUs));


            /*using (StringChecker checker = new StringChecker("My acr is blue reax.", enUs))
            {
                MisspelledWord error;

                while ((error = checker.GetNextMisspelling()) != null)
                {
                    var lc = error.GetLeftContext();
                    var rc = error.GetRightContext();
                }
            }*/

            using (FileChecker checker = new FileChecker("testen.txt", enUs))
            {
                MisspelledWord error;
                while ((error = checker.GetNextMisspelling()) != null)
                {
                   
                }
            }
            


        }



    }
}
