using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MapHandle
{
    public interface IBorderDataProvider
    {
        List<PointF> PolygonPts { get; }

        List<PointF> PolyLinePts { get; }

        List<PointF> Points { get; }
    }

}
