using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDF5DotNet;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace FireMonitor.HDFOper
{
    public enum HdfObjectType
    {
        Dataset,
        Attribute
    }


    public enum DataValueType
    {
        STRING,
        INT,
        FLOAT,
        COMPOUND,
        EMPTY
    }
    public class HDFOperator
    {
        private H5FileId m_fileId;

        private HdfAttributeOperator m_attributeOper = new HdfAttributeOperator();
        private HDFDatasetOperator m_datasetOper = new HDFDatasetOperator();

        private string m_AttribXmlData;

        public bool Open(string hdfFile)
        {
            bool ret = true;
            m_fileId = H5F.open(hdfFile, H5F.OpenMode.ACC_RDONLY);
            if (m_fileId.Id == 0)
            {
                ret = false;
            }

            
            DumpAttri(hdfFile);

            return ret;
        }

        public void Close()
        {
            H5F.close(m_fileId);
        }

        public void GetHdfObjInfo()
        {
            GetHdfObjInfo("/");
        }


        public void GetHdfObjInfo(string groupName)
        {
            H5GroupId groupId = H5G.open(m_fileId, groupName);
            H5LIterateCallback myDelegate;
            myDelegate = Op_Func;
            ulong linkNumber = 0;

            int attriNum = H5A.getNumberOfAttributes(groupId);
            for (int i = 0; i < attriNum; i++)
            {
                string attriName = H5A.getNameByIndex(groupId, groupName, H5IndexType.CRT_ORDER, H5IterationOrder.NATIVE, (ulong)i);

                OnHdfObjInfoFound(attriName, HdfObjectType.Attribute, groupName);
            }


            H5L.iterate(groupId, H5IndexType.NAME, H5IterationOrder.NATIVE, ref linkNumber, myDelegate, groupName);
            H5G.close(groupId);

        }

        public event EventHandler HdfObjInfoFound;

        // Function used with H5L.iterate 
        protected H5IterationResult Op_Func(H5GroupId id,
                                               string objectName,
                                               H5LinkInfo info, Object param)
        {
            H5ObjectInfo objInfo = H5O.getInfoByName(id, objectName);
            
            string groupName = (string)param;
            switch (objInfo.objectType)
            {
                case H5ObjectType.DATASET:
                    OnHdfObjInfoFound(objectName, HdfObjectType.Dataset, groupName);
                    break;
                case H5ObjectType.GROUP:

                    GetHdfObjInfo(objectName);

                    break;
                case H5ObjectType.NAMED_DATATYPE:
                    break;
                case H5ObjectType.UNKNOWN:
                    break;

            }

            return H5IterationResult.SUCCESS;
        }

        private void OnHdfObjInfoFound(string objName, HdfObjectType type, string groupName)
        {
            if (this.HdfObjInfoFound != null)
            {

                this.HdfObjInfoFound(this, new HdfObjInfoEventArgs(objName, type, groupName));

            }
        }

        public AttributeValue GetAttribute(string attributeName)
        {

            //return m_attributeOper.GetAttribute(m_fileId, attributeName);
            return m_attributeOper.GetAttribute(m_AttribXmlData, attributeName, m_fileId);

        }

        public DatasetInfo GetDatasetInfo(string datasetName, string groupName)
        {
            return m_datasetOper.GetDatasetInfo(m_fileId,datasetName, groupName);
        }

        public void GetDataset<T>(string datasetName, string groupName,T[,] datasetOut, int rowIndex, int rowcount)
        {
            //m_datasetOper.GetDataset(m_fileId, datasetName, groupName,datasetOut, rowIndex, rowcount);
        }

        public void GetDataset<T>(string datasetName, string groupName, T[,,] datasetOut,DataValueType type)
        {
            m_datasetOper.GetDataset(m_fileId, datasetName, groupName,datasetOut,type);
        }

        protected void DumpAttri(string hdfFile)
        {
            m_AttribXmlData = hdfFile + ".xml";

            if (File.Exists(m_AttribXmlData))
                File.Delete(m_AttribXmlData);

            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            //string dunmpExeParth = dir + "\\h5dump.exe";
            string dunmpExeParth = dir + "\\dumphdf.bat";
            Process m_Proc = new Process();
            m_Proc.StartInfo.FileName = dunmpExeParth;
            m_Proc.StartInfo.WorkingDirectory = dir;
            m_Proc.StartInfo.CreateNoWindow = true;
            m_Proc.StartInfo.UseShellExecute = false;
           // m_Proc.StartInfo.RedirectStandardOutput = true;
            // m_Proc.OutputDataReceived += new DataReceivedEventHandler(OnProc_OutputDataReceived);
           // m_Proc.StartInfo.RedirectStandardInput = true;
            //m_Proc.EnableRaisingEvents = true;
       
           //m_Proc.StartInfo.Arguments = "-A --xml " + hdfFile + ">" + "C:\\项目\\HDFCompareStatistic\\TestData\\1.xml" ;
            m_Proc.StartInfo.Arguments = hdfFile;
            m_Proc.Start();

            m_Proc.WaitForExit();

            m_Proc.Close();
        }
    }

    // Event argument for the Changed event.
    //
    public class HdfObjInfoEventArgs : EventArgs
    {
        public readonly string objectName;
        public readonly HdfObjectType objType;
        public readonly string ObjGroupName;
        public HdfObjInfoEventArgs(string name, HdfObjectType type, string groupName)
        {
            objectName = name;
            objType = type;
            ObjGroupName = groupName;
        }
    }

}
