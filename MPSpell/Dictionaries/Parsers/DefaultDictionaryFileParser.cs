using MPSpell.Dictionaries.Affixes;
using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Parsers
{



    public class DefaultDictionaryFileParser : IDictionaryFileParser
    {

        protected static readonly char[] Whitespace = new char[] { ' ' };

        public DictionaryWithFlags Parse(string file, Encoding encoding = null)
        {
            DictionaryWithFlags dictionary = new DictionaryWithFlags();

            using (StreamReader reader = new StreamReader(file, encoding))
            {
                string line;

                int lineCount;
                while (null != (line = reader.ReadLine()))
                {
                    line = line.Trim();
                    if (int.TryParse(line, out lineCount))
                    {
                        continue;
                    }
                    else
                    {
                        int slashPos = line.IndexOf('/');
                        string word = (slashPos > 0) ? line.Substring(0, slashPos) : line;
                        string flags = (slashPos > 0) ? line.Substring(slashPos + 1) : null;

                        dictionary.Add(new DictionaryItemWithFlags(word, flags));
                    }
                }
            }

            return dictionary;
        }

    }



}
