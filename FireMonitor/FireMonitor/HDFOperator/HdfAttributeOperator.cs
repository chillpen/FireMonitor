using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDF5DotNet;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace FireMonitor.HDFOper
{
    public struct AttributeValue
    {
        public DataValueType valueType;
        public object dataValue;
        public int rank;
    }

    public class HdfAttributeOperator
    {

        //public AttributeValue GetAttribute(H5FileId fileId, string attributeName)
        //{
        //    H5AttributeId attributeId = H5A.openName(fileId, attributeName);    //根据属性名称得到属性Id

        //    H5DataTypeId attributeType = H5A.getType(attributeId);            //得到属性数据类型

        //    int size = H5T.getSize(attributeType);

        //    H5T.H5TClass typeClass = H5T.getClass(attributeType);

        //    H5DataSpaceId spaceId = H5A.getSpace(attributeId);
        //    long[] dims = H5S.getSimpleExtentDims(spaceId);
        //    int rank = H5S.getSimpleExtentNDims(spaceId);

        //    H5T.H5Type h5type;

        //    Type dataType = null;

        //    AttributeValue atrributeData = new AttributeValue();
        //    atrributeData.dataValue = null;
        //    atrributeData.valueType = DataValueType.EMPTY;
        //    atrributeData.rank = rank;

        //    switch (typeClass)
        //    {
        //        case H5T.H5TClass.FLOAT:
        //            h5type = H5T.H5Type.NATIVE_FLOAT;

        //            if (rank == 1)
        //            {
        //                float[] floatDatatmp = new float[dims[0]];
        //                H5A.read(attributeId, new H5DataTypeId(h5type), new H5Array<float>(floatDatatmp));
        //                atrributeData.dataValue = floatDatatmp;

        //            }
        //            else if (rank == 2)
        //            {
        //                float[,] floatDatatmp = new float[dims[0], dims[1]];
        //                H5A.read(attributeId, new H5DataTypeId(h5type), new H5Array<float>(floatDatatmp));
        //                atrributeData.dataValue = floatDatatmp;

        //            }

        //            atrributeData.valueType = DataValueType.FLOAT;

        //            break;
        //        case H5T.H5TClass.INTEGER:
        //            h5type = H5T.H5Type.NATIVE_INT;
        //            // int[,] intDatatmp = null;
        //            if (rank == 1)
        //            {
        //                int[] intDatatmp = new int[dims[0]];
        //                H5A.read(attributeId, new H5DataTypeId(h5type), new H5Array<int>(intDatatmp));

        //                atrributeData.dataValue = intDatatmp;

        //            }
        //            else if (rank == 2)
        //            {
        //                int[,] intDatatmp = new int[dims[0], dims[1]];
        //                H5A.read(attributeId, new H5DataTypeId(h5type), new H5Array<int>(intDatatmp));

        //                atrributeData.dataValue = intDatatmp;
        //            }

        //            atrributeData.valueType = DataValueType.INT;

        //            break;
        //        case H5T.H5TClass.STRING:
        //            h5type = H5T.H5Type.C_S1;

        //            if (rank == 0)
        //            {
        //                string[] stringDatatmp = new string[1];
        //                byte[] bytedata = new byte[255];

        //                H5A.read(attributeId, attributeType, new H5Array<byte>(bytedata));
        //                //H5A.read(attributeId, new H5DataTypeId(h5type), new H5Array<string>(stringDatatmp));
        //                stringDatatmp[0] = Encoding.Default.GetString(bytedata).Trim('\0');
        //                atrributeData.dataValue = stringDatatmp;
        //            }
        //            else if (rank == 1)
        //            {
        //                string[] stringDatatmp = new string[dims[0]];
        //                // string stringDatatmp = "";
        //                // byte[] bytedata = new byte[255];
        //                // byte[,] bytedata = new byte[2,255];

        //                // H5DataTypeId memtype = H5T.copy(H5T.H5Type.C_S1);
        //                //H5T.setVariableSize(memtype);
        //                //H5T.setSize(attributeType, size);
        //                // H5A.read(attributeId, memtype, new H5Array<string>(stringDatatmp));

        //                // H5A.read(attributeId, new H5DataTypeId(h5type), new H5Array<string>(stringDatatmp));

        //                // stringDatatmp[0] = Encoding.Default.GetString(bytedata).Trim('\0');
        //                //string test = Encoding.Default.GetString(bytedata).Trim('\0');
        //                // atrributeData.dataValue = stringDatatmp;
        //                //  VariableLengthString[] value = new VariableLengthString[1]; 
        //                atrributeData.dataValue = stringDatatmp;
        //            }
        //            else if (rank == 2)
        //            {
        //                string[,] stringDatatmp = new string[dims[0], dims[1]];
        //                //H5DataTypeId memtype = H5T.copy(H5T.H5Type.C_S1);
        //                //H5T.setVariableSize(memtype);
        //                //H5A.read(attributeId, memtype, new H5Array<string>(stringDatatmp));
        //                atrributeData.dataValue = stringDatatmp;

        //            }

        //            atrributeData.valueType = DataValueType.STRING;

        //            break;
        //        default:
        //            h5type = H5T.H5Type.C_S1;
        //            break;
        //    }

        //    H5T.close(attributeType);
        //    H5S.close(spaceId);
        //    H5A.close(attributeId);
        //    return atrributeData;


        //}

        ValidationEventHandler eventHandler = new ValidationEventHandler(settings_ValidationEventHandler);


        public AttributeValue GetAttribute(string attribXml, string attributeName, H5FileId fileId)
        {
            H5AttributeId attributeId = H5A.openName(fileId, attributeName);    //根据属性名称得到属性Id
            H5DataTypeId attributeType = H5A.getType(attributeId);            //得到属性数据类型
            H5T.H5TClass typeClass = H5T.getClass(attributeType);
            H5DataSpaceId spaceId = H5A.getSpace(attributeId);
            int rank = H5S.getSimpleExtentNDims(spaceId);


            AttributeValue atrributeData = new AttributeValue();

            string[] stringDatatmp = new string[1];
            stringDatatmp[0] = "NULL";

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.ValidationType = ValidationType.None;
            settings.ValidationEventHandler += settings_ValidationEventHandler;
            settings.CheckCharacters = false;


            StringReader xmlSr = ChangeXmlInGBCode(attribXml);

            XmlReader reader = XmlReader.Create(xmlSr);


            XmlDocument xml = new XmlDocument();
            xml.Load(reader);

            XmlNodeList node = xml.GetElementsByTagName("hdf5:Attribute");

            foreach (XmlNode child in node)
            {
                if (child.OuterXml.Contains(attributeName))
                    stringDatatmp[0] = child.InnerText;

            }
            stringDatatmp[0] = stringDatatmp[0].Replace("\r\n", "");
            stringDatatmp[0] = stringDatatmp[0].Trim();
            atrributeData.dataValue = stringDatatmp;

            switch (typeClass)
            {
                case H5T.H5TClass.FLOAT:
                    atrributeData.valueType = DataValueType.FLOAT;
                    break;
                case H5T.H5TClass.INTEGER:
                    atrributeData.valueType = DataValueType.INT;
                    break;
                case H5T.H5TClass.STRING:

                    atrributeData.valueType = DataValueType.STRING;

                    break;
                default:

                    break;
            }
            atrributeData.rank = rank;
            H5T.close(attributeType);
            H5S.close(spaceId);
            H5A.close(attributeId);
            return atrributeData;
        }

        private static void settings_ValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private StringReader ChangeXmlInGBCode(string xmlFile)
        {
            StreamReader fileStream = new StreamReader(xmlFile, Encoding.UTF8);

            string content = fileStream.ReadToEnd();
            //content = content.Replace("encoding=\"UTF-8\"", "encoding=\"Unicode\"");
            //content = content.Normalize();
            fileStream.Close();


            return new StringReader(content);
        }
    }
}
