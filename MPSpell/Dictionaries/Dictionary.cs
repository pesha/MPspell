using MPSpell.Correction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSpell.Extensions;

namespace MPSpell.Dictionaries
{

    public class Dictionary : IDictionary
    {

        public string Name { get; private set; }
        public char[] Alphabet { get; private set; }
        public char[] SpecialCharsInsideWord { get; private set; }

        private string path;
        private string wordBoundaryRegex;
        private bool isDictionaryLoaded = false;
        private Dictionary<char, List<char>> accentPairs;
        private Dictionary<DictionaryFileType, string> files = new Dictionary<DictionaryFileType, string>();
        private Dictionary<EditOperation, ConfusionMatrix> matrixes = new Dictionary<EditOperation, ConfusionMatrix>();
        private Dictionary<FrequencyVectorType, FrequencyVector<string>> frequences = new Dictionary<FrequencyVectorType, FrequencyVector<string>>();
        private Dictionary<NgramType, NgramCollection> ngrams = new Dictionary<NgramType, NgramCollection>();
        private DictionaryLoader loader;

        private DictionaryNode dictionary = new DictionaryNode();

        public Dictionary(DictionaryLoader loader, string name, string path, char[] alphabet, char[] specialChars = null, string wordBoundaryRegex = null, Dictionary<char, List<char>> accentPairs = null)
        {
            Name = name;
            Alphabet = alphabet;
            SpecialCharsInsideWord = specialChars;

            this.path = path;
            this.wordBoundaryRegex = wordBoundaryRegex;
            this.accentPairs = accentPairs;
            this.loader = loader;
        }

        public void Add(string word)
        {
            word = word.ToLowerInvariant();
            dictionary.Add(word);
        }

        public void AddRange(List<string> words)
        {
            foreach (string word in words)
            {
                this.Add(word);
            }
        }

        public bool FindWord(string word)
        {
            return dictionary.FindWord(word);
        }

        public bool ExistPath(string token)
        {
            return dictionary.ExistPath(token);
        }

        public string[] GetAlphabetAsString()
        {
            return Alphabet.ToStringArray();
        }

        public string[] GetAlphabetForErrorModel(bool withSpace = false)
        {
            List<string> alphabet = new List<string>();
            alphabet.AddRange(this.GetAlphabetAsString());
            foreach (char chr in SpecialCharsInsideWord)
            {
                alphabet.Add(chr.ToString());
            }
            if (withSpace)
            {
                alphabet.Add(" ");
            }

            string[] res = alphabet.ToArray();
            Array.Sort<string>(res);

            return res;
        }

        public string GetWordBoundaryRegex()
        {
            return this.wordBoundaryRegex;
        }

        public void AddFile(DictionaryFileType type, string filename)
        {
            files.Add(type, filename);
        }

        internal string GetFile(DictionaryFileType type, bool withPath = true)
        {
            if (files.ContainsKey(type))
            {
                return withPath ? path + "/" + files[type] : files[type];
            }

            return null;
        }

        public int GetConfusionFrequency(char x, char y, EditOperation operation)
        {
            if (this.Alphabet.Contains(x) && this.Alphabet.Contains(y))
            {
                return matrixes[operation].GetValue(x, y);
            }

            return 0;
        }

        public int GetOneCharFrequency(string str)
        {
            if (this.frequences[FrequencyVectorType.OneChar].ContainsKey(str))
            {
                return this.frequences[FrequencyVectorType.OneChar][str];
            }

            return 0;
        }

        public int GetTwoCharFrequency(string str)
        {
            if (this.frequences[FrequencyVectorType.TwoChar].ContainsKey(str))
            {
                return this.frequences[FrequencyVectorType.TwoChar][str];
            }

            return 0;
        }

        public NgramType GetHighestAvailableNgramCollection(int contextSize)
        {
            if (contextSize == 3)
            {
                if (this.ngrams.ContainsKey(NgramType.Trigram))
                {
                    return NgramType.Trigram;
                }
                if (this.ngrams.ContainsKey(NgramType.Digram))
                {
                    return NgramType.Digram;
                }
            }
            if (contextSize == 2)
            {
                if (this.ngrams.ContainsKey(NgramType.Digram))
                {
                    return NgramType.Digram;
                }
            }

            return NgramType.Unigram;
        }

        public bool IsAvailableNgramCollection(NgramType type)
        {
            return this.ngrams.ContainsKey(type);
        }

        public Dictionary<char, List<char>> GetAccentPairs()
        {
            return this.accentPairs;
        }

        public NgramCollection GetNgramCollection(NgramType type)
        {
            return this.ngrams[type];
        }

        internal void AddConfusionMatrix(EditOperation operation, ConfusionMatrix matrix)
        {
            matrixes.Add(operation, matrix);
        }

        internal void AddFrequencyVector(FrequencyVectorType type, FrequencyVector<string> vector)
        {
            frequences.Add(type, vector);
        }

        internal void AddNgramCollection(NgramType type, NgramCollection collection)
        {
            ngrams.Add(type, collection);
        }


        // @todo refactor
        public void PreloadDictionaries()
        {
            if (!this.isDictionaryLoaded)
            {
                if (this.GetFile(DictionaryFileType.LineDictionary) != null)
                {
                    this.loader.ParseSimpleDictionary(this);
                }
                else
                {
                    this.loader.ParseDictionary(this);
                }
                this.loader.ParseConfusionMatrixes(this);
                this.loader.ParseFrequences(this);
                this.loader.ParseNgrams(this);
            }
        }

    }

    public enum DictionaryFileType
    {
        LineDictionary,
        Dictionary,
        Affix,
        OneCharFrequences,
        TwoCharFrequences,
        DeletetionsMatrix,
        InsertionsMatrix,
        SubstitutionsMatrix,
        TranspositionsMatrix,
        UnigramFrequences,
        DigramFrequences,
        TrigramFrequences,
        Unknown
    }


}
