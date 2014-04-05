using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries.Affixes
{

    public class Rule : List<RuleItem>
    {

        public string Key { get; private set; }
        public Combinable Combinable { get; private set; }
        public RuleType Type { get; private set; }

        public Rule(string key, Combinable combinable, RuleType type)
        {
            Key = key;
            Combinable = combinable;
            Type = type;
        }

    }

    public class RuleItem
    {

        public string CharsToDelete { get; private set; }
        public string StringToAdd { get; private set; }
        public List<Condition> Conditions { get; private set; }


        public RuleItem(string charsToDelete, string stringToAdd, string conditions)
        {
            CharsToDelete = charsToDelete;
            StringToAdd = stringToAdd;
            Conditions = this.ParseCondition(conditions);
        }

        private List<Condition> ParseCondition(string condition)
        {

            char[] chrAr = condition.ToCharArray();

            List<Condition> conditions = new List<Condition>();
            Stack<char> tempStack = null;
            bool except = false;
            foreach (char item in chrAr)
            {
                switch (item)
                {
                    case '[':
                        tempStack = new Stack<char>();
                        continue;

                    case '^':
                        except = true;
                        continue;

                    case ']':
                        Condition data = new Condition();

                        while (tempStack.Count > 0)
                        {
                            data.Add(tempStack.Pop());
                        }

                        data.Except = except;
                        conditions.Add(data);
                        tempStack = null;
                        except = false;
                        break;

                    default:
                        if (null == tempStack)
                        {
                            conditions.Add(new Condition { item });
                        }
                        else
                        {
                            tempStack.Push(item);
                        }
                        break;

                }

            }

            return conditions;
        }

    }

}
