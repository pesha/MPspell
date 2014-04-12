using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    public class Tokenizer
    {

        protected IDictionary dictionary;

        protected string word = string.Empty;
        protected Window window = new Window();
        protected int lastStart = 0;
        protected int currentPos = 0;

        private Regex rg = new Regex(@"(\W*)([a-z]*)(\W*)", RegexOptions.Compiled);

        public Tokenizer(IDictionary dict)
        {
            dictionary = dict;
        }

        public MisspelledWord HandleChar(char chr, bool padding = false)
        {
            currentPos++;
            MisspelledWord misspelling = null;

            if (padding && word == String.Empty)
            {
                window.Add(new Token('.', true));
                misspelling = window.GetMisspelledWord();
            }
            else
            {

                switch (chr)
                {
                    case '\r':
                    case '\n':
                    case '.':
                    case '?':
                    case '!':
                    case ' ':
                        Token token = null;
                        if (String.Empty != word)
                        {
                            string[] wordContext = this.TrimSpecialChars(word.ToLowerInvariant());
                            if (string.Empty == wordContext[1])
                            {
                                word = String.Empty;
                                break;
                            }

                            bool context = this.HasContextEnded(chr) ? true : false;

                            if (!this.dictionary.FindWord(wordContext[1]))
                            {
                                token = new Token(wordContext[1], context, wordContext, lastStart);
                            }
                            else
                            {
                                token = new Token(wordContext[1], context);
                            }

                            word = String.Empty;
                        }
                                                    
                        if (null != token)
                        {
                            
                            window.Add(token);
                            misspelling = window.GetMisspelledWord();
                        }

                        lastStart = currentPos;

                        break;

                    default:
                        word += chr;
                        break;
                }

            }

            return misspelling;
        }

        protected string[] TrimSpecialChars(string word)
        {
            // nutno predelat do regularu kvuli opakovane aplikaci
            //return word.Trim(new char[] { '„', ',', '”', '\'', ':' });

            return new string[] {
                rg.Match(word).Groups[1].Value,    
                rg.Match(word).Groups[2].Value,
                rg.Match(word).Groups[3].Value
            }; ;
       } 

        protected bool HasContextEnded(char chr)
        {
            bool end = false;
            switch (chr)
            {
                case '.':
                case '?':
                case '!':
                    end = true;
                    break;
            }

            return end;
        }


    }
}
