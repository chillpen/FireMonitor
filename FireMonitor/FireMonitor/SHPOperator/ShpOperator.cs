using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data;


namespace FireMonitor.SHPOperator
{
    public class ShpOperator
    {
        public bool ReadShpFile(string fileName)
        {
            if (!File.Exists(fileName))
                return false;


            //FileStream fs = File.OpenRead(fileName);

            //BinaryReader binReader = new BinaryReader(fs);

            //long length = fs.Length;

            //binReader.ReadBytes(36);

            //double xmin, ymin, xmax, ymax;

            //xmin = binReader.ReadDouble();
            //ymax = binReader.ReadDouble();
            //xmax = binReader.ReadDouble();
            //ymin = binReader.ReadDouble();


            //int head = fs.r
            ParseSHP(fileName);
            return true;
        }

        private List<Polygon_shape> m_polygons = new List<Polygon_shape>();

        public List<Polygon_shape> Polygons
        {
            get { return m_polygons; }
        }
        private List<PolyLine_shape> m_polylines = new List<PolyLine_shape>();

        public List<PolyLine_shape> Polylines
        {
            get { return m_polylines; }
        }

        private List<PointF> m_points = new List<PointF>();

        public List<PointF> Points
        {
            get { return m_points; }
        }


        Pen pen = new Pen(Color.Blue, 2);
        public int ShapeType;


        private void ParseSHP(string fileName)
        {
            BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open));

            br.ReadBytes(24);
            int FileLength = br.ReadInt32();//<0代表数据长度未知
            int FileBanben = br.ReadInt32();
            ShapeType = br.ReadInt32();
            double xmin, ymin, xmax, ymax;
            xmin = br.ReadDouble();
            ymax = -1 * br.ReadDouble();
            xmax = br.ReadDouble();
            ymin = -1 * br.ReadDouble();
            double width = xmax * xmin;
            double height = ymax * ymin;
            br.ReadBytes(32);

            switch (ShapeType)
            {
                case 1:
                    m_points.Clear();
                    while (br.PeekChar() != -1)
                    {
                        PointF point = new PointF();

                        uint RecordNum = br.ReadUInt32();
                        int DataLength = br.ReadInt32();


                        //读取第i个记录
                        br.ReadInt32();
                        point.X = (float)br.ReadDouble();
                        point.Y = (float)br.ReadDouble();
                        m_points.Add(point);
                    }
                    break;
                case 3:
                    m_polylines.Clear();
                    while (br.PeekChar() != -1)
                    {
                        PolyLine_shape polyline = new PolyLine_shape();
                        polyline.Box = new double[4];
                        polyline.Parts = new List<int>();
                        polyline.Points = new List<PointF>();

                        uint RecordNum = br.ReadUInt32();
                        int DataLength = br.ReadInt32();

                        //读取第i个记录
                        br.ReadInt32();
                        polyline.Box[0] = br.ReadDouble();
                        polyline.Box[1] = br.ReadDouble();
                        polyline.Box[2] = br.ReadDouble();
                        polyline.Box[3] = br.ReadDouble();
                        polyline.NumParts = br.ReadInt32();
                        polyline.NumPoints = br.ReadInt32();
                        for (int i = 0; i < polyline.NumParts; i++)
                        {
                            polyline.Parts.Add(br.ReadInt32());
                        }
                        for (int j = 0; j < polyline.NumPoints; j++)
                        {

                            PointF pointtemp = new PointF();
                            pointtemp.X = (float)br.ReadDouble();
                            pointtemp.Y = (float)br.ReadDouble();

                            polyline.Points.Add(pointtemp);
                        }
                        m_polylines.Add(polyline);
                    }

                    break;
                case 5:
                    m_polygons.Clear();
                    while (br.PeekChar() != -1)
                    {
                        Polygon_shape polygon = new Polygon_shape();
                        polygon.Box = new double[4];
                        polygon.Parts = new List<int>();
                        polygon.Points = new List<PointF>();

                        uint RecordNum = br.ReadUInt32();
                        int DataLength = br.ReadInt32();

                        //读取第i个记录
                        int m = br.ReadInt32();
                        polygon.Box[0] = br.ReadDouble();
                        polygon.Box[1] = br.ReadDouble();
                        polygon.Box[2] = br.ReadDouble();
                        polygon.Box[3] = br.ReadDouble();
                        polygon.NumParts = br.ReadInt32();
                        polygon.NumPoints = br.ReadInt32();
                        for (int j = 0; j < polygon.NumParts; j++)
                        {
                            polygon.Parts.Add(br.ReadInt32());
                        }
                        for (int j = 0; j < polygon.NumPoints; j++)
                        {
                            PointF pointtemp = new PointF();
                            pointtemp.X = (float)br.ReadDouble();
                            pointtemp.Y = (float)br.ReadDouble();
                            polygon.Points.Add(pointtemp);
                        }
                        m_polygons.Add(polygon);
                    }

                    break;
            }
            br.Close();
        }

    }
}
public struct Polygon_shape
{
    public double[] Box; //边界盒
    public int NumParts; //部分的数目
    public int NumPoints; //点的总数目
    public List<int> Parts; //在部分中第一个点的索引
    public List<PointF> Points; //所有部分的点
}

public struct PolyLine_shape
{
    public double[] Box; //边界盒
    public int NumParts; //部分的数目
    public int NumPoints; //点的总数目
    public List<int> Parts; //在部分中第一个点的索引
    public List<PointF> Points; //所有部分的点
}