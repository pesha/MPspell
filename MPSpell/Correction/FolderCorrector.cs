﻿using MPSpell.Dictionaries;
using MPSpell.Extensions;
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

        public List<FileInfo> FilesToProcess { get; private set; }

        private string directory;
        private string resultDirectory;
        private List<string> allowedExtensions = new List<string>() { ".txt", "" };        
        private Dictionary dictionary;
        private ILanguageModel languageModel;
        private IAccentModel accentModel;
        private IErrorModel errorModel;

        public FolderCorrector(Dictionary dictionary, string directory, string resultDirectory = null)
        {
            this.directory = directory;
            this.dictionary = dictionary;
            this.languageModel = new LanguageModel(dictionary);
            this.errorModel = new ErrorModel(dictionary);
            this.accentModel = new AccentModel(dictionary);
            this.FilesToProcess = this.AnalyzeDir(new DirectoryInfo(directory));

            this.resultDirectory = null != resultDirectory ? resultDirectory : directory;
        }

        public FolderCorrector(Dictionary dictionary, List<string> files)
        {
            this.dictionary = dictionary;            
        }

        public FolderAnalyzeResult GetFolderAnalyzeResult()
        {
            long size = 0;
            foreach (FileInfo file in FilesToProcess)
            {
                size += file.Length;
            }

            return new FolderAnalyzeResult(FilesToProcess.Count, size);
        }





        public long CorrectFiles()
        {            
            dictionary.PreloadDictionaries();
            Stopwatch time = Stopwatch.StartNew();
            Corrector corrector = new Corrector(errorModel, languageModel, accentModel, true);
            CorrectionStatitic stats = new CorrectionStatitic("stats.txt", "statscorrected.txt");
      
            foreach (FileInfo file in FilesToProcess)            
            {
                FileHandler handler = new FileHandler(file.FullName, this.resultDirectory + "/" + file.Name); 

                using (FileChecker checker = new FileChecker(file.FullName, dictionary))
                {
                    while (!checker.EndOfCheck)
                    {
                        MisspelledWord error = checker.GetNextMisspelling();
                        if (null != error)
                        {                            
                            corrector.Correct(error);
                            stats.AddCorrection(error);

                            if (error.CorrectWord != null)
                            {
                                handler.Push(error);    
                            }
                        }
                    }
                }

                handler.Close();           
            }
            time.Stop();
            Debug.WriteLine("Elapsed time: " + time.ElapsedMilliseconds + " ms");

            return time.ElapsedMilliseconds;
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

    public class FolderAnalyzeResult
    {

        public int FileCount { get; private set; }
        public long FileSize { get; private set; }

        public FolderAnalyzeResult(int count, long size)
        {
            FileCount = count;
            FileSize = size;
        }

        // todo resit helperem na verejne casti
        public string GetSizeInMb()
        {
            return Math.Round((double) FileSize / 1024 / 1024, 2).ToString() + "MB";
        }

    }


}
