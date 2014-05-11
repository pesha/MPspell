using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public class SimpleFileLoader
    {


        public static List<string> Load(string file)
        {
            List<string> lines = new List<string>();

            using (StreamReader reader = EncodingDetector.GetStreamWithEncoding(file))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            return lines;
        }

    }
}
