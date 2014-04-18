using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools.ErrorModel
{
    public abstract class MatrixGenerator
    {

        protected List<char> alphabet;

        public MatrixGenerator(List<char> alphabet)
        {
            this.alphabet = alphabet;
            this.alphabet.Sort();
        }

        protected abstract List<ErrorType> GenerateErrors(string correctWord, string wrongWord);

        public Dictionary<char, Dictionary<char, int>> GenerateMatrix(Dictionary<string, List<string>> data)
        {
            Dictionary<char, Dictionary<char, int>> matrix = this.InitMatrix();

            foreach (var pair in data)
            {
                foreach (string mistake in pair.Value)
                {
                    List<ErrorType> errors = this.GenerateErrors(pair.Key, mistake);
                    foreach (ErrorType error in errors)
                    {
                        matrix[error.X][error.Y]++;
                    }
                }
            }

            return matrix;
        }

        private Dictionary<char, Dictionary<char, int>> InitMatrix()
        {
            Dictionary<char, Dictionary<char, int>> matrix = new Dictionary<char, Dictionary<char, int>>();

            foreach (char chr in alphabet)
            {
                Dictionary<char, int> line = new Dictionary<char, int>();
                foreach (char chr2 in alphabet)
                {
                    line.Add(chr2, 0);
                }
                matrix.Add(chr, line);
            }

            return matrix;
        }

    }

    public class ErrorType
    {

        public char X { get; set; }
        public char Y { get; set; }

    }
}
