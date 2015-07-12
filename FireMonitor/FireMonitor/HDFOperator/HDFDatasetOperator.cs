using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDF5DotNet;
using System.Runtime.InteropServices;


namespace FireMonitor.HDFOper
{
    public struct DatasetInfo
    {
        public int rank;
        public int band;
        public int row;
        public int col;
        public DataValueType type;
    }

    public struct CompoundStruct
    {
        public int v1;
        public int v2;
        public int v3;
        public int v4;

    }

    public class HDFDatasetOperator
    {
        public DatasetInfo GetDatasetInfo(H5FileId fileId, string datasetName, string groupName)
        {
            DatasetInfo datasetInfo = new DatasetInfo();
            datasetInfo.band = 1;
            datasetInfo.col = 1;
            datasetInfo.rank = 1;
            datasetInfo.row = 1;

            H5GroupId groupId = H5G.open(fileId, groupName);
            H5DataSetId dataSetId = H5D.open(groupId, datasetName);
            //  ulong storeSize = H5D.getStorageSize(dataSetId); //得到数据数组存储大小
            H5DataSpaceId spaceid = H5D.getSpace(dataSetId);
            long[] dims = H5S.getSimpleExtentDims(spaceid);//得到数据数组的大小，比如[3,1800,2048]
            datasetInfo.rank = H5S.getSimpleExtentNDims(spaceid);//得到数据数组的维数，比如3

            int dimCount = dims.Length;
            if (dimCount == 2)
            {
                datasetInfo.col = Convert.ToInt32(dims[1]);//宽
                datasetInfo.row = Convert.ToInt32(dims[0]);
            }
            else if (dimCount == 3)
            {
                datasetInfo.band = Convert.ToInt32(dims[0]);//波段数
                datasetInfo.col = Convert.ToInt32(dims[2]);  //宽
                datasetInfo.row = Convert.ToInt32(dims[1]);  //高
            }
            else if (dimCount == 1)
            {
                datasetInfo.row = Convert.ToInt32(dims[0]);//高
            }

            H5DataTypeId typeId = H5D.getType(dataSetId);
            H5T.H5TClass dataClass = H5T.getClass(typeId);//得到数据集的类型
            string typeName = dataClass.ToString();

            switch (typeName)
            {
                case "FLOAT":
                    datasetInfo.type = DataValueType.FLOAT;
                    break;

                case "INTEGER":
                    datasetInfo.type = DataValueType.INT;
                    break;

                case "COMPOUND":
                    datasetInfo.type = DataValueType.COMPOUND;
                    H5DataTypeId tid0 = H5D.getType(dataSetId);
                    int nMember = H5T.getNMembers(tid0);
                    datasetInfo.col = nMember;

                    break;
                default:
                    datasetInfo.type = DataValueType.EMPTY;
                    break;
            }


            H5T.close(typeId);

            H5S.close(spaceid);

            H5D.close(dataSetId);

            H5G.close(groupId);
            return datasetInfo;
        }

        /// <summary>
        /// 暂时不用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileId"></param>
        /// <param name="datasetName"></param>
        /// <param name="groupName"></param>
        /// <param name="datasetOut"></param>
        /// <param name="rowIndex"></param>
        /// <param name="rowcount"></param>
        /// <param name="colcount"></param>
        public void GetDataset<T>(H5FileId fileId, string datasetName, string groupName, T[,] datasetOut, int rowIndex, int rowcount, int colcount)
        {

            H5GroupId groupId = H5G.open(fileId, groupName);
            H5DataSetId dataSetId = H5D.open(groupId, datasetName/*"EV_Emissive"*/);
            H5DataTypeId tid0 = H5D.getType(dataSetId);

            H5DataSpaceId spaceid = H5D.getSpace(dataSetId);


            long[] start = new long[2];
            start[0] = rowIndex;
            start[1] = 0;
            long[] count = new long[2];
            count[0] = rowcount;
            count[1] = colcount;

            H5S.selectHyperslab(spaceid, H5S.SelectOperator.SET, start, count);

            //long[] dimes = new long[2];
            //dimes[0] = 1;
            //dimes[1] = 8192;

            H5DataSpaceId simpleSpaceid = H5S.create_simple(2, count);


            H5PropertyListId listid = new H5PropertyListId(H5P.Template.DEFAULT);

            H5DataTypeId tid1 = new H5DataTypeId(H5T.H5Type.NATIVE_INT);//数据类型

            // Read the array back

            //int[,] dataSet = new int[cout[0], cout[1]];
            H5D.read(dataSetId, tid1, simpleSpaceid, spaceid, listid, new H5Array<T>(datasetOut));

            H5S.close(simpleSpaceid);
            H5S.close(spaceid);
            H5T.close(tid0);
            H5D.close(dataSetId);
            H5G.close(groupId);

        }

        public void GetDataset<T>(H5FileId fileId, string datasetName, string groupName, T[, ,] datasetOut, DataValueType type)
        {

            H5GroupId groupId = H5G.open(fileId, groupName);
            H5DataSetId dataSetId = H5D.open(groupId, datasetName/*"EV_Emissive"*/);

            switch (type)
            {
                case DataValueType.FLOAT:
                    H5DataTypeId tidfloat = new H5DataTypeId(H5T.H5Type.NATIVE_FLOAT);

                    // Read the array back
                    H5D.read(dataSetId, tidfloat,
                        new H5Array<T>(datasetOut));//(dataSetId, tid1, new H5Array<int>(vlReadBackArray));
                    // H5T.close(tidfloat);
                    break;
                case DataValueType.INT:
                    H5DataTypeId tidint = new H5DataTypeId(H5T.H5Type.NATIVE_INT);



                    //  H5T.H5TClass c =  H5T.getMemberClass(tid0);


                    // Read the array back
                    H5D.read(dataSetId, tidint,
                        new H5Array<T>(datasetOut));//(dataSetId, tid1, new H5Array<int>(vlReadBackArray));


                    //H5T.close(tidint);
                    break;
                case DataValueType.COMPOUND:
                    H5DataTypeId tid0 = H5D.getType(dataSetId);

                    int nMember = H5T.getNMembers(tid0);

                    H5DataSpaceId spaceid = H5D.getSpace(dataSetId);
                    long[] dims = H5S.getSimpleExtentDims(spaceid);//得到数据数组的大小，比如[3,1800,2048]
                    int length = 1;
                    for (int i = 0; i < dims.Length; i++)
                    {
                        length *= (int)dims[i];
                    }

                    for (int i = 0; i < nMember; i++)
                    {
                        string memberName = H5T.getMemberName(tid0, i);
                        H5DataTypeId memberTypeId = H5T.getMemberType(tid0, i);
                        H5T.H5TClass dataClass = H5T.getClass(memberTypeId);//得到数据集的类型
                        string typeName = dataClass.ToString();

                        if (typeName == "INTEGER")//目前先只支持整形的
                        {
                            H5DataTypeId tidtmp = H5T.create(H5T.CreateClass.COMPOUND, sizeof(int));
                            H5T.insert(tidtmp, memberName, 0, H5T.H5Type.NATIVE_INT);

                            int[] dataTmp = new int[length];
                            H5D.read(dataSetId, tidtmp, new H5Array<int>(dataTmp));

                            for (int j = 0; j < length; j++)
                            {
                                datasetOut[0, j, i] = (T)Convert.ChangeType(dataTmp[j], datasetOut[0, j, i].GetType());

                            }
                        }




                    }

                    H5S.close(spaceid);
                    break;
                default:
                    break;
            }



            H5D.close(dataSetId);
            H5G.close(groupId);
            //H5F.close(fileId);
        }

    }
}
