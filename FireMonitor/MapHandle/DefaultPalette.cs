using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace MapHandle
{
    class DefaultPalette:IPalette
    {
        public System.Drawing.Color GetColor(int value)
        {
            System.Drawing.Color color;
            
            int min = 0;
            int max = 65535;

            int inter = 255*(value - min) / (max - min) ;


            color = System.Drawing.Color.FromArgb(((System.Byte)(inter)), ((System.Byte)(inter)), ((System.Byte)(inter)));
            return color;
        }
    }
}
