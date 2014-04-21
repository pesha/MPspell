using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell
{
    class NgramNode
    {

        public char? Key { get; private set; }
        public int Word { get; private set; }
        public int Occurences { get; private set; }
        public List<NgramNode> nodes = new List<NgramNode>();

        public NgramNode(char? key = null, int word = 0)
        {
            Key = key;
            Word = word;
        }

        public void Add(Ngram ngram, int word = 0, int level = 0)
        {
            if (level >= ngram.Words[word].Length)
            {
                word++;
                level = 0;

                if (ngram.Words.Length < (word + 1))
                {
                    Occurences = ngram.Frequency;
                    return;
                }
            }

            foreach (NgramNode node in nodes)
            {
                if (node.Key == ngram.Words[word][level])
                {
                    node.Add(ngram, word, ++level);
                    return;
                }
            }

            NgramNode newNode = new NgramNode(ngram.Words[word][level], word);
            nodes.Add(newNode);
            newNode.Add(ngram, word, ++level);
        }

        public int GetOccurences(string[] context)
        {
            NgramNode node = this.FindNode(context);
            return (node != null) ? node.Occurences : 0;
        }

        private NgramNode FindNode(string[] context, int word = 0, int level = 0)
        {            
            if (level >= context[word].Length)
            {
                word++;
                level = 0;

                if (context.Length < (word + 1))
                {                    
                    return this;
                }
            }

            NgramNode result = null;
            int nextLevel = level + 1;
            foreach (NgramNode node in nodes)
            {
                if (node.Key == context[word][level])
                {
                    result = node.FindNode(context, word, nextLevel);
                    //todo neni zde problem?
                    if (null != result)
                    {
                        return result;
                    }
                }
            }

            return result;
        }


    }
}
