﻿using MPSpell.Correction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{

    public class Dictionary : List<string>
    {

        public string Name { get; private set; }

        private string path;
        private Dictionary<DictionaryFileType, string> files = new Dictionary<DictionaryFileType, string>();
        private Dictionary<EditOperation, ConfusionMatrix> matrixes = new Dictionary<EditOperation, ConfusionMatrix>();
        private Dictionary<DictionaryFileType, FrequencyVector<string>> frequences = new Dictionary<DictionaryFileType, FrequencyVector<string>>();
        private DictionaryLoader loader;

        public Dictionary(DictionaryLoader loader, string name, string path)
        {
            Name = name;
            this.path = path;
            this.loader = loader;
        }

        public bool FindWord(string word, bool caseSensitive = false)
        {
            return this.Contains(word) ? true : (this.Contains(word.ToLowerInvariant()) ? true : false);
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
            return this.frequences[DictionaryFileType.OneCharFrequences][str];
        }

        public int GetTwoCharFrequency(string str)
        {
            return this.frequences[DictionaryFileType.TwoCharFrequences][str];
        }

        internal void AddConfusionMatrix(EditOperation operation, ConfusionMatrix matrix)
        {
            matrixes.Add(operation, matrix);
        }

        internal void AddFrequencyVector(DictionaryFileType type, FrequencyVector<string> vector)
        {
            frequences.Add(type, vector);
        }

        // @todo refactor
        public void PreloadDictionaries()
        {
            this.loader.ParseDictionary(this);
            this.loader.ParseConfusionMatrixes(this);
            this.loader.ParseFrequences(this);
        }

    }

    public enum DictionaryFileType
    {
        PlainDictionary,
        Dictionary,
        Affix,
        OneCharFrequences,
        TwoCharFrequences,
        DeletetionsMatrix,
        InsertionsMatrix,
        SubstitutionsMatrix,
        TranspositionsMatrix,
        Unknown
    }


}
