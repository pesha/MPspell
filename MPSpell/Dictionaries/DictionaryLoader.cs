using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Dictionaries.Parsers;
using MPSpell.Extensions;

namespace MPSpell.Dictionaries
{
    public class DictionaryLoader
    {

        private IDictionaryFileParser parser;
        private IAffixFileParser affixParser;
        private IConfusionMatrixParser matrixParser;

        public DictionaryLoader(IDictionaryFileParser parser, IAffixFileParser affixParser = null, IConfusionMatrixParser matrixParser = null)
        {
            this.parser = parser;
            this.affixParser = affixParser;
            this.matrixParser = matrixParser;
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

            if (null != matrixParser)
            {
                foreach (string matrixFile in dictionary.ConfusionMatrixesFiles)
                {
                    ConfusionMatrix matrix = this.matrixParser.ParseMatrix(dictionary.Location + "/" + matrixFile);

                    dictionary.ConfusionMatrixes.Add(matrix);
                }
            }
            
            return dictionary;
        }




    }

}
