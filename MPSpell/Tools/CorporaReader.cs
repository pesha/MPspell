using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public class CorporaReader
    {

        private HCLineParser parser;
        private ICorporaOutputHandler handler;

        public CorporaReader(HCLineParser parser, ICorporaOutputHandler handler)
        {
            this.parser = parser;
            this.handler = handler;
        }

        public void ProcessFile(string file)
        {
            Encoding encoding = EncodingDetector.DetectEncoding(file);

            using (StreamReader reader = new StreamReader(file, encoding))
            {
                string line;

                while (null != (line = reader.ReadLine()))
                {
                    line = this.parser.ParseLine(line);
                    
                    for(int i = 0; i < line.Length; i++){
                        handler.HandleOutput(line[i].ToString());
                    }
                }

            }

        }      

    }
}
