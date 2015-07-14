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
    public class FY3AVirrL1DataProvider : IDataProvider
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
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(row, col,PixelFormat.Format24bppRgb);



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
                        int pixelCount = pixel << 2;
                        byte pixelValue = (byte)(datasetInt[0, i, j] * 255 / 4096);
                        *(p + pixelCount) = pixelValue;		//R
                        *(p + pixelCount + 1) = pixelValue;	//G
                        *(p + pixelCount + 2) = pixelValue;	//B

                        pixel++;
                    }
                }
            }


            return bmp;
        }

        public event EventHandler DataChanged;

        private string m_file;
        public string File
        {
            set
            {
                m_hdfOperator.Close();
                m_file = value;
                m_hdfOperator.Open(m_file);
            }
        }



    }
}
