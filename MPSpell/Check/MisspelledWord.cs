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
        private List<WindowItem> context;

        public MisspelledWord(IEnumerable<WindowItem> collection, int errorPos)
        {
            context = new List<WindowItem>(collection);
            errorPosition = errorPos;
        }

        public WindowItem GetWrongWindowItem()
        {
            return this.context[this.errorPosition];
        }

        public string GetWrongWord()
        {
            return this.context[this.errorPosition].Word;
        }

        public List<string> GetLeftContext(int limit = 3)
        {
            List<string> leftContext = new List<string>();

            int start = this.errorPosition - limit;
            if (start < 0)
            {
                start = 0;
            }
            for (int i = start; i <= this.errorPosition; i++)
            {
                if (this.context[i].SentenceEnd)
                {
                    leftContext = new List<string>();
                }

                leftContext.Add(this.context[i].Word);                                    
            }

            return leftContext;
        }

        public List<string> GetRightContext(int limit = 3)
        {
            List<string> rightContext = new List<string>();

            int end = this.errorPosition + limit;
            if (this.context.Count < end)
            {
                end = rightContext.Count;
            }

            for (int i = this.errorPosition; i < end; i++)
            {
                if (this.context[i].SentenceEnd)
                {
                    break;
                }

                rightContext.Add(this.context[i].Word);
            }

            return rightContext;
        }


    }
}
