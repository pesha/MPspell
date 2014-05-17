using MPSpellCorrector.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpellCorrector
{
    public class SettingsViewModel
    {

        private MPSpellCorrector.Class.Settings settings;

        public SettingsViewModel(MPSpellCorrector.Class.Settings settings)
        {
            this.settings = settings;
        }

        public string ReportsPath
        {
            get
            {
                return settings.ResultFolder;
            }

            set
            {
                settings.ResultFolder = value;                

                OnPropertyChanged("ReportsPath");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
