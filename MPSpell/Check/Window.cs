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
            if (this.errorPositions[this.ContextSize])
            {
                return new MisspelledWord(this.history, this.ContextSize);
            }

            return null;
        }


    }

}
