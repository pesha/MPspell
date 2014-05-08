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
        private StreamWriter writerCorrected;

        public CorrectionStatitic(string fileAll, string fileCorrected)
        {
            FileStream stream = new FileStream(fileAll, FileMode.Create, FileAccess.Write);
            writer = new StreamWriter(stream, Encoding.UTF8);

            FileStream streamCor = new FileStream(fileCorrected, FileMode.Create, FileAccess.Write);
            writerCorrected = new StreamWriter(streamCor, Encoding.UTF8);
        }

        public void AddCorrection(MisspelledWord error)
        {
            writer.WriteLine(error.WrongWord + ";" + error.CorrectWord + ";" + error.RevokedByLm.ToString());

            if (!String.IsNullOrEmpty(error.CorrectWord))
            {
                writerCorrected.WriteLine(error.WrongWord + ";" + error.CorrectWord + ";" + error.Accuracy.ToString() + ";" + error.CorrectedBy.ToString() +";" + error.IsName().ToString());
            }
        }


    }

}
