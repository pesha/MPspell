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
    public class FileCorrectionHandler
    {

        private string path;
        private List<MisspelledWord> misspellings;
        private Encoding outEncoding;

        public FileCorrectionHandler(string path, List<MisspelledWord> misspellings, Encoding outputEncoding = null)
        {
            this.path = path;
            this.misspellings = misspellings;
            this.outEncoding = (null != outputEncoding) ? outputEncoding : Encoding.UTF8;
        }

        public void OverwriteWithCorrections()
        {
            this.ProccesFile(this.path, true);
        }

        public void SaveCorrectedAs(string file)
        {
            this.ProccesFile(file);
        }

        private void ProccesFile(string file, bool overwrite = false)
        {
            Encoding encoding = EncodingDetector.DetectEncoding(this.path);
            StreamReader reader = new StreamReader(this.path, encoding);            

            string outFile = overwrite ? "tmp_" + file : file;

            FileStream fStream = new FileStream(outFile, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fStream, this.outEncoding);

            int currentStart = 0;
            int currentEnd = 0;
            string line = string.Empty;
            bool process = false;            
            while (!reader.EndOfStream)
            {
                char chr = (char) reader.Read();                
                int padding = 0;
                if (chr == '\r' || chr == '\n')
                {
                    // todo refactor
                    padding += 1;
                    if (chr == '\r' && ((char)reader.Peek()) == '\n')
                    {
                        reader.Read();
                        padding += 1;
                    }
                    process = true;
                }
                else
                {
                    line += chr;
                }

                if (reader.EndOfStream)
                {
                    process = true;
                }

                if (process)
                {
                    currentEnd = currentStart + line.Length + padding;
                    foreach (MisspelledWord word in misspellings)
                    {
                        if (currentStart < word.GetPosition() && word.GetPosition() < currentEnd && word.CorrectWord != null)
                        {
                            bool added = false;
                            int a = word.GetPosition() - currentStart;
                            int b = a + word.RawWord.Length;

                            string newLine = "";
                            for (int i = 0; i < line.Length; i++)
                            {
                                if (i < a || i >= b)
                                {
                                    newLine += line[i];
                                }
                                else if (!added)
                                {
                                    newLine += word.CorrectWordWithContext;
                                    added = true;
                                }
                            }

                            line = newLine;
                        }
                    }

                    if (!reader.EndOfStream)
                    {
                        writer.WriteLine(line);
                    }
                    else
                    {
                        writer.Write(line);
                    }

                    line = String.Empty;
                    currentStart = currentEnd;
                    process = false;
                }
            }

            reader.Close();
            reader.Dispose();

            writer.Close();
            writer.Dispose();

            if (overwrite)
            {
                System.IO.File.Delete(file);
                System.IO.File.Move(outFile, file);
            }
        }


    }
}
