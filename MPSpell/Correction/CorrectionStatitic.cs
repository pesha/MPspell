using MPSpell.Extensions;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{

    class CorrectionStatitic
    {

        public string File { get; private set; }
        public string FileCorrected { get; private set; }

        private StreamWriter writer;
        private StreamWriter writerCorrected;

        public CorrectionStatitic(string fileAll = null, string fileCorrected = null)
        {
            this.File = (null != fileAll) ? fileAll : Path.GetTempFileName();
            this.FileCorrected = (null != fileCorrected) ? fileCorrected : Path.GetTempFileName();

            FileStream stream = new FileStream(this.File, FileMode.Create, FileAccess.Write);
            writer = new StreamWriter(stream, Encoding.UTF8);

            FileStream streamCor = new FileStream(this.FileCorrected, FileMode.Create, FileAccess.Write);
            writerCorrected = new StreamWriter(streamCor, Encoding.UTF8);
        }

        public void AddCorrection(MisspelledWord error)
        {
            writer.WriteLine(error.WrongWord + ";" + error.CorrectWord + ";" + error.RevokedByLm.ToString());

            if (!String.IsNullOrEmpty(error.CorrectWord))
            {
                writerCorrected.WriteLine(error.WrongWord + ";" + error.CorrectWord + ";" + error.Accuracy.ToString() + ";" + error.CorrectedBy.ToString() +";" + error.IsName().ToString());
            }
        }

        public void Close()
        {
            writer.Close();
            writerCorrected.Close();
        }

        public void GenerateSummary()
        {
            this.Close();

            Dictionary<string, long> occurences = new Dictionary<string, long>();
            
            char[] separator = new char[] { ';' };            

            using (StreamReader reader = EncodingDetector.GetStreamWithEncoding(this.File))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(separator);

                    if (occurences.ContainsKey(parts[0]))
                    {
                        occurences[parts[0]] += 1;
                    }
                    else
                    {
                        occurences.Add(parts[0], 1);
                    }
                }
            }

            var sortedDict = from entry in occurences orderby entry.Value descending select entry;

            using (StreamWriter writer = new StreamWriter(System.IO.File.OpenWrite("summary.txt"), Encoding.UTF8))
            {
                foreach (var pair in sortedDict)
                {
                    writer.WriteLine(pair.Key + ";" + pair.Value);
                }
            }
            
        }

    }

}
