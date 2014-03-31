using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance
{
    class Program
    {
        static void Main(string[] args)
        {

            string word = "acress";

            var result = DamerauLevensteinEditDistance.GenerateSubtitutions(word);

        }
    }
}
