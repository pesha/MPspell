using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public class ErrorModel : IWordGenerator
    {

        private string[] alphabet;
        private Dictionary dictionary;

        public ErrorModel(Dictionary dictionary)
        {
            this.dictionary = dictionary;
            this.alphabet = dictionary.GetAlphabet();
        }

        private double CalculateProbability(EditOperation operation, char x, char y)
        {
            double output = 0;
            switch (operation)
            {
                case EditOperation.Insertion:

                    break;

                case EditOperation.Deletion:

                    break;

                case EditOperation.Substitution:

                    break;

                case EditOperation.Transposition:

                    break;

            }

            return output;
        }

        public Dictionary<string, double> GeneratePossibleWords(string word)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            
            // substitution
            for (int i = 0; i < word.Length; i++)
            {
                foreach (string charItem in alphabet)
                {
                    string edited = String.Copy(word).Remove(i, 1).Insert(i, charItem);
                    if (dictionary.FindWord(edited))
                    {
                        result.Add(edited, this.CalculateProbability(EditOperation.Substitution,word[i],charItem[0]));
                    }
                }
            }

            // deletions
            for (int i = 0; i < word.Length; i++)
            {
                string edited = String.Copy(word).Remove(i, 1);
                if (dictionary.FindWord(edited))
                {
                    char prev = (i - 1) < 0 ? '@' : word[i];
                    result.Add(edited, this.CalculateProbability(EditOperation.Deletion,prev,word[i]));
                }
            }


            // insertions
            for (int i = 0; i <= word.Length; i++)
            {
                foreach (string item in alphabet)
                {
                    string edited = String.Copy(word).Insert(i, item);
                    if (dictionary.FindWord(edited))
                    {
                        char prev = (i - 1) < 0 ? '@' : word[i];
                        result.Add(edited, this.CalculateProbability(EditOperation.Insertion, prev, item[0]));
                    }
                }
            }


            // transposition
            for (int i = 0; i < word.Length - 1; i++)
            {
                string newString = String.Copy(word);
                string charItem = newString[i].ToString();
                string edited = newString.Remove(i, 1).Insert(i + 1, charItem);
                if (dictionary.FindWord(edited))
                {
                    result.Add(edited, this.CalculateProbability(EditOperation.Transposition,word[i],word[i+1]));
                }
            }

            return result;
        }


    }

    public enum EditOperation
    {
        Insertion,
        Deletion,
        Substitution,
        Transposition,
        Unknown,
    }


}
