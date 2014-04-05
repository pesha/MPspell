using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Parsers
{
    public class FrequencyVectorParser
    {


        private static readonly char[] separator = new char[] { ',' };


        public FrequencyVector<string> ParseFrequency(string file)
        {
            FrequencyVector<string> frequences = new FrequencyVector<string>();

            Encoding encoding = EncodingDetector.DetectEncoding(file);
            using (StreamReader reader = new StreamReader(file, encoding))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    frequences.Add(parts[0].ToString(), int.Parse(parts[1]));
                }
            }

            return frequences;
        }




    }
}
