using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{
    public class Tokenizer
    {

        protected IDictionary dictionary;

        protected string word = string.Empty;
        protected Window window = new Window();
        protected uint lastStart = 0;
        protected uint currentPos = 0;


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
                            string pureWord = this.TrimSpecialChars(word.ToLowerInvariant());
                            if (string.Empty == pureWord)
                            {
                                word = String.Empty;
                                break;
                            }

                            bool context = this.HasContextEnded(chr) ? true : false;

                            if (!this.dictionary.FindWord(pureWord))
                            {
                                token = new Token(pureWord, context, word, lastStart);
                            }
                            else
                            {
                                token = new Token(pureWord, context);
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

        protected string TrimSpecialChars(string word)
        {
            // nutno predelat do regularu kvuli opakovane aplikaci
            return word.Trim(new char[] { '„', ',', '”', '\'', ':' });
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
