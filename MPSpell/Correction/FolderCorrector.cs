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
        public int ThreadsAvailable { get; private set; }
        public int ThreadsUsed { get; private set; }
        public int ProcessedFiles { get; private set; }
        public int TotalFiles { get; private set; }

        public bool ExportContext { get; set; }

        public int Detected { get; private set; }
        public int Corrected { get; private set; }


        public string ReportDirectory { get; private set; }
        public string ResultDirectory { get; private set; }
        public string SourceDirectory { get; private set; }
        public bool OnlySelectedFiles { get; private set; }
                     
        private List<string> allowedExtensions = new List<string>() { ".txt", "", ".text" };
        private Dictionary dictionary;
        private ILanguageModel languageModel;
        private IAccentModel accentModel;
        private IErrorModel errorModel;

        private Corrector corrector;

        // temporary info        
        private BackgroundWorker worker;
        private int estimateLimit;
        private List<FileInfo>[] filesGroups;

        public FolderCorrector(Dictionary dictionary, string sourceDirectory, string resultDirectory = null, string reportDirectory = null, bool preserveSubfolders = true)
        {
            this.SourceDirectory = sourceDirectory;
          
            // prepare files and folders
            this.FilesToProcess = this.AnalyzeDir(new DirectoryInfo(sourceDirectory));

            this.PrepareProject(dictionary, resultDirectory, reportDirectory, preserveSubfolders);
        }

        public FolderCorrector(Dictionary dictionary, string[] sourceFiles, string resultDirectory = null, string reportDirectory = null)
        {
            this.OnlySelectedFiles = true;
            this.FilesToProcess = this.GetFileInfo(sourceFiles);

            this.PrepareProject(dictionary, resultDirectory, reportDirectory, false);
        }

        private void PrepareProject(Dictionary dictionary, string resultDirectory, string reportDirectory, bool preserveSubfolders)
        {
            this.ExportContext = false;
            this.ResultDirectory = resultDirectory;
            this.ReportDirectory = reportDirectory;

            this.dictionary = dictionary;

            // setup models
            this.languageModel = new LanguageModel(dictionary);
            this.errorModel = new ErrorModel(dictionary);
            this.accentModel = dictionary.IsAccentModelAvailable() ? new AccentModel(dictionary) : null;

            // setup corrector
            this.corrector = new Corrector(errorModel, languageModel, accentModel);

            this.ThreadsAvailable = this.ScaleThreads();
            this.filesGroups = this.DivadeIntoGroups(this.ThreadsAvailable);
            this.ThreadsUsed = this.FilesToProcess.Count > 1 ? filesGroups.Length : 1;

            // other settings
            PreserveSubfolders = preserveSubfolders;
        }

        public void RunCorrection(BackgroundWorker worker = null)
        {
            this.worker = worker;

            this.worker.ReportProgress(0, new ProgressReport(Report.PreloadingDictionary));
            dictionary.PreloadDictionaries();


            this.TotalFiles = this.FilesToProcess.Count;
            this.ProcessedFiles = 0;
            this.estimateLimit = 10000; // for progress bar
            Stopwatch time = Stopwatch.StartNew();

            this.worker.ReportProgress(0, new ProgressReport(Report.Working));
            Task<CorrectionStatitic>[] tasks = new Task<CorrectionStatitic>[this.ThreadsUsed];
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

            time.Stop();
            this.CorrectionTime = time.ElapsedMilliseconds;
            if (!worker.CancellationPending)
            {
                List<CorrectionStatitic> stats = new List<CorrectionStatitic>();
                foreach (Task<CorrectionStatitic> task in tasks)
                {
                    stats.Add(task.Result);
                }

                this.worker.ReportProgress(100, new ProgressReport(Report.PreparingStatistics));
                CorrectionSummary summary = new CorrectionSummary("all.txt", "corrected.txt", "counts.txt", this.GetReportDirectory());
                summary.MergeStats(stats);

                this.Corrected = summary.Corrected;
                this.Detected = summary.Detected;

                this.worker.ReportProgress(100, new ProgressReport(Report.Done));
            }
            else
            {
                this.worker.ReportProgress(0, new ProgressReport(Report.Canceled));
            }
        }

        private string GetReportDirectory()
        {
            return ReportDirectory + "/" +  CorrectionSummary.GetResultFolder();
        }

        private int ScaleThreads()
        {
            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select NumberOfCores from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }

            return coreCount;
        }

        private CorrectionStatitic CorrectGroup(List<FileInfo> group, int id)
        {
            CorrectionStatitic stats = new CorrectionStatitic(null, null, this.ExportContext);

            foreach (FileInfo file in group)
            {
                string output = PreserveSubfolders ? this.GetSubfolder(file) : this.ResultDirectory + "/" + file.Name;

                if (file.FullName == new FileInfo(output).FullName)
                {
                    output = output + ".1";
                }

                FileHandler handler = new FileHandler(file.FullName, output);

                using (FileChecker checker = new FileChecker(file.FullName, dictionary))
                {
                    Task<List<MisspelledWord>> task = null;
                    List<MisspelledWord> errors = new List<MisspelledWord>();
                    int estimates = 0;
                    while (!checker.EndOfCheck)
                    {
                        MisspelledWord error = checker.GetNextMisspelling();
                        if (null != error)
                        {
                            estimates++;
                            errors.Add(error);                            
                        }

                        if (errors.Count > 1000 || checker.EndOfCheck)
                        {                            
                            if (task != null)
                            {
                                task.Wait();
                                List<MisspelledWord> corrected = task.Result;
                                foreach (MisspelledWord item in corrected)
                                {
                                    stats.AddCorrection(item);
                                    if (item.CorrectWord != null)
                                    {
                                        handler.Push(item);
                                    }
                                }
                            }

                            List<MisspelledWord> errorBatch = errors;                            
                            errors = new List<MisspelledWord>();

                            task = Task<List<MisspelledWord>>.Factory.StartNew(() =>
                            {
                                return this.CorrectErrors(errorBatch);
                            });                            
                        }

                        if (estimates > estimateLimit)
                        {
                            this.UpdateProgres(0, checker.EstimateProcess());
                            estimates = 0;
                        }

                        if (worker.CancellationPending)
                        {                            
                            return null;
                        }
                    }

                    if (null != task)
                    {
                        task.Wait();
                        List<MisspelledWord> leftover = task.Result;
                        foreach (MisspelledWord item in leftover)
                        {
                            stats.AddCorrection(item);
                            if (item.CorrectWord != null)
                            {
                                handler.Push(item);
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

        private List<MisspelledWord> CorrectErrors(List<MisspelledWord> errors)
        {
            foreach (MisspelledWord error in errors)
            {
                corrector.Correct(error);
            }

            return errors;
        }

        private void UpdateProgres(int files, double? file = 0)
        {
            lock ("progress")
            {
                if (files > 0)
                {
                    ProcessedFiles += files;
                }

                worker.ReportProgress((int)((this.ProcessedFiles + file) * 100)/this.TotalFiles);                
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
            path = path.Replace(this.SourceDirectory, "");

            return this.ResultDirectory + path;
        }

        private List<FileInfo> GetFileInfo(string[] filesPath)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (string file in filesPath)
            {
                FileInfo info = new FileInfo(file);
                if (info.Exists)
                {
                    files.Add(info);
                }
            }

            return files;
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
            return Math.Round(((double)FileSize / 1024), 2).ToString() + " kB";
        }

        public string GetAvgSize()
        {
            double count = ((double)FileSize / FileCount) / 1024.0;
            return Math.Round(count, 2).ToString() + " kB";
        }

    }

    public class ProgressReport
    {

        public Report Report { get; private set; }

        public ProgressReport(Report report)
        {
            Report = report;
        }

    }

    public enum Report
    {
        WaitingToStart,
        PreloadingDictionary,
        Working,
        PreparingStatistics,
        Done,
        Canceled
    }




}
