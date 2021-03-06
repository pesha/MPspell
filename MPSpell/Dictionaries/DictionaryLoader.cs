﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Dictionaries.Parsers;
using MPSpell.Extensions;
using MPSpell.Correction;
using System.IO;

namespace MPSpell.Dictionaries
{
    public class DictionaryLoader
    {

        private IDictionaryFileParser parser;
        private IAffixFileParser affixParser;
        private IConfusionMatrixParser matrixParser;
        private FrequencyVectorParser frequencyParser;
        private NgramParser ngramParser;

        public DictionaryLoader(
            IDictionaryFileParser parser,
            IAffixFileParser affixParser = null,
            IConfusionMatrixParser matrixParser = null,
            FrequencyVectorParser freqParser = null,
            NgramParser ngramParser = null)
        {
            this.parser = parser;
            this.affixParser = affixParser;
            this.matrixParser = matrixParser;
            this.frequencyParser = freqParser;
            this.ngramParser = ngramParser;
        }

        // todo move 
        internal void ParseSimpleDictionary(Dictionary dictionary)
        {
            string file = dictionary.GetFile(DictionaryFileType.LineDictionary);
            if (null != file)
            {
                Encoding enc = EncodingDetector.DetectEncoding(file);
                using (StreamReader reader = new StreamReader(file, enc))
                {
                    while (!reader.EndOfStream)
                    {
                        dictionary.Add(reader.ReadLine());
                    }
                }
            }
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
                if (null == item.Flags)
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
                    dictionary.AddConfusionMatrix(ConvertFileTypeToEditOperation(type), matrix);
                }
            }
        }

        private EditOperation ConvertFileTypeToEditOperation(DictionaryFileType type)
        {
            EditOperation op = EditOperation.Unknown;
            switch (type)
            {
                case DictionaryFileType.DeletetionsMatrix:
                    op = EditOperation.Deletion;
                    break;

                case DictionaryFileType.InsertionsMatrix:
                    op = EditOperation.Insertion;
                    break;

                case DictionaryFileType.SubstitutionsMatrix:
                    op = EditOperation.Substitution;
                    break;

                case DictionaryFileType.TranspositionsMatrix:
                    op = EditOperation.Transposition;
                    break;
            }

            return op;
        }

        internal void ParseFrequences(Dictionary dictionary)
        {
            FrequencyVector<string> oneChrFrq = this.frequencyParser.ParseFrequency(dictionary.GetFile(DictionaryFileType.OneCharFrequences));
            dictionary.AddFrequencyVector(FrequencyVectorType.OneChar, oneChrFrq);

            FrequencyVector<string> twoChrFrq = this.frequencyParser.ParseFrequency(dictionary.GetFile(DictionaryFileType.TwoCharFrequences));
            dictionary.AddFrequencyVector(FrequencyVectorType.TwoChar, twoChrFrq);
        }

        internal void ParseNgrams(Dictionary dictionary)
        {
            DictionaryFileType[] files = new DictionaryFileType[]
            {
                DictionaryFileType.UnigramFrequences,
                DictionaryFileType.DigramFrequences,
                DictionaryFileType.TrigramFrequences
            };

            foreach (DictionaryFileType type in files)
            {
                string file = dictionary.GetFile(type);

                if (null != file)
                {
                    NgramCollection collection = this.ngramParser.ParseNgrams(file);
                    NgramType ngramType;
                    switch (type)
                    {
                        case DictionaryFileType.DigramFrequences:
                            ngramType = NgramType.Digram;
                            break;

                        case DictionaryFileType.TrigramFrequences:
                            ngramType = NgramType.Trigram;
                            break;

                        default:
                            ngramType = NgramType.Unigram;
                            break;
                    }


                    dictionary.AddNgramCollection(ngramType, collection);
                }
            }
        }

    }

}
