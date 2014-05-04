using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public class HCLineParser
    {

        private static readonly char[] separator = new char[] { '	' };

        public string ParseLine(string line)
        {
            string[] lines = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return lines.Length > 0 ? lines.Last() + " " : "";
        }

    }
}
