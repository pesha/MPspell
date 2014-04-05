using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{
    public class FrequencyVector<T> : Dictionary<T, int>
    {

    }

    public enum FrequencyVectorType
    {
        OneChar,
        TwoChar        
    }

}
