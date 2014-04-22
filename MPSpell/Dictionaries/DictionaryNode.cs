using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{
    class DictionaryNode
    {

        public char? Key { get; private set; }
        public bool IsEnd { get; private set; }
        private List<DictionaryNode> nodes = new List<DictionaryNode>();

        public DictionaryNode(char? key = null, bool isEnd = false)
        {            
            Key = key;
            IsEnd = isEnd;
        }

        public void Add(string word, int level = 0)
        {
            if (level >= word.Length)
            {
                this.IsEnd = true;
                return;
            }

            foreach(DictionaryNode node in nodes){
                if (node.Key == word[level])
                {
                    node.Add(word, ++level);
                    return;
                }
            }

            DictionaryNode newNode = new DictionaryNode(word[level]);
            nodes.Add(newNode);
            newNode.Add(word, ++level);
        }

        public bool FindWord(string word)
        {
            return this.FindToken(word);
        }

        private bool FindToken(string word, int level = 0)
        {
            if (word.Length == level)
            {
                return IsEnd;
            }

            foreach(DictionaryNode node in nodes){
                if (node.Key == word[level])
                {
                    return node.FindToken(word, ++level);                    
                }
            }

            return false;
        }

    }
}
