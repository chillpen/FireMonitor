using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FireMonitor.SHPOperator
{
    public class ShpOperator
    {
        public bool ReadShpFile(string fileName)
        {
            if (!File.Exists(fileName))
                return false;


            FileStream fs = File.OpenRead(fileName);

            BinaryReader binReader = new BinaryReader(fs);

            long length = fs.Length;

            binReader.ReadBytes(36);

            double xmin, ymin, xmax, ymax;

            xmin = binReader.ReadDouble();
            ymax = binReader.ReadDouble();
            xmax = binReader.ReadDouble();
            ymin = binReader.ReadDouble();


            //int head = fs.r

            return true;
        }
    }
}
