using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public class DictionaryWriter
    {

        public static void Write(string file, Dictionary dictionary)
        {
            using (FileStream fStream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fStream, Encoding.UTF8))
                {
                    foreach (string word in dictionary)
                    {
                        writer.WriteLine(word);
                    }
                    writer.Close();
                }
            }
        }

    }

}
