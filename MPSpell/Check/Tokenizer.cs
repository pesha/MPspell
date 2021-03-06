﻿using MPSpell.Dictionaries;
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
        protected Token lastToken;
        protected int lastStart = 0;
        protected int currentPos = 0;

        protected Regex rg;
        protected Regex containSpecial = new Regex(@"([-]+)", RegexOptions.Compiled);
        protected Regex tokenWithAlphanum = new Regex(@"([a-z\d]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        protected Regex abbreviation = new Regex("([A-Z]{2,})", RegexOptions.Compiled);

        public Tokenizer(IDictionary dict)
        {
            dictionary = dict;

            string regexp = dict.GetWordBoundaryRegex();
            if (!String.IsNullOrEmpty(regexp))
            {
                rg = new Regex(@"(\W*)("+regexp+@")(\W*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            else
            {
                rg = new Regex(@"(\W*)([a-z-]*'*[a-z]+)(\W*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }

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
                            bool skipDetection = false;
                            string[] wordContext = this.TrimSpecialChars(word);
                            string pureWord = wordContext[1].ToLowerInvariant();
                            if (pureWord.Length == 1)
                            {
                                skipDetection = true;
                            }

                            if (containSpecial.Match(pureWord).Success || abbreviation.Match(wordContext[1]).Success || this.IsProbablyName(wordContext[1]))
                            {
                                skipDetection = true;
                            }                            
                            if (string.Empty == pureWord)
                            {
                                if (tokenWithAlphanum.Match(word).Success)
                                {
                                    skipDetection = true;
                                    pureWord = word;
                                }
                                else
                                {
                                    lastStart = currentPos;
                                    word = String.Empty;
                                    break;
                                }
                            }

                            bool context = this.HasContextEnded(chr) ? true : false;

                            if (!skipDetection && !this.dictionary.FindWord(pureWord))
                            {
                                token = new Token(pureWord, context, wordContext, lastStart);
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
                            lastToken = token;
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

        protected bool IsProbablyName(string word)
        {
            if (lastToken != null && word != String.Empty)
            {
                if (word[0] == char.ToUpperInvariant(word[0]) && !lastToken.ContextEnd)
                {
                    return true;
                }
            }

            return false;
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
