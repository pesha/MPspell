using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    public class MisspelledWord
    {

        private int errorPosition;
        private List<Token> context;
        public string WrongWord
        {
            get
            {
                return this.context[this.errorPosition].Word;
            }
        }

        public MisspelledWord(IEnumerable<Token> collection, int errorPos)
        {
            context = new List<Token>(collection);
            errorPosition = errorPos;
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
                if (this.context[i].ContextEnd)
                {
                    leftContext = new List<string>();                    
                }

                leftContext.Add(this.context[i].Word);
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

            for (int i = this.errorPosition; i < end; i++)
            {
                rightContext.Add(this.context[i].Word);
                if (this.context[i].ContextEnd)
                {
                    break;
                }                
            }

            return rightContext;
        }


    }
}
