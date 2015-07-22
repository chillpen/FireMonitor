using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapHandle;
using HDF5DotNet;
using FireMonitor.HDFOper;
using System.Drawing.Imaging;
using System.Drawing;

namespace FireMonitor.DataProvider
{
    public class FY3AVirrL1DataProvider : IImageDataProvider
    {
        private HDFOperator m_hdfOperator = new HDFOperator();

        public System.Drawing.Bitmap GetData()
        {
            DatasetInfo datasetInfo = m_hdfOperator.GetDatasetInfo("EV_RefSB", "/");

            int row = datasetInfo.row;
            int col = datasetInfo.col;
            int band = datasetInfo.band;
            int[, ,] datasetInt = new int[band, row, col];

            m_hdfOperator.GetDataset("EV_RefSB", "/", datasetInt, datasetInfo.type);
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(col, row, PixelFormat.Format24bppRgb);



            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            unsafe
            {
                byte* p = (byte*)ptr;
                int pixel = 0;

                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        int pixelCount = pixel * 3;
                        byte pixelValue = (byte)(datasetInt[1, i, j] * 255 / 1024);
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
            }
        }

        private float[, ,] m_lon;
        private float[, ,] ReadLon()
        {
            DatasetInfo datasetInfo = m_hdfOperator.GetDatasetInfo("Longitude", "/");

            int row = datasetInfo.row;
            int col = datasetInfo.col;
            int band = datasetInfo.band;
            float[, ,] datasetFloat = new float[band, row, col];

            m_hdfOperator.GetDataset("Longitude", "/", datasetFloat, datasetInfo.type);

            return datasetFloat;
        }

        private float[, ,] m_lat;
        private float[, ,] ReadLat()
        {
            DatasetInfo datasetInfo = m_hdfOperator.GetDatasetInfo("Latitude", "/");

            int row = datasetInfo.row;
            int col = datasetInfo.col;
            int band = datasetInfo.band;
            float[, ,] datasetFloat = new float[band, row, col];

            m_hdfOperator.GetDataset("Latitude", "/", datasetFloat, datasetInfo.type);

            return datasetFloat;
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
