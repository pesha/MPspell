using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSpell.Dictionary.Affixes;
using MPSpell.Dictionary.Parsers;

namespace MPSpell.Dictionary
{
    public class DictionaryBuilder
    {

        private IDictionaryFileParser parser;
        private IAffixFileParser affixParser;

        public DictionaryBuilder(IDictionaryFileParser parser, IAffixFileParser affixParser = null)
        {
            this.parser = parser;
            this.affixParser = affixParser;
        }

        public Dictionary BuildDictionary(string dictionaryFile, string suffixFile = null)
        {
            AffixRules rules = null;
            Encoding encoding = null;
            if (null != suffixFile)
            {
                encoding = Utils.DetectEncoding(suffixFile);
                rules = this.affixParser.Parse(suffixFile, encoding);
            }
            DictionaryWithFlags rawDict = this.parser.Parse(dictionaryFile, encoding);
            
            Dictionary dictionary = new Dictionary();
            foreach(DictionaryItemWithFlags item in rawDict){
                if (null == rules)
                {
                    dictionary.Add(item.Word);
                }
                else
                {                    
                    dictionary.AddRange(rules.GetPossibleWords(item));
                }
            }
            
            return dictionary;
        }




    }

    public class Dictionary : List<string>
    {


    }

}
