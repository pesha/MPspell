using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpellCorrector.Class
{
    public class Project
    {

        public Dictionary Dictionary { get; set; }
        public string FolderPath { get; set; }
        public string DestinationPath { get; set; }

    }
}
