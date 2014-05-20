using MPSpellCorrector.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpellCorrector.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {

        private MPSpellCorrector.Class.Settings settings;

        private string reportsPath = null;

        public SettingsViewModel(MPSpellCorrector.Class.Settings settings)
        {
            this.settings = settings;
        }

        public string ReportsPath
        {
            get
            {
                return settings.ReportFolder;
            }

            set
            {
                reportsPath = settings.ReportFolder;
                settings.ReportFolder = value;                

                OnPropertyChanged("ReportsPath");
            }
        }

        public bool ExportContext
        {
            get
            {
                return settings.ExportContext;
            }

            set
            {
                settings.ExportContext = value;
                OnPropertyChanged("ExportContext");
            }
        }

        public string CustomDictionariesPath
        {
            get
            {
                return settings.CustomDictionariesFolder;
            }

            set
            {
                settings.CustomDictionariesFolder = value;
                OnPropertyChanged("CustomDictionariesPath");
            }
        }

        public void Save()
        {
            settings.SaveSettings();
        }

        public void Cancel()
        {
            if (null != reportsPath)
                ReportsPath = reportsPath;
        }

    }
}
