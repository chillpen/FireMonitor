using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MapHandle.Selection
{
    public class SelectRect:ISelectRegion
    {
        PointF p_start = new PointF();
        PointF p_end = new PointF();
        int width;
        int height;

        //获取矩形框左上、右下角点的大地坐标
        public void setPoints(Coordinate c1,Coordinate c2, int w, int h)
        {           
            p_start.X = (float)c1.X;
            p_start.Y = (float)c1.Y;

            p_end.X = (float)c2.X;
            p_end.Y = (float)c2.Y;

            width = w;
            height = h;
        }

        //获取矩形框中所有的点
        
        public List<PointF> getSelectedPts( )
        {
            List<PointF> pts = new List<PointF>();
            //x y轴步长
            float x_step = (p_end.X - p_start.X) / width;
            float y_step = (p_start.Y - p_end.Y) / height;

            //计算大地坐标 以左下角为原点
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    PointF p1 = new PointF();
                    p1.X = p_start.X + x_step * j;
                    p1.Y = p_end.Y + y_step * i;

                    pts.Add(p1);
                }

            return pts;
        }


    }
}
