using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public class AccentModel : IAccentModel
    {

        private Dictionary dictionary;
        private Dictionary<char, List<char>> accentPairs;

        public AccentModel(Dictionary dictionary)
        {
            this.dictionary = dictionary;
            this.accentPairs = dictionary.GetAccentPairs();
        }

        public List<string> AddAccent(string word)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < word.Length; i++)
            {
                if (this.accentPairs.ContainsKey(word[i]))
                {
                    List<string> newItems = new List<string>();
                    foreach(char acChr in this.accentPairs[word[i]]){                        
                        for (int pos = 0; pos < result.Count; pos++)
                        {
                            string item = result[pos] + acChr;
                            if (this.dictionary.ExistPath(item))
                            {
                                newItems.Add(item);
                            }
                        }
                    }

  
                    for (int pos = 0; pos < result.Count; pos++)
                    {
                        string item = result[pos] + word[i];
                        if (this.dictionary.ExistPath(item))
                        {
                            newItems.Add(item);
                        }
                    }

                    if (newItems.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        result = newItems;
                    }
                }
                else
                {
                    if (result.Count > 0)
                    {
                        for (int pos = 0; pos < result.Count; pos++)
                        {
                            result[pos] += word[i];
                        }
                    }
                    else
                    {
                        result.Add(word[i].ToString());
                    }
                }
            }


            List<string> finalResult = new List<string>();
            foreach (string candidate in result)
            {
                if (dictionary.FindWord(candidate))
                {
                    finalResult.Add(candidate);
                }
            }

            return finalResult.Count > 0 ? finalResult : null;
        }
    }
}
