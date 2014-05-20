using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MPSpellCorrector.Class
{
    public class Extractor
    {

        private string path;
        private string dictionariesPath;

        public Extractor(string path, string dictionariesPath)
        {
            this.path = path + @"\Dictionaries";
            this.dictionariesPath = dictionariesPath + @"\DictionariesData";
        }

        private List<FileInfo> FindArchives()
        {
            DirectoryInfo dictInfo = new DirectoryInfo(this.path);
            return new List<FileInfo>(dictInfo.EnumerateFiles());
        }

        public void Run()
        {
            List<FileInfo> files = this.FindArchives();

            foreach (FileInfo file in files)
            {
                using (ZipArchive zip = ZipFile.Open(file.FullName, ZipArchiveMode.Read))
                {
                    string folder = this.dictionariesPath;
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);                        
                    }

                    string name = folder + @"\" +file.Name.Replace(".zip", "");

                    if (!Directory.Exists(name))
                    {
                        zip.ExtractToDirectory(folder);
                    }
                }
            }

        }

        public string GetDictionariesPath()
        {
            return this.dictionariesPath;
        }


    }
}
