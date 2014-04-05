using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public class TwoCharFrequencyCounter : BaseCounter<string>, ICorporaOutputHandler
    {

        private bool forceLower;
        private List<string> tokens;
        private string prevChar;

        public TwoCharFrequencyCounter(string[] alphabet = null, bool forceLower = true)
        {
            this.forceLower = forceLower;            
            this.tokens = (null != alphabet) ? this.GenerateTokens(alphabet) : null;            
        }

        public new void HandleOutput(string str)
        {
            if (forceLower)
            {
                str = str.ToLowerInvariant();
            }

            string res = prevChar + str;
            prevChar = str;
            if (null != tokens && !tokens.Contains(res))
            {
                return;
            }

            base.HandleOutput(res);                      
        }

        private List<string> GenerateTokens(string[] alphabet)
        {
            List<string> tokens = new List<string>();

            foreach (string item in alphabet)
            {
                foreach (string secItem in alphabet)
                {                    
                    tokens.Add(item + secItem);
                }
            }

            return tokens;
        }



    }
}
