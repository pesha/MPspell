using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance
{
    class DamerauLevensteinEditDistance
    {

        static string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };


        public static List<string> GenerateSubtitutions(string word)
        {
            List<string> result = new List<string>();

            // substitution
            for (int i = 0; i < word.Length; i++)
            {
                foreach (string charItem in alphabet)
                {
                    result.Add(String.Copy(word).Remove(i, 1).Insert(i, charItem));
                }
            }

            // deletions
            for (int i = 0; i < word.Length; i++)
            {
                result.Add(String.Copy(word).Remove(i, 1));
            }


            // insertions
            for (int i = 0; i <= word.Length; i++)
            {
                foreach (string item in alphabet)
                {
                    result.Add(String.Copy(word).Insert(i, item));
                }
            }


            // transposition
            for (int i = 0; i < word.Length - 1; i++)
            {
                String newString = String.Copy(word);
                String charItem = newString[i].ToString();
                result.Add(newString.Remove(i, 1).Insert(i + 1, charItem));
            }

            return result;
        }


    }
}
