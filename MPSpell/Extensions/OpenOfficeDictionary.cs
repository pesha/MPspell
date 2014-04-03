using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MPSpell.Extensions
{
    public class OpenOfficeDictionaries
    {

        private static readonly string DescriptionFile = "dictionaries.xcu";
        private static readonly string SpellDictionary = "DICT_SPELL";

        private string file;
        private List<OpenOfficeDictionaryItem> dictionaries = null;

        public OpenOfficeDictionaries(string file)
        {
            this.file = file;
        }

        public List<OpenOfficeDictionaryItem> GetAvailableDictionaries()
        {
            if (null == dictionaries)
            {
                this.dictionaries = ParseDescriptionXML(this.file);
            }

            return this.dictionaries;
        }        

        internal List<string> ExtractFiles(string[] files, string path)
        {
            List<string> extractedFiles = new List<string>();
            ZipArchive zip = ZipFile.Open(this.file, ZipArchiveMode.Read);
            
            foreach(string file in files){
                ZipArchiveEntry entry = zip.GetEntry(file);
                string pathToFile = path + file;
                entry.ExtractToFile(pathToFile);
                extractedFiles.Add(pathToFile);
            }
            
            zip.Dispose();

            return extractedFiles;
        }

        private List<OpenOfficeDictionaryItem> ParseDescriptionXML(string file)
        {
            ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read);
            ZipArchiveEntry description = null;
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (entry.Name.ToLowerInvariant() == DescriptionFile)
                {
                    description = entry;
                    break;
                }
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(description.Open());
            XmlNodeList list = xml.GetElementsByTagName("node");
            XmlNodeList dictionaries = null;
            foreach (XmlNode node in list)
            {
                if (node.Attributes["oor:name"].Value == "Dictionaries")
                {
                    if (node.HasChildNodes)
                    {
                        dictionaries = node.ChildNodes;
                        break;
                    }
                }
            }

            List<OpenOfficeDictionaryItem> dictionaryList = new List<OpenOfficeDictionaryItem>();
            foreach (XmlNode node in dictionaries)
            {
                string type = "";
                string name = "";
                string locales = "";
                string locations = "";
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    name = childNode.Attributes["oor:name"].Value;

                    switch (childNode.Attributes["oor:name"].Value.ToLowerInvariant())
                    {
                        case "locations":
                            locations = childNode.FirstChild.InnerText;
                            break;

                        case "format":
                            type = childNode.FirstChild.InnerText;
                            break;

                        case "locales":
                            locales = childNode.FirstChild.InnerText;
                            break;
                    }

                }


                if (type == SpellDictionary)
                {
                    dictionaryList.Add(new OpenOfficeDictionaryItem(name, locations, locales, this));
                }
            }

            zip.Dispose();

            return dictionaryList;
        }

    }

    public class OpenOfficeDictionaryItem
    {

        public string Name { get; private set; }
        public string[] Locations { get; private set; }
        public string[] Locales { get; private set; }

        private OpenOfficeDictionaries dictionaryExtension;

        public OpenOfficeDictionaryItem(string name, string locations, string locales, OpenOfficeDictionaries dictionaryExtension)
        {
            Name = name;
            locations = locations.Replace("%origin%/", "");
            Locations = locations.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Locales = locales.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            this.dictionaryExtension = dictionaryExtension;
        }

        public bool IsValidDictionary()
        {
            throw new NotImplementedException();
        }

        public List<string> ExtractFiles(string path)
        {
            return dictionaryExtension.ExtractFiles(this.Locations, path);
        }
    

    }

}
