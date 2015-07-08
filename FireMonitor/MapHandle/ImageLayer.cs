using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace MapHandle
{
    public class ImageLayer
    {
        config imgCon = new config();
        public IPalette Palette { get; set; }
        public IDataProvider dataPro { get; set; }
        public uint[] data;
        
        public void getCFG(config inCFG)
        {
            imgCon = inCFG;
            //imgCon.fullPath = "";
        }
        //生成IMAGE对应的XML文件
        private void imgInfoCreator(string filePath)
        {
            try
            {
                string xstart = imgCon.minX.ToString("E17");
                string ystart = imgCon.maxY.ToString("E17");
                string xresulotin = imgCon.resolution.ToString("E17");
                string yresulotin = ((-1.0) * imgCon.resolution).ToString("E17");
                string xmlfile = filePath + ".aux.xml";
                string value = "";
                value = xstart + ", " + xresulotin + ", 0.0000000000000000e+000, " + ystart + ", 0.0000000000000000e+000," + yresulotin;
                File.Copy("Geo.aux.xml", xmlfile, true);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlfile);//打开xml
                XmlNode xmlPAMD = xmldoc.SelectSingleNode("PAMDataset");
                XmlNode xmlGeo = xmlPAMD.SelectSingleNode("GeoTransform");
                XmlElement xe2 = (XmlElement)xmlGeo;//转换类型
                xe2.InnerText = value;
                xmldoc.Save(xmlfile);
            }
            catch
            { }
        }

        //加载影像
        public GdalRasterLayer getIMGLayer(string layerName, string filePath)
        {
            imgInfoCreator(filePath);
            GdalRasterLayer layer = new GdalRasterLayer(layerName,filePath);
            return layer;
        }

        //加载内存数据
        public GdalRasterLayer getIMGLayerHDF(string layerName)
        {
            Random ran = new Random();
            int RandKey = ran.Next(100, 999);
            string filePath = @".\tmpImg\";
            Directory.CreateDirectory(filePath);
            filePath = filePath + "tmp_" + RandKey.ToString() + ".jpg";

            if (File.Exists(filePath))
                File.Delete(filePath);

            data = new uint[imgCon.width * imgCon.height];
            if(dataPro==null) //使用默认测试数据
            {
                //for (int i = 0; i < imgCon.height; i++)
                //    for (int j = 0; j < imgCon.width; j++)
                //    {
                //        data[i * imgCon.width + j] = (short)((i * imgCon.width + j) / 10);
                //    }

                DefaultData dd = new DefaultData();
                //data = dd.GetData();
            }
            else     //使用传入的数据
            {
                //data = dataPro.GetData();
            }

            Bitmap bmp = new Bitmap(imgCon.width, imgCon.height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
         
            
            for(int i=0;i<imgCon.height;i++)
                for(int j = 0;j<imgCon.width;j++)
                {
                    //if(Palette==null) //使用默认调色板
                    //{
                    //    Color tmpColor = new DefaultPalette().GetColor(data[i * imgCon.width + j]);
                    //    bmp.SetPixel(j, i, tmpColor);
                    //}
                        
                    //else    //使用传入的调色板
                    //{
                    //    bmp.SetPixel(j, i, Palette.GetColor(data[i * imgCon.width + j]));
                    //}
                    //Color color = System.Drawing.Color.FromArgb(((System.Byte)(data[i * imgCon.width + j])), ((System.Byte)(data[i * imgCon.width + j])), ((System.Byte)(data[i * imgCon.width + j])));
                    uint value = data[i * imgCon.width + j];
                    Color color = Color.FromArgb((byte)((value >> 24) & 0xFF), (byte)((value >> 16) & 0xFF),(byte)((value >> 8) & 0xFF),(byte)(value & 0xFF));
                    bmp.SetPixel(j,i,color);
                }
            bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            imgInfoCreator(filePath);
            GdalRasterLayer layer = new GdalRasterLayer(layerName, filePath);
            return layer;
        }

        private Bitmap KiResizeImage(Bitmap bmp, int newW, int newH, Color fillColor)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                b.MakeTransparent(fillColor);

                // b.MakeTransparent(Color.White);//取消注释
                return b;
            }
            catch
            {
                return null;
            }
        }


       
    }
}
