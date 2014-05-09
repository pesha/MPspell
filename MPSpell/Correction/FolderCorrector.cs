using MPSpell.Dictionaries;
using MPSpell.Extensions;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public class FolderCorrector
    {

        public List<FileInfo> FilesToProcess { get; private set; }
        public long CorrectionTime { get; private set; }
        public bool PreserveSubfolders { get; set; }        

        private string directory;
        private string resultDirectory;
        private List<string> allowedExtensions = new List<string>() { ".txt", "" };
        private Dictionary dictionary;
        private ILanguageModel languageModel;
        private IAccentModel accentModel;
        private IErrorModel errorModel;

        private Corrector corrector;

        // temporary info
        private int totalFiles;
        private int processedFiles;
        private BackgroundWorker worker;

        public FolderCorrector(Dictionary dictionary, string directory, string resultDirectory = null)
        {
            this.directory = directory;
            this.dictionary = dictionary;

            // setup models
            this.languageModel = new LanguageModel(dictionary);
            this.errorModel = new ErrorModel(dictionary);
            this.accentModel = dictionary.IsAccentModelAvailable() ? new AccentModel(dictionary) : null;

            // setup corrector
            this.corrector = new Corrector(errorModel, languageModel, accentModel, true);

            // prepare files and folders
            this.FilesToProcess = this.AnalyzeDir(new DirectoryInfo(directory));
            this.resultDirectory = null != resultDirectory ? resultDirectory : directory;

            // other settings
            PreserveSubfolders = true;
        }

        public void RunCorrection(BackgroundWorker worker = null)
        {
            this.worker = worker;

            dictionary.PreloadDictionaries();
            this.totalFiles = this.FilesToProcess.Count;
            this.processedFiles = 0;
            Stopwatch time = Stopwatch.StartNew();

            List<FileInfo>[] filesGroups = this.DivadeIntoGroups(2);

            Task<CorrectionStatitic>[] tasks = new Task<CorrectionStatitic>[filesGroups.Length];
            int id = 0;
            foreach (List<FileInfo> group in filesGroups)
            {
                if (group.Count > 0)
                {
                    Task<CorrectionStatitic> task = Task<CorrectionStatitic>.Factory.StartNew(() =>
                    {
                        return this.CorrectGroup(group, id);
                    });

                    tasks[id++] = task;
                }                
            }

            Task.WaitAll(tasks);            

            this.CorrectionTime = time.ElapsedMilliseconds;
        }

        private CorrectionStatitic CorrectGroup(List<FileInfo> group, int id)
        {
            CorrectionStatitic stats = new CorrectionStatitic();

            foreach (FileInfo file in group)
            {
                string output = PreserveSubfolders ? this.GetSubfolder(file) : this.resultDirectory + "/" + file.Name;

                FileHandler handler = new FileHandler(file.FullName, output);

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

                this.UpdateProgres(1);
            }

            stats.Close();
            return stats;
        }

        private void UpdateProgres(int files)
        {
            lock ("progress")
            {
                processedFiles += files;
                worker.ReportProgress((this.processedFiles * 100)/this.totalFiles);
            }
        }

        private List<FileInfo>[] DivadeIntoGroups(int groupCount)
        {
            List<FileInfo>[] groups = new List<FileInfo>[groupCount];

            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = new List<FileInfo>();
            }

            int current = 0;
            foreach (FileInfo file in this.FilesToProcess)
            {
                groups[current].Add(file);
                current++;
                if (current >= groups.Length)
                {
                    current = 0;
                }
            }

            return groups;
        }

        private string GetSubfolder(FileInfo info)
        {
            string path = info.FullName;
            path = path.Replace(this.directory, "");

            return this.resultDirectory + path;
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

        public FolderAnalyzeResult GetFolderAnalyzeResult()
        {
            long size = 0;
            foreach (FileInfo file in FilesToProcess)
            {
                size += file.Length;
            }

            return new FolderAnalyzeResult(FilesToProcess.Count, size);
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
            return Math.Round((double)FileSize / 1024 / 1024, 2).ToString() + "MB";
        }

    }


}
