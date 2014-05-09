using MPSpell.Extensions;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Correction
{

    public class CorrectionStatitic
    {

        public string File { get; private set; }
        public string FileCorrected { get; private set; }
        public bool ExportContext { get; private set; }

        private StreamWriter writer;
        private StreamWriter writerCorrected;

        public CorrectionStatitic(string fileAll = null, string fileCorrected = null, bool exportContext = false)
        {
            this.File = (null != fileAll) ? fileAll : Path.GetTempFileName();
            this.FileCorrected = (null != fileCorrected) ? fileCorrected : Path.GetTempFileName();
            this.ExportContext = exportContext;

            FileStream stream = new FileStream(this.File, FileMode.Create, FileAccess.Write);
            writer = new StreamWriter(stream, Encoding.UTF8);

            FileStream streamCor = new FileStream(this.FileCorrected, FileMode.Create, FileAccess.Write);
            writerCorrected = new StreamWriter(streamCor, Encoding.UTF8);
        }

        public void AddCorrection(MisspelledWord error)
        {
            string context = ExportContext ? ";" + error.GetLeftContext().ToStringRepresentation() + ";" + error.GetRightContext().ToStringRepresentation() : ""; 
            writer.WriteLine(error.WrongWord + ";" + error.CorrectWord + ";" + error.RevokedByLm.ToString() + context);

            if (!String.IsNullOrEmpty(error.CorrectWord))
            {
                writerCorrected.WriteLine(error.WrongWord + ";" + error.CorrectWord + ";" + Math.Round(error.Accuracy,1).ToString() + ";" + error.CorrectedBy.ToString() +";" + error.IsName().ToString());
            }
        }

        public void Close()
        {
            writer.Close();
            writerCorrected.Close();
        }



    }

}
