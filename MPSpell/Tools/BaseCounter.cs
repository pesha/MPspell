using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Tools
{
    public abstract class BaseCounter<T>
    {

        protected Dictionary<T, int> frequences = new Dictionary<T, int>();

        protected void HandleOutput(T value)
        {
            if (frequences.ContainsKey(value))
            {
                frequences[value]++;
            }
            else
            {
                frequences.Add(value, 1);
            }
        }

        public IEnumerator<string> GetFrequencesString()
        {
            foreach(KeyValuePair<T, int> item in frequences){
                yield return item.Key.ToString() + "," + item.Value.ToString();
            }
        }

        public Dictionary<T, int> GetFrequences()
        {
            return frequences;
        }
        
        public void Save(string file)
        {
            using (FileStream stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    IEnumerator<string> lines = this.GetFrequencesString();
                    while (lines.MoveNext())
                    {
                        writer.WriteLine(lines.Current);
                    }
                }
            }             
        }

    }
}
