using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{

    public class DictionaryManager
    {

        private string dictionaryFolder;

        public DictionaryManager(string dictionaryFolder)
        {
            this.dictionaryFolder = dictionaryFolder;
        }

        public List<Dictionary> GetAvailableDictionaries()
        {
            List<Dictionary> dictionaries = new List<Dictionary>();

            DirectoryInfo dictInfo = new DirectoryInfo(this.dictionaryFolder);
            List<DirectoryInfo> directories = new List<DirectoryInfo>(dictInfo.EnumerateDirectories());
            foreach (DirectoryInfo info in directories)
            {
                if (info.Name.Contains("_"))
                {
                    Dictionary dictionary = this.GetDictionary(info.Name);
                    dictionaries.Add(dictionary);  
                }
            }

            return dictionaries;
        }

        public Dictionary GetDictionary(string name)
        {
            DirectoryInfo dir = new DirectoryInfo(dictionaryFolder + "/" + name);

            List<FileInfo> files = new List<FileInfo>(dir.EnumerateFiles());

            Dictionary dictionary = new Dictionary();
            dictionary.Location = dictionaryFolder + "/" + name;
            foreach (FileInfo file in files)
            {
                switch (file.Name.Substring(file.Name.Length - 4))
                {
                    case ".dic":
                        dictionary.FileName = file.Name;
                        break;

                    case ".aff":
                        dictionary.SuffixFileName = file.Name;
                        break;

                    case ".mtr":
                        dictionary.ConfusionMatrixesFiles.Add(file.Name);
                        break;

                    default:
                        dictionary.OtherFiles.Add(file.Name);
                        break;
                }

            }

            return dictionary;
        }


    }

}
