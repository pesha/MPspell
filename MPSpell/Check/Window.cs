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

        private int contextSize;
        private int windowSize;
        private Queue<WindowItem> history = new Queue<WindowItem>();
        BitArray errorPositions;
        
        public Window(int contextSize = 2)
        {
            this.contextSize = contextSize;
            this.windowSize = contextSize * 2 + 1;
            this.errorPositions = new BitArray(this.windowSize);
        }

        public void Add(WindowItem token)
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
            if (this.errorPositions[this.contextSize])
            {
                return new MisspelledWord(this.history, this.contextSize);                
            }

            return null;
        }

    }

}
