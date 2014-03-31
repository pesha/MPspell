using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell
{
    static class Utils
    {

        public static Encoding DetectEncoding(string file)
        {
            string line;
            using (StreamReader reader = new StreamReader(file))
            {
                line = reader.ReadLine();
            }

            string[] setEncoding = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Encoding enc = null;
            if (setEncoding.First() == "SET")
            {
                enc = Encoding.GetEncoding(setEncoding.Last());
            }

            return enc;
        }

    }
}
