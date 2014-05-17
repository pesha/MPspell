using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpellCorrector.Class
{

    public class Settings
    {

        public string DictionariesFolder
        {
            get
            {
                return MPSpellCorrector.Properties.Settings.Default.DictionariesFolder;
            }

            set
            {
                MPSpellCorrector.Properties.Settings.Default.DictionariesFolder = value;
                MPSpellCorrector.Properties.Settings.Default.Save();
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
                MPSpellCorrector.Properties.Settings.Default.Save();
            }
        }

        public string ResultFolder
        {
            get
            {
                return MPSpellCorrector.Properties.Settings.Default.ResultFolder;
            }

            set
            {
                MPSpellCorrector.Properties.Settings.Default.ResultFolder = value;
                MPSpellCorrector.Properties.Settings.Default.Save();
            }
        }


    }

}
