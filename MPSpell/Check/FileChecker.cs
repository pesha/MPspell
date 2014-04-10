using MPSpell.Dictionaries;
using MPSpell.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{

    public class FileChecker : Checker 
    {

        
        public FileChecker(MPSpell.Dictionaries.Dictionary dict)
            : base(dict)
        {
        }

        public List<MisspelledWord> CheckFile(string file)
        {
            List<MisspelledWord> misspelledWords = new List<MisspelledWord>();

            Window window = new Window();            

            uint currentPos = 0;
            using (StreamReader sr = EncodingDetector.GetStreamWithEncoding(file))
            {                
                string word = String.Empty;                
                while(!sr.EndOfStream){
                    char chr = (char) sr.Read();
                    currentPos++;

                    switch (chr)
                    {
                        case '\r':
                        case '\n':
                        case '.':
                        case '?':
                        case '!':
                        case ' ':
                            WindowItem token = null;
                            if (String.Empty != word)
                            {
                                string pureWord = this.TrimSpecialChars(word.ToLowerInvariant());
                                if (string.Empty == pureWord)
                                {
                                    word = String.Empty;
                                    break;
                                }

                                if (!this.dictionary.FindWord(pureWord))
                                {
                                    token = new WindowItem(pureWord, word, currentPos);                                    
                                }
                                else
                                {
                                    token = new WindowItem(pureWord);
                                }

 
                                word = String.Empty;
                            }

                            if (this.HasSentenceEnded(chr))
                            {
                                token = new WindowItem(chr, true);
                            }

                            if (null != token)
                            {
                                window.Add(token);
                                MisspelledWord error = window.GetMisspelledWord();
                                if (null != error)
                                {
                                    misspelledWords.Add(error);
                                }
                            }

                            break;

                        default:
                            word += chr;
                            break;

                    }


                }
            }

            return misspelledWords;
        }

    }

}
