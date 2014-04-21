using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public class ErrorModel : IErrorModel
    {

        private string[] alphabet;
        private IDictionary dictionary;

        public ErrorModel(IDictionary dictionary)
        {
            this.dictionary = dictionary;
            this.alphabet = dictionary.GetAlphabetAsString();
        }

        private double CalculateProbability(EditOperation operation, char x, char y)
        {
            double output = 0;
            string fr;
            switch (operation)
            {
                case EditOperation.Insertion:
                    output = (double) dictionary.GetConfusionFrequency(x,y, EditOperation.Insertion) / dictionary.GetOneCharFrequency(x.ToString());
                    break;

                case EditOperation.Deletion:
                    fr = x.ToString() + y.ToString();
                    output = (double) dictionary.GetConfusionFrequency(x, y, EditOperation.Deletion) / dictionary.GetTwoCharFrequency(fr);
                    break;

                case EditOperation.Substitution:
                    output = (double) dictionary.GetConfusionFrequency(x, y, EditOperation.Substitution) / dictionary.GetOneCharFrequency(y.ToString());
                    break;

                case EditOperation.Transposition:
                    fr = x.ToString() + y.ToString();                    
                    output = (double) dictionary.GetConfusionFrequency(x, y, EditOperation.Transposition) / dictionary.GetTwoCharFrequency(fr);
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
                        double prop = this.CalculateProbability(EditOperation.Substitution, word[i], charItem[0]);
                        if (!result.ContainsKey(edited))
                        {
                            result.Add(edited, prop);
                        }
                        else if (prop > result[edited])
                        {
                            result[edited] = prop;
                        }
                    }
                }
            }

            // deletions
            for (int i = 0; i < word.Length; i++)
            {
                string edited = String.Copy(word).Remove(i, 1);
                if (dictionary.FindWord(edited))
                {
                    char prev = (i - 1) < 0 ? ' ' : word[i];
                    double prop = this.CalculateProbability(EditOperation.Deletion,prev,word[i]);
                    if (!result.ContainsKey(edited))
                    {
                        result.Add(edited, prop);
                    }
                    else if (prop > result[edited])
                    {
                        result[edited] = prop;
                    }
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
                        char prev = (i - 1) < 0 ? ' ' : word[i-1];
                        double prop = this.CalculateProbability(EditOperation.Insertion, prev, item[0]);
                        if (!result.ContainsKey(edited))
                        {
                            result.Add(edited, prop);
                        }
                        else if (prop > result[edited])
                        {
                            result[edited] = prop;
                        }
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
                    double prop = this.CalculateProbability(EditOperation.Transposition, word[i], word[i + 1]);
                    if (!result.ContainsKey(edited))
                    {
                        result.Add(edited, prop);
                    }
                    else if(prop > result[edited]) 
                    {
                        result[edited] = prop;
                    }
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
