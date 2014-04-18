using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools.ErrorModel
{
    public class MatrixExport
    {

        public static void ExportMatrix(string file, Dictionary<char, Dictionary<char, int>> matrix)
        {
            using (FileStream fStream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(fStream, Encoding.UTF8);

                string line = "#:";
                foreach (var pair in matrix)
                {
                    line += pair.Key + ",";
                }

                line = line.Substring(0, line.Length - 1);
                writer.WriteLine(line);

                foreach (var pair in matrix)
                {
                    line = pair.Key + ":";
                    foreach (var item in pair.Value)
                    {
                        line += item.Value + ",";                        
                    }
                    line = line.Substring(0, line.Length - 1);
                    writer.WriteLine(line);
                }
                writer.Close();
            }
        }

    }
}
