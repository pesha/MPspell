using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public class CharFrequencyCounter : BaseCounter<string>, ICorporaOutputHandler
    {

        private bool forceLower;
        private string[] alphabet;

        public CharFrequencyCounter(string[] alphabet = null, bool forceLower = true)
        {
            this.forceLower = forceLower;
            this.alphabet = alphabet;
        }
        
        public new void HandleOutput(string str)
        {
            if (forceLower)
            {
                str = str.ToLowerInvariant();
            }

            if (null != alphabet && !alphabet.Contains(str))
            {
                return;
            }

            base.HandleOutput(str);
        }


    }
}
