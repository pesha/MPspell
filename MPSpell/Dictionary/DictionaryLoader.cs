using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSpell.Dictionary.Affixes;
using MPSpell.Dictionary.Parsers;
using MPSpell.Extensions;

namespace MPSpell.Dictionary
{
    public class DictionaryLoader
    {

        private IDictionaryFileParser parser;
        private IAffixFileParser affixParser;

        public DictionaryLoader(IDictionaryFileParser parser, IAffixFileParser affixParser = null)
        {
            this.parser = parser;
            this.affixParser = affixParser;
        }

        public Dictionary LoadDictionary(Dictionary dictionary)
        {
            AffixRules rules = null;
            Encoding encoding = null;
            if (null != dictionary.SuffixFileName)
            {
                string suffixFile = dictionary.Location + "/" + dictionary.SuffixFileName;
                encoding = Utils.DetectEncoding(suffixFile);
                if (null == encoding)
                {
                    encoding = EncodingDetector.DetectEncoding(suffixFile);
                }
                rules = this.affixParser.Parse(suffixFile, encoding);
            }
            string fileName = dictionary.Location + "/" + dictionary.FileName;
            DictionaryWithFlags rawDict = this.parser.Parse(fileName, encoding);
                        
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

}
