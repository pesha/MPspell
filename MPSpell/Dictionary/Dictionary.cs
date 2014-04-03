using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionary
{

    public class Dictionary : List<string>
    {

        public string FileName { get; set; }
        public string SuffixFileName { get; set; }
        public string Location { get; set; }

        public List<string> OtherFiles = new List<string>();


    }

}
