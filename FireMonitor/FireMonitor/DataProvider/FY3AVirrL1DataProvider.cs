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

namespace FireMonitor.DataProvider
{
    public struct GridPt
    {
        public int X;
        public int Y;

    }

    public class FY3AVirrL1DataProvider : IImageDataProvider, IBorderDataProvider
    {
        private HDFOperator m_hdfOperator = new HDFOperator();

        private System.Drawing.Bitmap CreateImageData()//todo:需要重构为属性模式，将生成图像部分提成函数，修改相应的Iprovider
        {

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m_EVInfo.col, m_EVInfo.row, PixelFormat.Format24bppRgb);



            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            unsafe
            {
                byte* p = (byte*)ptr;
                int pixel = 0;

                for (int i = 0; i < m_EVInfo.row; i++)
                {
                    for (int j = 0; j < m_EVInfo.col; j++)
                    {
                        int pixelCount = pixel * 3;
                        byte pixelValue = (byte)(m_EV[1, i, j] * 255 / 1024);
                        *(p + pixelCount) = pixelValue;		//R
                        *(p + pixelCount + 1) = pixelValue;	//G
                        *(p + pixelCount + 2) = pixelValue;	//B

                        pixel++;
                    }
                }
            }
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        private System.Drawing.Bitmap m_Image = new Bitmap(1, 1);
        public System.Drawing.Bitmap Image
        {
            get
            {
                m_Image = this.CreateImageData();
                return m_Image;
            }
        }

        private void ReadHdfFile()
        {
            m_lon = ReadLon();
            m_lat = ReadLat();
            m_EV = ReadEV();
        }

        public event EventHandler ImageDataChangedEvent;

        public void OnDataChange()
        {
            if (this.ImageDataChangedEvent != null)
                this.ImageDataChangedEvent(this, null);
        }

        private string m_L1file;
        public string L1File
        {
            set
            {
                m_hdfOperator.Close();
                m_L1file = value;
                m_hdfOperator.Open(m_L1file);
                ReadHdfFile();
                CreateBorder();
            }
        }

        private int[, ,] m_EV;


        private DatasetInfo m_EVInfo = new DatasetInfo();
        private int[, ,] ReadEV()
        {
            m_EVInfo = m_hdfOperator.GetDatasetInfo("EV_RefSB", "/");

            int[, ,] datasetInt = new int[m_EVInfo.band, m_EVInfo.row, m_EVInfo.col];

            m_hdfOperator.GetDataset("EV_RefSB", "/", datasetInt, m_EVInfo.type);

            return datasetInt;
        }

        private float[, ,] m_lon;
        private DatasetInfo m_lonInfo = new DatasetInfo();
        private float[, ,] ReadLon()
        {
            m_lonInfo = m_hdfOperator.GetDatasetInfo("Longitude", "/");

            float[, ,] datasetFloat = new float[m_lonInfo.band, m_lonInfo.row, m_lonInfo.col];

            m_hdfOperator.GetDataset("Longitude", "/", datasetFloat, m_lonInfo.type);

            return datasetFloat;
        }

        private float[, ,] m_lat;

        private DatasetInfo m_latInfo = new DatasetInfo();

        private float[, ,] ReadLat()
        {
            m_latInfo = m_hdfOperator.GetDatasetInfo("Latitude", "/");

            float[, ,] datasetFloat = new float[m_latInfo.band, m_latInfo.row, m_latInfo.col];

            m_hdfOperator.GetDataset("Latitude", "/", datasetFloat, m_latInfo.type);

            return datasetFloat;
        }


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

        private bool CreateBorder()
        {
            ShpOperator shpOper = new ShpOperator();
            shpOper.ReadShpFile(m_provinceShpFile);

            int polygonCnt = shpOper.Polygons.Count;

            float gridStep = 0.01f;

            RectangleF lonlatBox = FixLonLatBox();

            int widthPts = (int)(lonlatBox.Width / gridStep) + 1;
            int heightPts = (int)(lonlatBox.Height / gridStep) + 1;


            GridPt[,] grid = new GridPt[widthPts, heightPts];

            for (int ystart = 0; ystart < m_lonInfo.row; ystart++)
            {
                for (int xstart = 0; xstart < m_lonInfo.col; xstart++)
                {
                    int xIndex = (int)((m_lon[0, ystart, xstart] - lonlatBox.X) / gridStep);
                    int yIndex = (int)((m_lat[0, ystart, xstart] - lonlatBox.Y) / gridStep);

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


            m_BorderImgWidth = m_latInfo.col;
            m_BorderImgHeight = m_latInfo.row;
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

            float maxLon = m_lon[0, 0, 0];
            float minLon = m_lon[0, 0, 0];
            float maxLat = m_lat[0, 0, 0];
            float minLat = m_lat[0, 0, 0];

            for (int i = 0; i < m_lonInfo.row; i++)//lonlat的最大值因该在边缘，优化考虑
            {
                for (int j = 0; j < m_lonInfo.col; j++)
                {
                    float lontmp = m_lon[0, i, j];
                    maxLon = Math.Max(maxLon, lontmp);
                    minLon = Math.Min(minLon, lontmp);

                    float lattmp = m_lat[0, i, j];
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
