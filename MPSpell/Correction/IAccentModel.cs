using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{
    public interface IAccentModel
    {

        List<string> AddAccent(string word);

    }

}
