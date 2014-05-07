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
    public class FileHandler
    {

        private string sourcePath;
        private string destinationPath;
        private Encoding outEncoding;

        private StreamReader reader;
        private StreamWriter writer;

        private int pos;
        private int maxCharsInBatch;

        public FileHandler(string sourcePath, string destinationPath = null, Encoding encoding = null)
        {
            this.sourcePath = sourcePath;
            this.destinationPath = (null != destinationPath) ? destinationPath : sourcePath + ".tmp"; 
            this.outEncoding = (null != encoding) ? encoding : Encoding.UTF8;

            reader = EncodingDetector.GetStreamWithEncoding(this.sourcePath);

            FileStream fStream = new FileStream(this.destinationPath, FileMode.Create, FileAccess.Write);
            writer = new StreamWriter(fStream, this.outEncoding);
            
            pos = 0;
            maxCharsInBatch = 10000;
        }


        public void Write(MisspelledWord error = null)
        {
            int batch = 0;
            string data = "";
            while (!reader.EndOfStream)
            {
                data += (char)reader.Read();
                pos++;
                batch++;

                if (error != null && pos == error.GetPosition())
                {
                    data += error.CorrectWordWithContext;
                    for (int i = 0; i < error.WrongWord.Length; i++)
                    {
                        pos++;
                        reader.Read();
                    }

                    break;
                }

                if (batch > maxCharsInBatch)
                {
                    writer.Write(data);
                    data = "";
                }
            }

            writer.Write(data);
        }

        public void Push(MisspelledWord error)
        {
            Write(error);
        }

        public void Close()
        {
            this.Write();

            reader.Close();
            writer.Close();
        }


        public void CopyFile()
        {
            int limit = 10;
            int chrs = 0;
            string batch = "";
            while (!reader.EndOfStream)
            {
                batch += (char)reader.Read();
                chrs++;

                if (chrs > limit)
                {
                    writer.Write(batch);
                    batch = "";
                    chrs = 0;
                }
            }

            reader.Close();
            writer.Close();
        }






    }
}
