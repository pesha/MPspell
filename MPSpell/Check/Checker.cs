using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    public abstract class Checker
    {
                
        protected Tokenizer tokenizer;

        public Checker(Dictionary dictionary, int contextSize)
        {            
            this.tokenizer = new Tokenizer(dictionary);            
        }

        public abstract MisspelledWord GetNextMisspelling();        

    }
}
