using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MapHandle.Selection
{
    public interface ISelectRegion
    {
        List<PointF> getSelectedPts();
    }
}
