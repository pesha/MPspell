using MPSpell.Correction;
using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Parsers
{
    public class ConfusionMatrixParser : IConfusionMatrixParser
    {

        public ConfusionMatrix ParseMatrix(string file)
        {
            string operation = file.Substring(0, file.Length - 4);
            EditOperation editOperation;
            switch (operation)
            {
                case "deletions":
                    editOperation = EditOperation.Deletion;
                    break;

                case "insertions":
                    editOperation = EditOperation.Insertion;
                    break;

                case "substitutions":
                    editOperation = EditOperation.Substitution;
                    break;

                case "transpositions":
                    editOperation = EditOperation.Transposition;
                    break;

                default:
                    editOperation = EditOperation.Unknown;
                    break;

            }


            ConfusionMatrix matrix = new ConfusionMatrix(editOperation);

            Encoding enc = EncodingDetector.DetectEncoding(file);
            string[] lines = System.IO.File.ReadAllLines(file, enc);

            char[] delimiter = new char[] { ',' };
            string header = lines.First().Replace("#:", "");
            string[] keys = header.Split(delimiter);
            foreach (string line in lines)
            {
                if (line.StartsWith("#"))
                    continue;

                string[] parts = line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                string key = parts.First();
                string[] values = parts.Last().Split(delimiter);

                for (int i = 0; i < values.Length; i++)
                {
                    matrix.SetValue(key[0], keys[i][0], int.Parse(values[i]));
                }

            }

            return matrix;
        }

    }
}
