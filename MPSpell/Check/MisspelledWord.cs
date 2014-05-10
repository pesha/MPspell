using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MPSpell.Check
{

    public enum CorrectedBy
    {
        NotCorrected,
        AccentModel,
        ErrorModel,
        AccentAndLanguageModel,
        ErrorAndLanguageModel,
    }

    public class MisspelledWord
    {

        private int errorPosition;
        private List<Token> context;
        public string CorrectWord { get; set; }
        public double Accuracy { get; set; }
        public bool RevokedByLm { get; set; }
        public CorrectedBy CorrectedBy { get; set; }
        public string WrongWord
        {
            get
            {
                return this.context[errorPosition].Word;
            }
        }
        public string RawWord
        {
            get
            {
                return WordContext[0] + WordContext[1] + WordContext[2];
            }
        }        
        public string CorrectWordWithContext
        {
            get
            {
                string word = this.IsName() ? char.ToUpper(CorrectWord[0]) + CorrectWord.Substring(1) : CorrectWord;
                return WordContext[0] + word + WordContext[2];
            }
        }
        public string[] WordContext
        {
            get
            {
                return this.context[errorPosition].WordContext;
            }
        }


        public MisspelledWord(IEnumerable<Token> collection, int errorPos)
        {
            context = new List<Token>(collection);
            errorPosition = errorPos;
            
            RevokedByLm = false;
            CorrectedBy = Check.CorrectedBy.NotCorrected;
        }
       

        public int GetPosition()
        {
            return (int) this.context[errorPosition].Position;
        }

        public bool IsName()
        {
            return (this.WordContext[1][0] == char.ToUpperInvariant(this.WordContext[1][0])) ? true : false;
        }

        public Token GetWrongWindowItem()
        {
            return this.context[this.errorPosition];
        }

        public List<string> GetLeftContext(int limit = 2)
        {
            List<string> leftContext = new List<string>();

            int start = this.errorPosition - limit;
            if (start < 0)
            {
                start = 0;
            }
            for (int i = start; i <= this.errorPosition; i++)
            {
                leftContext.Add(this.context[i].Word);
                
                if (this.context[i].ContextEnd && i != this.errorPosition)
                {
                    leftContext = new List<string>();
                }
            }

            return leftContext;
        }

        public List<string> GetRightContext(int limit = 2)
        {
            List<string> rightContext = new List<string>();

            int end = this.errorPosition + limit;
            if (this.context.Count < (end + 1))
            {
                end = rightContext.Count;
            }

            for (int i = this.errorPosition; i <= end; i++)
            {                
                rightContext.Add(this.context[i].Word);
                
                if (this.context[i].ContextEnd)
                {
                    break;
                }
            }

            return rightContext;
        }

        public bool AreNeighborsInContext()
        {
            if (this.context[1].ContextEnd == true && this.context[2].ContextEnd == true)
            {
                return false;
            }

            return true;
        }


    }
}
