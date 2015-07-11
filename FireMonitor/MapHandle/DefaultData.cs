using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapHandle
{
    class DefaultData:IDataProvider
    {
        //public System.Drawing.Bitmap[] bmp;
        public System.Drawing.Bitmap GetData()
        {
            int width = 4100;
            int height = 1823;
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);


            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    uint value = (uint)((i * width + j)/10000);
                    System.Drawing.Color color = System.Drawing.Color.FromArgb((byte)((value >> 24) & 0xFF), (byte)((value >> 16) & 0xFF), (byte)((value >> 8) & 0xFF), (byte)(value & 0xFF));
                    bmp.SetPixel(j, i, color);
                }
            return bmp;
        }

        public event EventHandler DataChanged;
       
    }
}
