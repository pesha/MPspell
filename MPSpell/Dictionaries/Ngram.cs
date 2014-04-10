using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{

    public class Ngram
    {

        public string[] Words { get; private set; }
        public int Frequency { get; private set; }

        public Ngram(string[] words, int frequency)
        {
            Words = words;
            Frequency = frequency;
        }
        

    }


    public enum NgramType
    {
        Unigram,
        Digram,
        Trigram
    }

}
