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
        private FrequencyVectorParser frequencyParser;

        public DictionaryLoader(IDictionaryFileParser parser, IAffixFileParser affixParser = null, IConfusionMatrixParser matrixParser = null, FrequencyVectorParser freqParser = null)
        {
            this.parser = parser;
            this.affixParser = affixParser;
            this.matrixParser = matrixParser;
            this.frequencyParser = freqParser;
        }

        internal void ParseDictionary(Dictionary dictionary)
        {
            AffixRules rules = null;
            Encoding encoding = null;

            string affixFile = dictionary.GetFile(DictionaryFileType.Affix);
            if (null != affixFile)
            {
                encoding = Utils.DetectEncoding(affixFile);
                if (null == encoding)
                {
                    encoding = EncodingDetector.DetectEncoding(affixFile);
                }
                rules = this.affixParser.Parse(affixFile, encoding);
            }

            string fileName = dictionary.GetFile(DictionaryFileType.Dictionary);
            DictionaryWithFlags rawDict = this.parser.Parse(fileName, encoding);

            foreach (DictionaryItemWithFlags item in rawDict)
            {
                if (null == rules)
                {
                    dictionary.Add(item.Word);
                }
                else
                {
                    dictionary.AddRange(rules.GetPossibleWords(item));
                }
            }
        }

        internal void ParseConfusionMatrixes(Dictionary dictionary)
        {
            DictionaryFileType[] matrixes = new DictionaryFileType[] {
                DictionaryFileType.DeletetionsMatrix,
                DictionaryFileType.InsertionsMatrix,
                DictionaryFileType.SubstitutionsMatrix,
                DictionaryFileType.TranspositionsMatrix
            };

            foreach (DictionaryFileType type in matrixes)
            {
                string file = dictionary.GetFile(type);
                if (null != file)
                {
                    ConfusionMatrix matrix = this.matrixParser.ParseMatrix(file);
                    dictionary.AddConfusionMatrix(type, matrix);
                }
            }
        }

        internal void ParseFrequences(Dictionary dictionary)
        {
            FrequencyVector<string> oneChrFrq = this.frequencyParser.ParseFrequency(dictionary.GetFile(DictionaryFileType.OneCharFrequences));
            dictionary.AddFrequencyVector(DictionaryFileType.OneCharFrequences, oneChrFrq);

            FrequencyVector<string> twoChrFrq = this.frequencyParser.ParseFrequency(dictionary.GetFile(DictionaryFileType.TwoCharFrequences));
            dictionary.AddFrequencyVector(DictionaryFileType.TwoCharFrequences, twoChrFrq);
        }

    }

}
