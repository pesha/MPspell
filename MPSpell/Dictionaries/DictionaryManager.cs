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
        private string customDictionaryFolder;
        private List<Dictionary> dictionaryCache = null;

        public DictionaryManager(string dictionaryFolder, string customDictionaryFolder = null)
        {
            this.dictionaryFolder = dictionaryFolder;
            this.customDictionaryFolder = customDictionaryFolder;
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

                if (null != customDictionaryFolder)
                {
                    dictInfo = new DirectoryInfo(this.customDictionaryFolder);
                    directories = new List<DirectoryInfo>(dictInfo.EnumerateDirectories());
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
                XmlElement el = node as XmlElement;

                string name = node.Attributes["locale"].Value;
                char[] alphabet = node.Attributes["alphabet"].Value.ToCharArray();
                char[] specialChars = null;
                Dictionary<char, List<char>> accentPairs = null;
                string regex = null;
                if (el.HasAttribute("allowedSpecialChars"))
                {
                    specialChars = node.Attributes["allowedSpecialChars"].Value.ToCharArray();
                }
                if (el.HasAttribute("wordBoundaryRegex"))
                {
                    regex = node.Attributes["wordBoundaryRegex"].Value;
                }
                if (el.HasAttribute("accentPairs"))
                {
                    string[] pairs = node.Attributes["accentPairs"].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    accentPairs = this.ParsePairs(pairs);
                }
                dictionary = new Dictionary(loader, name, path, alphabet, specialChars, regex, accentPairs);
                foreach (XmlNode file in node.ChildNodes)
                {
                    DictionaryFileType type = DictionaryFileType.Unknown;
                    switch (file.Attributes["type"].Value)
                    {
                        case "LineDictionary":
                            type = DictionaryFileType.LineDictionary;
                            break;

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

        protected Dictionary<char, List<char>> ParsePairs(string[] pairs)
        {
            Dictionary<char, List<char>> result = new Dictionary<char, List<char>>();
            foreach (string pair in pairs)
            {
                string[] data = pair.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (result.ContainsKey(data[0][0]))
                {
                    result[data[0][0]].Add(data[1][0]);
                }
                else
                {
                    result.Add(data[0][0], new List<char>() { data[1][0] });
                }
            }

            return result;
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
