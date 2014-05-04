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

        public string ResultFolder
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


    }

}
