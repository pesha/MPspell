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

        private Settings settings = new Settings();
        private DictionaryManager dictManager;        

        public Container()
        {
            dictManager = new DictionaryManager(settings.DictionariesFolder);
        }

        public DictionaryManager GetDictionaryManager()
        {
            return dictManager;
        }



    }
}
