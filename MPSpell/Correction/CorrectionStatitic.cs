using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{

    class CorrectionStatitic
    {

        private StreamWriter writer;

        public CorrectionStatitic(string file)
        {
            FileStream stream = new FileStream(file, FileMode.Create, FileAccess.Write);
            writer = new StreamWriter(stream, Encoding.UTF8);
        }

        public void AddCorrection(MisspelledWord error)
        {
            writer.WriteLine(error.WrongWord + ": " + error.CorrectWord);
            writer.Flush();
        }


    }

}
