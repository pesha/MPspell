using MPSpell.Dictionaries;
using MPSpell.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{

    public class Window
    {

        public int ContextSize { get; private set; }
        private int windowSize;
        private Queue<Token> history = new Queue<Token>();
        BitArray errorPositions;
        
        public Window(int contextSize = 2)
        {
            this.ContextSize = contextSize;
            this.windowSize = contextSize * 2 + 1;
            this.errorPositions = new BitArray(this.windowSize);
            for (int i = 0; i < this.windowSize; i++)
            {
                history.Enqueue(new Token('_', true));
            }
        }

        public void Add(Token token)
        {
            if (null != token.Position)
            {
                this.errorPositions.ShiftLeft(true);
            }
            else
            {
                this.errorPositions.ShiftLeft(false);
            }

            history.Enqueue(token);
            if (history.Count > this.windowSize)
            {
                history.Dequeue();
            }            
        }

        public MisspelledWord GetMisspelledWord()
        {
            MisspelledWord word = null;

            if (this.errorPositions[this.ContextSize])
            {
                int errors = 0;
                for (int i = 0; i < this.errorPositions.Count; i++)
                {
                    if (this.errorPositions[i])
                    {
                        errors++;
                    }
                }

                //detekce jineho jazyka
                if (errors >= (this.windowSize - 1))
                {
                    return null;
                }

                // v okolnich slovech je chyba, takze preskocit
                if (this.errorPositions[this.ContextSize - 1] && this.errorPositions[this.ContextSize + 1])
                {
                    return null;
                }

                word = new MisspelledWord(this.history, this.ContextSize);
                if (!word.AreNeighborsInContext())
                {
                    return null;
                }
            }
          
            return word;
        }


    }

}
