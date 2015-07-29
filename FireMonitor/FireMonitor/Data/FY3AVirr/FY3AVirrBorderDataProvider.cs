using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapHandle;
using HDF5DotNet;
using FireMonitor.HDFOper;
using System.Drawing.Imaging;
using System.Drawing;
using FireMonitor.SHPOperator;
using System.Windows.Forms;

namespace FireMonitor.Data
{
    public class FY3AVirrBorderDataProvider : IBorderDataProvider
    {

        public FY3AVirrBorderDataProvider(FY3AVirrL1DataFile l1DataFile)
        {
            m_L1DataFile = l1DataFile;

            m_L1DataFile.DataChangedEvent += new EventHandler(On_DataChangedEvent);
        }

        void On_DataChangedEvent(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            m_PolygonPts.Clear();

            this.CreateBorder();
        }

        private FY3AVirrL1DataFile m_L1DataFile = null;

        private List<Point[]> m_PolygonPts = new List<Point[]>();

        public List<Point[]> PolygonPts { get { return m_PolygonPts; } }

        private List<Point> m_PolyLinePts = null;

        public List<Point> PolyLinePts { get { return m_PolyLinePts; } }

        private List<Point> m_Points = null;

        public List<Point> Points { get { return m_Points; } }


        private string m_provinceShpFile = Application.StartupPath + "\\province.shp";
        public string ProvinceShpFile
        {
            set { m_provinceShpFile = value; }
        }
        /// <summary>
        /// 创建边界
        /// </summary>
        /// <returns></returns>
        private bool CreateBorder()
        {
            ShpOperator shpOper = new ShpOperator();
            shpOper.ReadShpFile(m_provinceShpFile);

            int polygonCnt = shpOper.Polygons.Count;

            float gridStep = 0.005f;

            RectangleF lonlatBox = FixLonLatBox();

            int widthPts = (int)(lonlatBox.Width / gridStep) + 1;
            int heightPts = (int)(lonlatBox.Height / gridStep) + 1;

            DatasetInfo lonInfo = m_L1DataFile.LongitudeInfo;
            float[, ,] lon = m_L1DataFile.Longitude;
            float[, ,] lat = m_L1DataFile.Latitude;
            DatasetInfo latInfo = m_L1DataFile.LatitudeInfo;

            GridPt[,] grid = new GridPt[widthPts, heightPts];

            for (int ystart = 0; ystart < lonInfo.row; ystart++)
            {
                for (int xstart = 0; xstart < lonInfo.col; xstart++)
                {
                    int xIndex = (int)((lon[0, ystart, xstart] - lonlatBox.X) / gridStep);
                    int yIndex = (int)((lat[0, ystart, xstart] - lonlatBox.Y) / gridStep);

                    grid[xIndex, yIndex].Y = ystart;
                    grid[xIndex, yIndex].X = xstart;
                }
            }


            for (int i = 0; i < polygonCnt; i++)
            {
                Polygon_shape shape = shpOper.Polygons[i];

                RectangleF shapeRect = new RectangleF();
                shapeRect.X = (float)shape.Box[0];
                shapeRect.Y = (float)shape.Box[1];
                shapeRect.Height = (float)shape.Box[3] - (float)shape.Box[1];
                shapeRect.Width = (float)shape.Box[2] - (float)shape.Box[0];

                if (lonlatBox.IntersectsWith(shapeRect))
                {
                    //return true;
                    List<Point> polygonPts = new List<Point>();
                    for (int j = 0; j < shape.NumPoints; j++)
                    {
                        //if(shape.Points[j])
                        int xIndex = (int)((shape.Points[j].X - lonlatBox.X) / gridStep);
                        int yIndex = (int)((shape.Points[j].Y - lonlatBox.Y) / gridStep);

                        if (xIndex <= 0 || yIndex <= 0 || xIndex >= widthPts || yIndex >= heightPts)
                            continue;

                        if (grid[xIndex, yIndex].X == 0 && grid[xIndex, yIndex].Y == 0)
                            continue;

                        polygonPts.Add(new Point(grid[xIndex, yIndex].X, grid[xIndex, yIndex].Y));
                        //m_PolygonPts.

                        //grid[xIndex,yIndex]
                    }

                    if (polygonPts.Count > 3)
                        m_PolygonPts.Add(polygonPts.ToArray());

                }

                //shape.
            }


            m_BorderImgWidth = latInfo.col;
            m_BorderImgHeight = latInfo.row;
            return false;
        }

        private int m_BorderImgWidth = 0;

        private int m_BorderImgHeight = 0;

        public int BorderImgWidth
        {
            get { return m_BorderImgWidth; }
        }

        public int BorderImgHeight
        {
            get { return m_BorderImgHeight; }
        }
        //private RectangleF m_LonLatBox = new RectangleF();

        private RectangleF FixLonLatBox()
        {
            RectangleF lonlatbox = new RectangleF();
            DatasetInfo lonInfo = m_L1DataFile.LongitudeInfo;
            float[, ,] lon = m_L1DataFile.Longitude;
            float[, ,] lat = m_L1DataFile.Latitude;
            DatasetInfo latInfo = m_L1DataFile.LatitudeInfo;

            float maxLon = lon[0, 0, 0];
            float minLon = lon[0, 0, 0];
            float maxLat = lat[0, 0, 0];
            float minLat = lat[0, 0, 0];

            for (int i = 0; i < lonInfo.row; i++)//lonlat的最大值因该在边缘，优化考虑
            {
                for (int j = 0; j < lonInfo.col; j++)
                {
                    float lontmp = lon[0, i, j];
                    maxLon = Math.Max(maxLon, lontmp);
                    minLon = Math.Min(minLon, lontmp);

                    float lattmp = lat[0, i, j];
                    maxLat = Math.Max(maxLat, lattmp);
                    minLat = Math.Min(minLat, lattmp);
                }
            }


            //目前考虑的中国区域不涉及到经纬度边界的问题，回头再修正边界问题
            lonlatbox.Y = minLat;
            lonlatbox.X = minLon;
            lonlatbox.Width = maxLon - minLon;
            lonlatbox.Height = maxLat - minLat;




            return lonlatbox;
        }

    }
}
