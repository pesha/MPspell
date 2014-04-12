using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    public class StringChecker : Checker, IDisposable
    {

        private StringReader reader;
        private int contextLeft;

        public StringChecker(string text, Dictionary dictionary, int contextSize = 2)
            : base(dictionary, contextSize)
        {
            reader = new StringReader(text);
            contextLeft = contextSize;
        }

        public override MisspelledWord GetNextMisspelling()
        {
            int chr;
            MisspelledWord misspelling = null;
            while ((chr = reader.Read()) > 0)
            {             
                misspelling = this.tokenizer.HandleChar((char)chr);
                if (null != misspelling)
                {
                    break;
                }
            }

            if(null == misspelling && contextLeft > 0){
                for (int i = contextLeft; i > 0; i--)
                {
                    contextLeft--;
                    misspelling = this.tokenizer.HandleChar('.', true);
                    if (null != misspelling)
                    {
                        break;
                    }
                }
            }
                        
            return misspelling;            
        }

        public void Dispose()
        {
            reader.Dispose();                     
        }
    }
}
