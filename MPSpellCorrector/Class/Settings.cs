using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace MPSpellCorrector.Class
{

    public class Settings
    {

        private readonly string AppName = "MPSpellCorrector";

        public string DictionariesFolder
        {
            get
            {
                return MPSpellCorrector.Properties.Settings.Default.DictionariesFolder;
            }

            set
            {
                MPSpellCorrector.Properties.Settings.Default.DictionariesFolder = value;
            }
        }

        public string CustomDictionariesFolder
        {
            get
            {
                return MPSpellCorrector.Properties.Settings.Default.CustomDictionariesFolder;
            }

            set
            {
                MPSpellCorrector.Properties.Settings.Default.CustomDictionariesFolder = value;
            }

        }

        public string DataFolder
        {
            get
            {
                return MPSpellCorrector.Properties.Settings.Default.DataFolder;
            }

            set
            {
                MPSpellCorrector.Properties.Settings.Default.DataFolder = value;
            }
        }

        public string ReportFolder
        {
            get
            {
                return MPSpellCorrector.Properties.Settings.Default.ResultFolder;
            }

            set
            {
                MPSpellCorrector.Properties.Settings.Default.ResultFolder = value;
            }
        }

        public bool ExportContext
        {
            get
            {
                return MPSpellCorrector.Properties.Settings.Default.ExportContext;
            }

            set
            {
                MPSpellCorrector.Properties.Settings.Default.ExportContext = value;
            }
        }

        public void InitSettings()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (String.IsNullOrEmpty(ReportFolder) || !Directory.Exists(ReportFolder))
            {
                string path = documents + @"\" + AppName + @"\Reports";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                this.ReportFolder = path;
            }

            if (String.IsNullOrEmpty(CustomDictionariesFolder) || !Directory.Exists(CustomDictionariesFolder))
            {
                string path = documents + @"\" + AppName + @"\Dictionaries";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                this.CustomDictionariesFolder = path;
            }

            if (String.IsNullOrEmpty(this.DictionariesFolder) || !Directory.Exists(this.DictionariesFolder) || !ContainsFiles(this.DictionariesFolder))
            {
                string dataPath;
                try
                {
                    dataPath = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.DataDirectory;
                }
                catch (Exception e)
                {
                    dataPath = @"C:\dev\git\Pspell\MPSpellCorrector\";
                }

                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string userFilePath = Path.Combine(localAppData, "MPSpellCorrector");

                Extractor ext = new Extractor(dataPath, userFilePath);
                ext.Run();

                this.DictionariesFolder = ext.GetDictionariesPath();
            }
            

            this.SaveSettings();
        }

        private bool ContainsFiles(string path)
        {
            DirectoryInfo dictInfo = new DirectoryInfo(path);
            List<FileInfo> files = new List<FileInfo>(dictInfo.EnumerateFiles());

            return files.Count > 0 ? true : false;
        }

        public void SaveSettings()
        {
            MPSpellCorrector.Properties.Settings.Default.Save();
        }


    }

}
