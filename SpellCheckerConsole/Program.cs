using MPSpell;
using MPSpell.Correction;
using MPSpell.Dictionaries;
using MPSpell.Dictionaries.Parsers;
using MPSpell.Extensions;
using MPSpell.Check;
using MPSpell.Tools;
using MPSpell.Tools.ErrorModel;
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
            //CharFrequencyCounter counter = new CharFrequencyCounter(Dictionary.GetAlphabetStatic());
            //CorporaReader reader = new CorporaReader(new HCLineParser(), counter);
            //reader.ProcessFile("eng_news.txt");
            //counter.Save("char_freq_en2.txt");

            DictionaryManager manager = new DictionaryManager("gen"); //dictionaries
            Dictionary enUs = manager.GetDictionary("en_US");

            //enUs.PreloadDictionaries();

            string path = @"C:\dev\git\Pspell\SpellCheckerConsole\bin\Debug\gen\data"; //@"F:\_dp\english\news";
            DictionaryGenerator generator = new DictionaryGenerator(enUs, path);
            //generator.CalculateFrequences();
            //generator.Save();
            generator.RunBatch();

            /*ErrorListParser parser = new ErrorListParser("generators/en_errors.txt");            
            var data = parser.Parse();

            InsertionsMatrixGenerator generator = new InsertionsMatrixGenerator(enUs.GetAlphabetForErrorModel(true).ToCharArray());
            var matrix = generator.GenerateMatrix(data);
            MatrixExport.ExportMatrix("insertTest.txt", matrix);
                       

            FolderCorrector analyze = new FolderCorrector(enUs, @"C:\dev\git\Pspell\SpellCheckerConsole\bin\Debug\20_newsgroups");
            analyze.CorrectFiles();
            */


            

            Corrector corrector = new Corrector(new ErrorModel(enUs), new LanguageModel(enUs));

            List<MisspelledWord> mistakes = new List<MisspelledWord>();
            using (FileChecker checker = new FileChecker("testen2.txt", enUs))
            {
                MisspelledWord error;               
                while ((error = checker.GetNextMisspelling()) != null)
                {
                    mistakes.Add(error);
                }
            }

            foreach (MisspelledWord word in mistakes)
            {
                corrector.Correct(word);
            }


            FileCorrectionHandler handler = new FileCorrectionHandler("testen2.txt", mistakes);
            handler.SaveCorrectedAs("testFixed.txt");
            //handler.OverwriteWithCorrections();



        }



    }
}
