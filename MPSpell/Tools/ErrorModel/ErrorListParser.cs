using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MPSpell.Tools.ErrorModel
{
    public class ErrorListParser
    {

        private Regex stringAndCount = new Regex(@"(\w+)[*]*(\d)*", RegexOptions.IgnoreCase | RegexOptions.Compiled);        
        private string file;

        public ErrorListParser(string file)
        {
            this.file = file;
        }

        public Dictionary<string, List<string>> Parse()
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            Encoding enc = EncodingDetector.DetectEncoding(file);
            using (StreamReader reader = new StreamReader(file, enc))
            {
                while (!reader.EndOfStream)
                {
                    var pair = this.ParseLine(reader.ReadLine());
                    result.Add(pair.Key, pair.Value);
                }
            }

            return result;
        }

        private KeyValuePair<string, List<string>> ParseLine(string line)
        {
            string[] parts = line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> mistakeList = new List<string>();
            string[] mistakes = parts[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string mistake in mistakes)
            {
                GroupCollection res = stringAndCount.Match(mistake).Groups;
                if (res[2].Success)
                {
                    for (int i = 0; i < int.Parse(res[2].Value); i++)
                    {
                        mistakeList.Add(res[1].Value);
                    }
                }
                else
                {
                    mistakeList.Add(res[1].Value);
                }
            }

            return new KeyValuePair<string, List<string>>(parts[0].Trim(), mistakeList);
        }

    }
}
