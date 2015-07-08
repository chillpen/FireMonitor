using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapHandle
{
    public interface IPalette
    {
        System.Drawing.Color GetColor(int value);
    }
}
