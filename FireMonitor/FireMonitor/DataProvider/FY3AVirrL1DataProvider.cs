using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapHandle;
using HDF5DotNet;
using FireMonitor.HDFOper;


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
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(row,col);

            bmp.

            for(int i =0;i<row;i++)
            {
                for (int j = 0; j < col; j++)
                {
                    //bmp.SetPixel(row, col, datasetInt[0, i, j]);
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
