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
using FireMonitor.FirePoints;

namespace FireMonitor.Data
{
    public struct GridPt
    {
        public int X;
        public int Y;

    }

    public class FY3AVirrDataCreator : IDataCreator
    {



        public FY3AVirrDataCreator()
        {
            m_ImgDataProvider = new FY3AVirrImgDataProvider(m_L1Datafile);
            m_BorderDataProvider = new FY3AVirrBorderDataProvider(m_L1Datafile);
        }

        private FY3AVirrL1DataFile m_L1Datafile = new FY3AVirrL1DataFile();

        private FirePointsCalculate m_firePtsCal = new FirePointsCalculate();

        
        private IImageDataProvider m_ImgDataProvider = null;

        public IImageDataProvider ImageDataProvider
        {
            get { return m_ImgDataProvider; }
        }


        private IBorderDataProvider m_BorderDataProvider = null;

        public IBorderDataProvider BorderDataProvider
        {
            get { return m_BorderDataProvider; }
        }


        private string m_L1file;
        public string L1File
        {
            set
            {

                m_L1file = value;

                m_L1Datafile.ReadData(m_L1file);

            }
        }






    }


}
