﻿using MPSpell.Dictionaries;
using MPSpell.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Check
{

    public class FileChecker : Checker, IDisposable
    {

        private StreamReader reader = null;
        public string Path { get; private set; }
        public bool EndOfCheck
        {
            get
            {
                return (contextLeft == 0);
            }
        }

        private int contextLeft;
        private long position;
        private long fileSize;
        private FileInfo fileInfo;

        public FileChecker(string path, Dictionary dictionary, int contextSize = 2)
            : base(dictionary, contextSize)
        {
            Path = path;
            contextLeft = contextSize;
        }

        private void Init()
        {            
            reader = EncodingDetector.GetStreamWithEncoding(Path);
            position = 0;
            fileInfo = new FileInfo(Path);
            fileSize = fileInfo.Length;
        }

        public double EstimateProcess()
        {
            double res = (double) position / fileSize;
            return res > 1 ? 1 : res;
        }

        public override MisspelledWord GetNextMisspelling()
        {
            if (null == reader)
            {
                this.Init();
            }

            
            MisspelledWord misspelling = null;
            char chr;
            while (!reader.EndOfStream)
            {                
                chr = (char) reader.Read();
                position++;
                misspelling = this.tokenizer.HandleChar(chr);
                if (null != misspelling)
                {
                    break;
                }
            }

            if (null == misspelling && contextLeft > 0)
            {
                for (int i = contextLeft; i > 0; i--)
                {
                    contextLeft--;
                    misspelling = this.tokenizer.HandleChar('.', true);
                    if (null != misspelling)
                    {
                        break;
                    }
                }
            }

            return misspelling;
        }

        public void Dispose()
        {
            if (null != reader)
            {
                reader.Dispose();
            }
        }

    }

}
