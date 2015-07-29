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
    public class FY3AVirrImgDataProvider : IImageDataProvider
    {

        public FY3AVirrImgDataProvider(FY3AVirrL1DataFile l1DataFile)
        {
            m_L1DataFile = l1DataFile;

            m_L1DataFile.DataChangedEvent += new EventHandler(On_DataChangedEvent);
        }

        void On_DataChangedEvent(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            m_Image.Dispose();
            m_Image = this.CreateImageData();

        }

        private FY3AVirrL1DataFile m_L1DataFile = null;

        /// <summary>
        /// 创建图像
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Bitmap CreateImageData()
        {

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m_L1DataFile.EVInfo.col, m_L1DataFile.EVInfo.row, PixelFormat.Format24bppRgb);



            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            float[] calcoef = (float[])(m_L1DataFile.CalCoef.dataValue);



            unsafe
            {
                byte* p = (byte*)ptr;
                int pixel = 0;

                for (int i = 0; i < m_L1DataFile.EVInfo.row; i++)
                {
                    for (int j = 0; j < m_L1DataFile.EVInfo.col; j++)
                    {
                        int pixelCount = pixel * 3;
                        int vr = (int)(m_L1DataFile.EV[0, i, j] * calcoef[0] + calcoef[1]);
                        int vg = (int)(m_L1DataFile.EV[1, i, j] * calcoef[2] + calcoef[3]);
                        int vb = (int)(m_L1DataFile.EV[6, i, j] * calcoef[10] + calcoef[11]);


                        byte pixelValueR = (byte)(vr * 255 / 100);
                        byte pixelValueG = (byte)(vg * 255 / 100);
                        byte pixelValueB = (byte)(vb * 255 / 100);
                        *(p + pixelCount) = pixelValueR;		//R
                        *(p + pixelCount + 1) = pixelValueG;	//G
                        *(p + pixelCount + 2) = pixelValueB;	//B

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
                //m_Image = this.CreateImageData();
                return m_Image;
            }
        }


        public event EventHandler ImageDataChangedEvent;
        /// <summary>
        /// 相应图像数据修改
        /// </summary>
        public void Update()
        {
            if (this.ImageDataChangedEvent != null)
                this.ImageDataChangedEvent(this, null);
        }
    }
}
