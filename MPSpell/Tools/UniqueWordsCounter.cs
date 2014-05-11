using MPSpell.Dictionaries;
using MPSpell.Extensions;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public class UniqueWordsCounter : Tokenizer
    {

        private string file;
        private List<string> words = new List<string>();


        public UniqueWordsCounter(string file, IDictionary dictionary)
            : base(dictionary)
        {
            this.file = file;
        }

        public List<string> GetWords()
        {
            return words;
        }


        public void Run()
        {            
            using (StreamReader reader = EncodingDetector.GetStreamWithEncoding(this.file))
            {
                string line;
                string word = null;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine() + " ";

                    for (int i = 0; i < line.Length; i++)
                    {
                        word = this.HandleChar(line[i]);
                        

                        if (null != word)
                        {
                            word = word.Replace("\t", "");
                            if (!words.Contains(word))
                            {
                                words.Add(word);
                            }
                        }
                    }


                }
            }
        }

        private string HandleChar(char chr)
        {
            currentPos++;
            string wordResult = null;

            switch (chr)
            {
                case '\r':
                case '\n':
                case '.':
                case '?':
                case '!':
                case ' ':
                    if (String.Empty != word)
                    {
                        string[] wordContext = this.TrimSpecialChars(word);
                        string pureWord = wordContext[1].ToLowerInvariant();


                        if (string.Empty == pureWord)
                        {
                            if (tokenWithAlphanum.Match(word).Success)
                            {
                                pureWord = word;
                            }
                            else
                            {
                                lastStart = currentPos;
                                word = String.Empty;
                                break;
                            }
                        }

                        wordResult = pureWord;

                        word = String.Empty;
                    }

                    if (null != wordResult)
                    {
                        return wordResult;
                    }

                    lastStart = currentPos;

                    break;

                default:
                    word += chr;
                    break;
            }

            return wordResult;
        }


    }
}
