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


    }

}
