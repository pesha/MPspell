using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools.ErrorModel
{
    public class InsertionsMatrixGenerator : MatrixGenerator
    {

        public InsertionsMatrixGenerator(char[] alphabet) : base(alphabet.ToList()) {}

        protected override List<ErrorType> GenerateErrors(string correctWord, string wrongWord)
        {
            List<ErrorType> errors = new List<ErrorType>();
            
            for (int i = 0; i <= correctWord.Length; i++)
            {
                foreach (char item in alphabet)
                {
                    string edited = String.Copy(correctWord).Insert(i, item.ToString());
                    if (edited == wrongWord)
                    {
                        ErrorType error = new ErrorType();
                        error.Y = item;
                        error.X = (i - 1 >= 0) ? correctWord[i - 1] : ' ';

                        errors.Add(error);
                    }
                }
            }

            return errors;
        }

    }
}
