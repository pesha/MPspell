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
        public string CustomDictionary { get; set; }

        public string SourcePath { get; set; }
        public string[] SourceFiles { get; set; }
        public string DestinationPath { get; set; }

        
        public string ReportPath { get; set; }
        public bool PreserveSubfolders { get; set; }


        public Project()
        {
            PreserveSubfolders = true;
        }


    }
}
