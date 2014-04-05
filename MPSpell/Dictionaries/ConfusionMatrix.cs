using MPSpell.Correction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Dictionaries
{
    public class ConfusionMatrix
    {

        public EditOperation EditOperation { get; private set; }
        private Dictionary<char, Dictionary<char, int>> matrix = new Dictionary<char, Dictionary<char, int>>();

        public ConfusionMatrix(EditOperation operation)
        {
            EditOperation = operation;
        }

        public int GetValue(char rowKey, char columnKey)
        {
            return matrix[rowKey][columnKey];
        }

        public void SetValue(char rowKey, char columnKey, int value)
        {
            if (matrix.ContainsKey(rowKey))
            {

                if (matrix[rowKey].ContainsKey(columnKey))
                {
                    matrix[rowKey][columnKey] = value;
                }
                else
                {
                    matrix[rowKey].Add(columnKey, value);
                }

            }
            else
            {
                Dictionary<char, int> innerDict = new Dictionary<char,int>();
                innerDict.Add(columnKey, value);

                matrix.Add(rowKey, innerDict);
            }
        }

    }
}
