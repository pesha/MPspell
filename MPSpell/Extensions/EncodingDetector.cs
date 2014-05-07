using href.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Extensions
{
    class EncodingDetector
    {

        public static StreamReader GetStreamWithEncoding(string file)
        {
            StreamReader reader = null;

            Encoding enc = TextFileEncodingDetector.DetectTextFileEncoding(file);
            if (null != enc)
            {
                reader = new StreamReader(file, enc);
            }
            else
            {
                //reader = EncodingTools.OpenTextFile(file);
                FileStream stream = File.OpenRead(file);
                reader = EncodingTools.OpenTextStream(stream);
            }

            return reader;
        }

        public static Encoding DetectEncoding(string file)
        {
            Encoding encoding = null;

            encoding = TextFileEncodingDetector.DetectTextFileEncoding(file);
            if (null == encoding)
            {
                string content = EncodingTools.ReadTextFile(file);
                encoding = EncodingTools.GetMostEfficientEncoding(content);
            }

            return encoding;
        }


    }
}
