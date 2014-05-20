using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpellCorrector.Class
{
    public class Container
    {

        public Project Project { get; set; }

        public Settings Settings = new Settings();
        private DictionaryManager dictManager;        



        public Container()
        {
            Settings.InitSettings();
            dictManager = new DictionaryManager(Settings.DictionariesFolder, Settings.CustomDictionariesFolder);
        }

        public DictionaryManager GetDictionaryManager()
        {
            return dictManager;
        }



    }
}
