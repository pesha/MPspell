using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Parsers
{
    public class NgramParser : FileParser
    {

        public static readonly char[] separator = new char[] { '	' };
        private int NgramCount;


        public NgramCollection ParseNgrams(string file)
        {
            NgramCollection collection = new NgramCollection();
            NgramCount = 0;

            IEnumerable<string> sequence = base.ParseFile(file);            
            using (var handler = sequence.GetEnumerator())
            {
                while (handler.MoveNext())
                {
                    collection.Add(this.ParseLine(handler.Current));
                }
            }

            collection.NgramCount = NgramCount;

            return collection;
        }

        private Ngram ParseLine(string line)
        {
            string[] parts = line.Split(separator,2);

            int count = int.Parse(parts[0]);
            NgramCount += count;

            return new Ngram(parts[1].Split(separator), count);
        }


    }
}
