using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionary.Affixes
{

    public class Condition : List<char>
    {

        public bool Except { get; set; }

    }

    public enum RuleType
    {
        PFX,
        SFX,
        Unknown,
    }

    public enum Combinable
    {
        Y,
        N
    }

}
