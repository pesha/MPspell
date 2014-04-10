using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Extensions
{
    public static class ExtensionsMethods
    {

        public static void ShiftLeft(this BitArray array, bool newValue)
        {
            for (int i = 1; i < array.Count; i++)
            {
                array[i - 1] = array[i];
            }

            array[array.Count - 1] = newValue;
        }

    }
}
