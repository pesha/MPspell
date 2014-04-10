using MPSpell.Dictionaries.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MPSpell.Dictionaries
{

    public class DictionaryManager
    {

        private string dictionaryFolder;
        private List<Dictionary> dictionaryCache = null;

        public DictionaryManager(string dictionaryFolder)
        {
            this.dictionaryFolder = dictionaryFolder;
        }

        public List<Dictionary> GetAvailableDictionaries()
        {
            if (null == dictionaryCache)
            {
                dictionaryCache = new List<Dictionary>();

                DirectoryInfo dictInfo = new DirectoryInfo(this.dictionaryFolder);
                List<DirectoryInfo> directories = new List<DirectoryInfo>(dictInfo.EnumerateDirectories());
                foreach (DirectoryInfo info in directories)
                {
                    List<FileInfo> files = new List<FileInfo>(info.EnumerateFiles());
                    foreach (FileInfo file in files)
                    {
                        if (file.Name.ToLowerInvariant() == "dictionary.xml")
                        {
                            dictionaryCache.Add(this.GetDictionary(file, info.FullName));
                        }
                    }
                }
            }

            return dictionaryCache;
        }

        public Dictionary GetDictionary(string name)
        {
            if (null == dictionaryCache)
            {
                dictionaryCache = this.GetAvailableDictionaries();
            }

            Dictionary res = null;
            foreach (Dictionary dictionary in dictionaryCache)
            {
                if (name == dictionary.Name)
                {
                    res = dictionary;
                    break;
                }
            }

            return res;
        }

        protected Dictionary GetDictionary(FileInfo dictionaryXml, string path)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(dictionaryXml.FullName);
            XmlNodeList root = xml.GetElementsByTagName("Dictionary");
            XmlNode node = null;
            if (root.Count > 0)
            {
                node = root.Item(0);
            }

            Dictionary dictionary = null;

            if (null != node)
            {
                DictionaryLoader loader = this.CreateDefaultLoader();
                string name = node.Attributes["locale"].Value;
                dictionary = new Dictionary(loader, name, path);
                foreach (XmlNode file in node.ChildNodes)
                {
                    DictionaryFileType type = DictionaryFileType.Unknown;
                    switch (file.Attributes["type"].Value)
                    {
                        case "Dictionary":
                            type = DictionaryFileType.Dictionary;
                            break;

                        case "Affix":
                            type = DictionaryFileType.Affix;
                            break;

                        case "OneCharFrequences":
                            type = DictionaryFileType.OneCharFrequences;
                            break;

                        case "TwoCharFrequences":
                            type = DictionaryFileType.TwoCharFrequences;
                            break;

                        case "DeletionsMatrix":
                            type = DictionaryFileType.DeletetionsMatrix;
                            break;

                        case "InsertionsMatrix":
                            type = DictionaryFileType.InsertionsMatrix;
                            break;

                        case "TranspositionsMatrix":
                            type = DictionaryFileType.TranspositionsMatrix;
                            break;

                        case "SubstitutionsMatrix":
                            type = DictionaryFileType.SubstitutionsMatrix;
                            break;

                        case "UnigramFrequences":
                            type = DictionaryFileType.UnigramFrequences;
                            break;

                        case "DigramFrequences":
                            type = DictionaryFileType.DigramFrequences;
                            break;

                        case "TrigramFrequences":
                            type = DictionaryFileType.TrigramFrequences;
                            break;
                    }

                    dictionary.AddFile(type, file.InnerText.Trim());
                }

            }

            return dictionary;
        }

        private DictionaryLoader CreateDefaultLoader()
        {
            DictionaryLoader loader = new DictionaryLoader(
                new DefaultDictionaryFileParser(),
                new DefaultAffixFileParser(),
                new ConfusionMatrixParser(),
                new FrequencyVectorParser(),
                new NgramParser()
            );

            return loader;
        }


    }

}
