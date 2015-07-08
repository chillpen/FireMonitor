using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MapHandle.Selection
{
    class SelectImageData
    {
        
        public short[] getSelectData(List<PointF> pts, int dataCount, MapHandle.config cfg, double pixelsize)
        {
            //pixelsize,每个像素的地理坐标宽度
            
            int num = 0;
            short[] selectData = new short[dataCount];

            //Initialization 
            for (int i = 0; i < dataCount; i++)
            {
                selectData[i] = 0;
            }

            //Calculation
            for (int i = 0; i < pts.Count(); i++)
            {
                double step = pixelsize / cfg.resolution;
                double m = (cfg.maxY - pts[i].Y) / cfg.resolution;//行
                double n = (pts[i].X - cfg.minX) / cfg.resolution;//列

                //double m = (pts[i].X - p_0.X) * step;
                //double n = (pts[i].Y - p_0.Y) * step;

                //double m = (pts[i].X - p_0.X);
                //double n = (pts[i].Y - p_0.Y);

                for (int j = 0; j < step; j++)
                {
                    for (int k = 0; k < step; k++)
                    {
                        if ((m + j) >= 0 && (m + j) < cfg.height && (n + k) >= 0 && (n + k) < cfg.width)
                        {
                            //int index = (int)Math.Round((m + j) * cfg.width + (n + k));
                            int index = (int)(Math.Round(m + j) * cfg.width + Math.Round(n + k));
                            if (index >= 0 && index < dataCount)
                            {
                                selectData[index] = 1;
                                num++;
                            }
                        }
                    }
                }

                //int index = (int)(Math.Round(m) * cfg.width + Math.Round(n));

                //int index = (int)Math.Round(m * cfg.width + n);
                //if (index >= 0 && index < dataCount)
                //{
                //    selectData[index] = 1;
                //    num++;
                //}

                //int index = (int)Math.Round(pts[i].X * cfg.width + pts[i].Y);
                //if (index >= 0 && index < dataCount)
                //{
                //    selectData[index] = 1;
                //    num++;
                //}

               
            }

            return selectData;
        }
    }
}
