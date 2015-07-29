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
    public class FY3AVirrL1DataFile : IDataFile
    {
        private HDFOperator m_hdfOperator = new HDFOperator();

        public FY3AVirrL1DataFile()
        {
            m_EVInfo.type = DataValueType.EMPTY;
        }

        private string m_L1file;


        public event EventHandler DataChangedEvent;
        /// <summary>
        /// 触发数据修改事件
        /// </summary>
        public void Update()
        {
            if (this.DataChangedEvent != null)
                this.DataChangedEvent(this, null);
        }
        

        public bool ReadData(string fileName)
        {
            m_hdfOperator.Close();
            m_L1file = fileName;
            m_hdfOperator.Open(fileName);

            m_EVInfo = m_hdfOperator.GetDatasetInfo("EV_RefSB", "/");
            m_EV = ReadEV();

            m_lonInfo = m_hdfOperator.GetDatasetInfo("Longitude", "/");
            m_lon = ReadLon();

            m_latInfo = m_hdfOperator.GetDatasetInfo("Latitude", "/");
            m_lat = ReadLat();

            m_calcoef = ReadCalcoefAttr();

            this.Update();

            return true;
        }

        private int[, ,] m_EV;
        public int[, ,] EV
        {
            get { return m_EV; }
        }


        private DatasetInfo m_EVInfo = new DatasetInfo();
        public DatasetInfo EVInfo
        {
            get { return m_EVInfo; }
        }

        // private DatasetInfo
        private int[, ,] ReadEV()
        {

            int[, ,] datasetInt = new int[m_EVInfo.band, m_EVInfo.row, m_EVInfo.col];

            m_hdfOperator.GetDataset("EV_RefSB", "/", datasetInt, m_EVInfo.type);

            return datasetInt;
        }

        private float[, ,] m_lon;
        public float[, ,] Longitude
        {
            get { return m_lon; }
        }


        private DatasetInfo m_lonInfo = new DatasetInfo();
        public DatasetInfo LongitudeInfo
        {
            get { return m_lonInfo; }

        }
        private float[, ,] ReadLon()
        {
           // m_lonInfo = m_hdfOperator.GetDatasetInfo("Longitude", "/");

            float[, ,] datasetFloat = new float[m_lonInfo.band, m_lonInfo.row, m_lonInfo.col];

            m_hdfOperator.GetDataset("Longitude", "/", datasetFloat, m_lonInfo.type);

            return datasetFloat;
        }

        private float[, ,] m_lat;

        public float[, ,] Latitude
        {
            get { return m_lat; }
        }

        private DatasetInfo m_latInfo = new DatasetInfo();
        public DatasetInfo LatitudeInfo
        {
            get { return m_latInfo; }


        }

        private float[, ,] ReadLat()
        {
            m_latInfo = m_hdfOperator.GetDatasetInfo("Latitude", "/");

            float[, ,] datasetFloat = new float[m_latInfo.band, m_latInfo.row, m_latInfo.col];

            m_hdfOperator.GetDataset("Latitude", "/", datasetFloat, m_latInfo.type);

            return datasetFloat;
        }

        private AttributeValue m_calcoef = new AttributeValue();
        public AttributeValue CalCoef
        {
            get { return m_calcoef; }
        }


        private AttributeValue ReadCalcoefAttr()
        {
            return m_hdfOperator.GetAttribute("RefSB_Cal_Coefficients");
        }

        private AttributeValue m_emisOffset = new AttributeValue();
        private AttributeValue ReadEmisOffset()
        {
            return m_hdfOperator.GetAttribute("Emissive_Radiance_Offsets");
        }

        private AttributeValue m_emisScale = new AttributeValue();
        private AttributeValue ReadEmisScale()
        {
            return m_hdfOperator.GetAttribute("Emissive_Radiance_Scales");
        }

    }
}
