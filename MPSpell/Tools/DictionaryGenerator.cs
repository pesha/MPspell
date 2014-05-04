using MPSpell.Correction;
using MPSpell.Dictionaries;
using MPSpell.Check;
using MPSpell.Tools.ErrorModel;
using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public class DictionaryGenerator
    {

        private Dictionary dictionary;
        private string directory;
        private string outputDirectory;
        private List<string> allowedExtensions = new List<string>() { ".txt", "" };
        private ILanguageModel languageModel;
        private IErrorModel errorModel;

        private InsertionsMatrixGenerator insGen;
        private DeletionsMatrixGenerator delGen;
        private SubstitutionsMatrixGenerator subGen;
        private TranspositionsMatrixGenerator trnGen;

        private CharFrequencyCounter charCounter;
        private TwoCharFrequencyCounter twoCharCounter;

        private Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

        public DictionaryGenerator(Dictionary dictionary, string directory, string outputDirectory)
        {            
            this.dictionary = dictionary;
            this.outputDirectory = outputDirectory;
            this.directory = directory;            
            this.errorModel = new MPSpell.Correction.ErrorModel(dictionary);
            this.languageModel = new LanguageModel(dictionary);

            int initValue = 1;

            char[] alphabetWithSpace = dictionary.GetAlphabetForErrorModel(true).ToCharArray();
            char[] alphabet = dictionary.GetAlphabetForErrorModel().ToCharArray();
            insGen = new InsertionsMatrixGenerator(alphabetWithSpace, initValue);
            delGen = new DeletionsMatrixGenerator(alphabetWithSpace, initValue);
            subGen = new SubstitutionsMatrixGenerator(alphabet, initValue);
            trnGen = new TranspositionsMatrixGenerator(alphabet, initValue);

            charCounter = new CharFrequencyCounter(alphabetWithSpace.ToStringArray());
            twoCharCounter = new TwoCharFrequencyCounter(alphabetWithSpace.ToStringArray());
        }

        public void CalculateFrequences()
        {
            List<FileInfo> files = this.AnalyzeDir(new DirectoryInfo(this.directory));

            foreach (FileInfo file in files)
            {                
                Encoding enc = EncodingDetector.DetectEncoding(file.FullName);
                using (StreamReader reader = new StreamReader(file.FullName, enc))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        for (int i = 0; i < line.Length; i++)
                        {
                            charCounter.HandleOutput(line[i].ToString());
                            twoCharCounter.HandleOutput(line[i].ToString());
                        }
                    }
                }
            }

            charCounter.Save(this.outputDirectory + "/oneCharFr.txt");
            twoCharCounter.Save(this.outputDirectory + "/twoCharFr.txt");
        }

        public void RunBatch()
        {
            List<FileInfo> files = this.AnalyzeDir(new DirectoryInfo(this.directory));

            dictionary.PreloadDictionaries();
            

            Corrector corrector = new Corrector(errorModel, languageModel);

            foreach (FileInfo file in files)
            {

                using (FileChecker checker = new FileChecker(file.FullName, dictionary))
                {
                    while (!checker.EndOfCheck)
                    {
                        MisspelledWord error = checker.GetNextMisspelling();
                        if (null != error)
                        {
                            corrector.Correct(error);
                            if (null != error.CorrectWord)
                            {
                                if (!this.data.ContainsKey(error.CorrectWord))
                                {
                                    this.data.Add(error.CorrectWord, new List<string> { error.RawWord });
                                }
                                else
                                {
                                    if (!this.data[error.CorrectWord].Contains(error.RawWord))
                                    {
                                        this.data[error.CorrectWord].Add(error.RawWord);
                                    }
                                }
                            }
                        }
                    }
                }

            }

            this.Save();
        }

        public void Save()
        {
            MatrixExport.ExportMatrix(this.outputDirectory + "/ins.txt", insGen.GenerateMatrix(this.data));
            MatrixExport.ExportMatrix(this.outputDirectory + "/del.txt", delGen.GenerateMatrix(this.data));
            MatrixExport.ExportMatrix(this.outputDirectory + "/sub.txt", subGen.GenerateMatrix(this.data));
            MatrixExport.ExportMatrix(this.outputDirectory + "/trn.txt", trnGen.GenerateMatrix(this.data));
        }


        //todo duplicitni kod
        private List<FileInfo> AnalyzeDir(DirectoryInfo dir)
        {
            List<FileInfo> files = new List<FileInfo>();

            foreach (FileInfo file in dir.EnumerateFiles())
            {
                if (allowedExtensions.Contains(file.Extension))
                {
                    files.Add(file);
                }
            }

            foreach (DirectoryInfo dirInfo in dir.EnumerateDirectories())
            {
                files.AddRange(this.AnalyzeDir(dirInfo));
            }

            return files;
        }

    }
}
