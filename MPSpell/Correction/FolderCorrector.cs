using MPSpell.Dictionaries;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public class FolderCorrector
    {

        private string directory;
        private List<string> allowedExtensions = new List<string>() { "txt", "" };        
        private List<FileInfo> filesToProcess;
        private Dictionary dictionary;
        private ILanguageModel languageModel;
        private IErrorModel errorModel;

        public FolderCorrector(Dictionary dictionary, string directory)
        {
            this.directory = directory;
            this.dictionary = dictionary;
            this.languageModel = new LanguageModel(dictionary);
            this.errorModel = new ErrorModel(dictionary);            
            this.filesToProcess = this.AnalyzeDir(new DirectoryInfo(directory));
        }

        public FolderCorrector(Dictionary dictionary, List<string> files)
        {
            this.dictionary = dictionary;
            
        }

        public void CorrectFiles()
        {            
            dictionary.PreloadDictionaries();
            Stopwatch time = Stopwatch.StartNew();
            Corrector corrector = new Corrector(errorModel, languageModel);

            foreach (FileInfo file in filesToProcess)            
            {
                List<MisspelledWord> errors = new List<MisspelledWord>();
                using (FileChecker checker = new FileChecker(file.FullName, dictionary))
                {
                    while (!checker.EndOfCheck)
                    {
                        MisspelledWord error = checker.GetNextMisspelling();
                        if (null != error)
                        {
                            errors.Add(error);
                        }
                    }
                }

                foreach (MisspelledWord error in errors)
                {
                    corrector.Correct(error);
                }

                FileCorrectionHandler handler = new FileCorrectionHandler(file.FullName, errors);
                handler.SaveCorrectedAs("20_newsgroups_cor/" + file.Name);                
            }
            time.Stop();
            Debug.WriteLine("Elapsed time: " + time.ElapsedMilliseconds + " ms");
        }

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
