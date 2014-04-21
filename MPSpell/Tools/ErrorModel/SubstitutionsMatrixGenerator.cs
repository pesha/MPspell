using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools.ErrorModel
{
    public class SubstitutionsMatrixGenerator : MatrixGenerator
    {

        public SubstitutionsMatrixGenerator(char[] alphabet, int initValue = 0) : base(alphabet.ToList(), initValue) { }

        protected override List<ErrorType> GenerateErrors(string correctWord, string wrongWord)
        {
            List<ErrorType> errors = new List<ErrorType>();

            for (int i = 0; i < correctWord.Length; i++)
            {
                foreach (char charItem in alphabet)
                {
                    string edited = String.Copy(correctWord).Remove(i, 1).Insert(i, charItem.ToString());
                    if (edited == wrongWord)
                    {
                        ErrorType error = new ErrorType();
                        error.X = charItem;
                        error.Y = correctWord[i];

                        errors.Add(error);
                    }
                }
            }

            return errors;
        }
    }
}
