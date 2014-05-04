using MPSpell.Dictionaries.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Affixes
{

    public class AffixRules
    {

        private Dictionary<string, Rule> rules = new Dictionary<string, Rule>();

        public void Add(Rule rule)
        {
            rules.Add(rule.Key, rule);
        }

        public void AddItem(string key, RuleItem item)
        {
            rules[key].Add(item);
        }

        public List<string> GetPossibleWords(DictionaryItemWithFlags item, bool includeDefault = true)
        {
            List<string> words = new List<string>();
            if (includeDefault)
            {
                words.Add(item.Word);
            }

            if (null != item.Flags)
            {
                string[] flags = item.GetFlags();

                foreach (string flag in flags)
                {
                    if (this.rules.ContainsKey(flag))
                    {
                        Rule rule = this.rules[flag];

                        foreach (RuleItem ruleItem in rule)
                        {
                            if (rule.Type == RuleType.SFX)
                            {
                                string word = GenerateSFX(item.Word, ruleItem);

                                if (null != word)
                                {
                                    words.Add(word);
                                }
                            }
                        }
                    }

                }

                List<string> newItems = new List<string>();
                foreach (string flag in flags)
                {
                    if (this.rules.ContainsKey(flag))
                    {
                        Rule rule = this.rules[flag];
                        
                        foreach (RuleItem ruleItem in rule)
                        {
                            if (rule.Type == RuleType.PFX)
                            {
                                foreach (string word in words)
                                {
                                    string newWord = GeneratePFX(word, ruleItem);

                                    if (null != newWord)
                                    {
                                        newItems.Add(newWord);
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        // not found between
                    }
                }

                if (newItems.Count > 0)
                {
                    words.AddRange(newItems);
                }
            }

            return words;
        }

        private string GeneratePFX(string word, RuleItem item)
        {
            int deleteCount = 0;
            if (!int.TryParse(item.CharsToDelete, out deleteCount))
            {
                for (int pos = 0; pos < item.CharsToDelete.Length; pos++)
                {
                    if (item.CharsToDelete[pos] == word[pos])
                    {
                        deleteCount++;
                    }
                }
            }

            string tempWord = word.Substring(deleteCount);

            // condition prozatimne
            if (item.Conditions.Count == 1 && item.Conditions.First().First() == '.')
            {
                tempWord = item.StringToAdd + tempWord;
            }


            return tempWord;
        }

        private string GenerateSFX(string word, RuleItem item)
        {
            string tempWord;
            bool cond = this.CheckCondition(word, item.Conditions, RuleType.SFX);
            if (cond)
            {
                int deleteCount = 0;
                if (!int.TryParse(item.CharsToDelete, out deleteCount))
                {
                    string wordEnd = word.Substring(word.Length - item.CharsToDelete.Length);
                    if (wordEnd == item.CharsToDelete)
                    {
                        deleteCount = item.CharsToDelete.Length;
                    }

                    /*for (int pos = word.Length; pos > (word.Length - item.CharsToDelete.Length); pos--)
                    {
                        if (item.CharsToDelete[pos - word.Length] == word[pos - 1])
                        {
                            deleteCount++;
                        }
                    }*/
                }

                tempWord = word.Substring(0, word.Length - deleteCount) + item.StringToAdd;
            }
            else
            {
                tempWord = null;
            }

            return tempWord;
        }

        private bool CheckCondition(string word, List<Condition> charConditions, RuleType type)
        {
            bool match = true;

            int pos = (type == RuleType.PFX) ? 0 : word.Length - charConditions.Count;
            int skip = 0; // condstions to skip because word is longer than conditions
            if (pos < 0)
            {
                skip = Math.Abs(pos);
                pos = 0;
            }

            foreach (Condition cond in charConditions)
            {
                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                if (cond.Contains(word[pos]))
                {
                    match = cond.Except ? false : true;
                }
                else if (cond.Contains('.'))
                {
                    match = true;
                }
                else
                {
                    match = cond.Except ? true : false;
                }

                if (!match)
                {
                    break;
                }

                pos++;
            }

            return match;
        }


    }

}
