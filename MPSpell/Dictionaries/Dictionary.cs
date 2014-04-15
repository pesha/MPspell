using MPSpell.Correction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{

    public class Dictionary : List<string>, IDictionary
    {

        public string Name { get; private set; }

        private string path;
        private Dictionary<DictionaryFileType, string> files = new Dictionary<DictionaryFileType, string>();
        private Dictionary<EditOperation, ConfusionMatrix> matrixes = new Dictionary<EditOperation, ConfusionMatrix>();
        private Dictionary<FrequencyVectorType, FrequencyVector<string>> frequences = new Dictionary<FrequencyVectorType, FrequencyVector<string>>();
        private Dictionary<NgramType, NgramCollection> ngrams = new Dictionary<NgramType, NgramCollection>();
        private DictionaryLoader loader;

        public Dictionary(DictionaryLoader loader, string name, string path)
        {
            Name = name;
            this.path = path;
            this.loader = loader;        
        }

        public void AddFast(string word)
        {
            base.Add(word);
        }

        public new void Add(string word)
        {
            word = word.ToLowerInvariant();
            if (!this.Contains(word))
            {
                base.Add(word);
            }
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
            return this.Contains(word) ? true : false;
        }

        public string[] GetAlphabet()
        {
            return new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        }

        public static string[] GetAlphabetStatic()
        {
            return new string[] { " ", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
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
            return matrixes[operation].GetValue(x, y);
        }

        public int GetOneCharFrequency(string str)
        {
            return this.frequences[FrequencyVectorType.OneChar][str];
        }

        public int GetTwoCharFrequency(string str)
        {
            return this.frequences[FrequencyVectorType.TwoChar][str];
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
