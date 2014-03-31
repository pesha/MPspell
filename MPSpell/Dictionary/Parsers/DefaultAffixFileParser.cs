using MPSpell.Dictionary.Affixes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MPSpell.Dictionary.Parsers
{



    public class DefaultAffixFileParser : IAffixFileParser
    {

        protected static readonly char[] Whitespace = new char[] { ' ' };

        public AffixRules Parse(string file, Encoding encoding = null)
        {
            AffixRules rules = new AffixRules();
            
            using (StreamReader reader = new StreamReader(file, encoding))
            {
                string line;

                while (null != (line = reader.ReadLine()))
                {
                    string[] parts = this.ParseLine(line);
                    if (null != parts)
                    {
                        if (parts.Length == 4)
                        {
                            Combinable comb = parts[2] == "Y" ? Combinable.Y : Combinable.N;
                            RuleType rType;
                            switch (parts[0])
                            {
                                case "SFX": rType = RuleType.SFX; break;
                                case "PFX": rType = RuleType.PFX; break;
                                default: rType = RuleType.Unknown; break;
                            }

                            rules.Add(new Rule(parts[1], comb, rType));
                        }
                        else if (parts.Length == 5)
                        {
                            rules.AddItem(parts[1], new RuleItem(parts[2], parts[3], parts[4]));
                        }
                    }
                }
            }

            return rules;
        }

        private string[] ParseLine(string line)
        {
            int commentStart = line.IndexOf('#');
            if (commentStart >= 0)
            {
                line = line.Substring(0, commentStart);
            }


            string[] parts = null;
            if (line.Length > 4)
            {
                line = line.Trim();
                string[] tempParts = line.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
                if (tempParts[0] == "PFX" || tempParts[0] == "SFX")
                {
                    parts = tempParts;
                }
            }

            return parts;
        }

    }
  
}
