using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MapHandle.Selection
{
    class SelectPoint:ISelectRegion
    {
        PointF p1 = new PointF();

        public void setPoint(Coordinate c1)
        {
            p1.X = (float)c1.X;
            p1.Y = (float)c1.Y;
        }
        public List<PointF> getSelectedPts()
        {
            List<PointF> pts = new List<PointF>();
            
            pts.Add(p1);

            return pts;
        }
    }
}
