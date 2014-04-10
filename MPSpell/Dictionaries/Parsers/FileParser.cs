using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Parsers
{

    public abstract class FileParser
    {

        protected IEnumerable<string> ParseFile(string file)
        {
            Encoding encoding = EncodingDetector.DetectEncoding(file);
            using (StreamReader reader = new StreamReader(file, encoding))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {                    
                    yield return line;                    
                }
            }
        }


    }

}
