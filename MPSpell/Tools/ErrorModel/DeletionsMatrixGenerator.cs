using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools.ErrorModel
{
    public class DeletionsMatrixGenerator : MatrixGenerator
    {

        public DeletionsMatrixGenerator(char[] alphabet) : base(alphabet.ToList()) { }

        protected override List<ErrorType> GenerateErrors(string correctWord, string wrongWord)
        {
            List<ErrorType> errors = new List<ErrorType>();

            for (int i = 0; i < correctWord.Length; i++)
            {
                string edited = String.Copy(correctWord).Remove(i, 1);
                if(edited == wrongWord){                    
                    ErrorType error = new ErrorType();
                    error.Y = correctWord[i];
                    error.X = (i - 1) < 0 ? ' ' : correctWord[i - 1];

                    errors.Add(error);
                }
            }

            return errors;
        }
    }
}
