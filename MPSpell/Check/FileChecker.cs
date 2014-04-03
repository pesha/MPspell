using MPSpell.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{

    public class FileChecker : Checker 
    {

        
        public FileChecker(MPSpell.Dictionary.Dictionary dict)
            : base(dict)
        {
        }

        public List<Ngram> CheckFile(string file)
        {
            List<Ngram> misspelledWords = new List<Ngram>();

            Ngram ngram = new Ngram();            

            // doresit kodovani respektive detekci
            uint currentPos = 0;
            using (StreamReader sr = EncodingDetector.GetStreamWithEncoding(file))
            {                
                string word = "";
                char? lastChar = null;                
                while(!sr.EndOfStream){
                    char chr = (char) sr.Read();
                    currentPos++;
                                       
                    lastChar = chr;
                    if (lastChar == '\r' || lastChar == '\n')
                    {
                        chr = ' ';
                    }

                    if (' ' != chr)
                    {
                        word += chr;                        
                    }
                    else if (word != "")
                    {                                         
                        string trimedWord = this.TrimSpecialChars(word);
                        ngram.Add(trimedWord);
                        bool res = this.CheckWord(trimedWord);
                        if (!res)
                        {
                            Ngram error = ngram.DeepCopy(word, currentPos);                            
                            misspelledWords.Add(error);
                        }
                        word = "";
                    }
                }
            }

            return misspelledWords;
        }

    }

}
