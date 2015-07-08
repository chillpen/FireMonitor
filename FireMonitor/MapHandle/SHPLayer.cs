using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpMap.Layers;
using SharpMap.Data.Providers;
using System.Drawing;


namespace MapHandle
{
    class SHPLayer
    {
        config imgCon = new config();
        Projection pj = new Projection();

        public void getCFG(config inCFG)
        {
            imgCon = inCFG;
            //imgCon.fullPath = "";
        }

        public VectorLayer getSHPLayer(string layername, string fullPath, Color color, int lineWidth, MapHandle.config.ProjPara pjPara)
        {
            VectorLayer vl = new VectorLayer(layername);
            vl.DataSource = new ShapeFile(fullPath);
            vl.Enabled = true;
            vl.Style.Fill = new SolidBrush(Color.Transparent);
            vl.Style.Outline = new Pen(color, 0.4f);
            vl.Style.Line.Width = lineWidth;
            vl.Style.Line.Color = color;
            
            vl.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            vl.Style.EnableOutline = true;

            //设置投影参数
            vl.CoordinateTransformation = pj.getmapTransform(pjPara);
            //vl.Envelope.
            

            return vl;
        }

       

    }
}
