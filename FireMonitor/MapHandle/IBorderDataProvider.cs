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
        List<Point[]> PolygonPts { get; }

        List<Point> PolyLinePts { get; }

        List<Point> Points { get; }

        int BorderImgWidth { get; }
        int BorderImgHeight { get; }
    }

}
