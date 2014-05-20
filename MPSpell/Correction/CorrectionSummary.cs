using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public class CorrectionSummary
    {

        public string FileAll { get; private set; }
        public string FileCorrected { get; private set; }
        public string FileSummary { get; private set; }

        public int Corrected { get; private set; }
        public int Detected { get; private set; }

        public CorrectionSummary(string fileAll, string fileCorrected, string fileSummary, string folder = "")
        {
            if (!String.IsNullOrEmpty(folder))
            {
                folder += "/";
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            FileAll = folder + fileAll;
            FileCorrected = folder + fileCorrected;
            FileSummary = folder + fileSummary;

            Corrected = 0;
            Detected = 0;
        }

        public void MergeStats(List<CorrectionStatitic> statistics)
        {
            StreamWriter writerAll = new StreamWriter(System.IO.File.OpenWrite(FileAll), Encoding.UTF8);
            StreamWriter writerCorrected = new StreamWriter(System.IO.File.OpenWrite(FileCorrected), Encoding.UTF8);

            foreach (CorrectionStatitic item in statistics)
            {
                using (StreamReader reader = EncodingDetector.GetStreamWithEncoding(item.File))
                {
                    string line;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        writerAll.WriteLine(line);
                        Detected++;
                    }
                }

                using (StreamReader reader = EncodingDetector.GetStreamWithEncoding(item.FileCorrected))
                {
                    string line;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        writerCorrected.WriteLine(line);
                        Corrected++;
                    }
                }
            }

            writerAll.Dispose();
            writerCorrected.Dispose();

            this.GenerateSummary();
        }

        public void GenerateSummary()
        {
            Dictionary<string, SummaryItem> occurences = new Dictionary<string, SummaryItem>();

            char[] separator = new char[] { ';' };

            using (StreamReader reader = EncodingDetector.GetStreamWithEncoding(this.FileAll))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(separator);

                    if (occurences.ContainsKey(parts[0]))
                    {
                        occurences[parts[0]].Occurences += 1;
                        if (!String.IsNullOrEmpty(parts[1]))
                        {
                            if (!occurences[parts[0]].Corrections.Contains(parts[1]))
                            {
                                occurences[parts[0]].Corrections.Add(parts[1]);
                            }
                        }
                    }
                    else
                    {
                        SummaryItem item = String.IsNullOrEmpty(parts[1]) ? new SummaryItem(1) : new SummaryItem(1, parts[1]);
                        occurences.Add(parts[0], item);
                    }
                }
            }

            var sortedDict = from entry in occurences orderby entry.Value.Occurences descending select entry;

            using (StreamWriter writer = new StreamWriter(System.IO.File.OpenWrite(this.FileSummary), Encoding.UTF8))
            {
                foreach (var pair in sortedDict)
                {
                    writer.WriteLine(pair.Key + ";" + pair.Value.Occurences + ";" + pair.Value.GetCorrectionsAsString());
                }
            }

        }

        public static string GetResultFolder()
        {
            DateTime time = DateTime.Now;
            string minute = time.Minute < 10 ? "0" + time.Minute.ToString() : time.Minute.ToString();

            return time.Day + "-" + time.Month + "-" + time.Year + " " + time.Hour + "-" + minute;
        }

        class SummaryItem
        {

            public long Occurences { get; set; }
            public List<string> Corrections = new List<string>();

            public SummaryItem(int occurence, string correction = null)
            {
                Occurences = 0 + occurence;

                if (null != correction)
                {
                    Corrections.Add(correction);
                }
            }

            public string GetCorrectionsAsString()
            {
                string res = "";
                foreach (string item in Corrections)
                {
                    res += item + ",";
                }

                return res.Length > 0 ? res.Substring(0, res.Length - 1) : "";
            }

        }

    }
}
