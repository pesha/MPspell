using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools.ErrorModel
{
    public class TranspositionsMatrixGenerator : MatrixGenerator
    {

        public TranspositionsMatrixGenerator(char[] alphabet) : base(alphabet.ToList()) { }

        protected override List<ErrorType> GenerateErrors(string correctWord, string wrongWord)
        {
            List<ErrorType> errors = new List<ErrorType>();

            for (int i = 0; i < correctWord.Length - 1; i++)
            {
                string newString = String.Copy(correctWord);
                string charItem = newString[i].ToString();
                string edited = newString.Remove(i, 1).Insert(i + 1, charItem);
                if (edited == wrongWord)
                {
                    ErrorType error = new ErrorType();
                    error.Y = correctWord[i + 1];
                    error.X = correctWord[i];

                    errors.Add(error);
                }
            }

            return errors;
        }
    }
}
