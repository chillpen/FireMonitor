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


    public class FY3AVirrL1DataProvider : IImageDataProvider, IBorderDataProvider
    {
        private HDFOperator m_hdfOperator = new HDFOperator();

        public System.Drawing.Bitmap GetData()//todo:需要重构为属性模式，将生成图像部分提成函数，修改相应的Iprovider
        {
            //DatasetInfo datasetInfo = m_hdfOperator.GetDatasetInfo("EV_RefSB", "/");

            //int row = datasetInfo.row;
            //int col = datasetInfo.col;
            //int band = datasetInfo.band;
            //int[, ,] datasetInt = new int[band, row, col];

            //m_hdfOperator.GetDataset("EV_RefSB", "/", datasetInt, datasetInfo.type);
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

        private void ReadHdfFile()
        {
            m_lon = ReadLon();
            m_lat = ReadLat();
            m_EV = ReadEV();
        }

        public event EventHandler ImageDataChangedEvent;

        public void DataChange()
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


        private List<PointF> m_PolygonPts = null;

        public List<PointF> PolygonPts { get { return m_PolygonPts; } }

        private List<PointF> m_PolyLinePts = null;

        public List<PointF> PolyLinePts { get { return m_PolyLinePts; } }

        private List<PointF> m_Points = null;

        public List<PointF> Points { get { return m_Points; } }


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

            for (int i = 0; i < polygonCnt; i++)
            {
                Polygon_shape shape = shpOper.Polygons[i];
                //shape.
            }
            return true;
        }

        private RectangleF m_LonLatBox = new RectangleF();

        private RectangleF FixLonLatBox()
        {
            RectangleF lonlatbox = new RectangleF();

            float maxLon = m_lon[0, 0, 0];
            float minLon = m_lon[0, 0, 0];
            float maxLat = m_lat[0, 0, 0];
            float minLat = m_lat[0, 0, 0];

            for (int i = 0; i < m_lonInfo.row; i++)
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

        //private List<bool> m_BorderPts = new List<bool>();

        //private void GetBorder()
        //{
        //    m_lat = ReadLat();
        //    int size = m_lat.Length;

        //    for (int i = 0; i < size; i++)
        //    {

        //    }
        //}


    }


}
