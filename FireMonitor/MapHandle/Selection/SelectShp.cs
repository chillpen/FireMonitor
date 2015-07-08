using GeoAPI;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using SharpMap.Rendering.Thematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MapHandle.Selection
{
    class SelectShp:ISelectRegion
    {
        GeoAPI.Geometries.Coordinate[] ContourGeomes;  //shp多边形边界的点，经纬度坐标
        List<PointF> bbox = new List<PointF>();        //与多边形相切的boundingbox内所有点坐标，经纬度
        //config.ProjPara pjPara;
        string featureName;
        float resolution ;
        config cfg;
        //NetTopologySuite.Geometries.Polygon polygon;
        NetTopologySuite.Geometries.Polygon polygon;
        //Polygon pl;
        


        public void setConfig(config incfg)
        {
            cfg = incfg;
            //resolution = cfg.resolution;
        }
        public VectorLayer getPolygonPoints(PointF points, VectorLayer veclayer)
        {
            //VectorLayer veclayer = (VectorLayer)m_viewBox.Map.GetLayerByName("province");
            SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer("Selection"); ;
            CustomTheme myTheme = new CustomTheme(FeatureColoured);
            ShapeFile vecshp = (ShapeFile)veclayer.DataSource;
            ShapeFile shp = vecshp;
            
            if (!shp.IsOpen)
                shp.Open();

            FeatureDataSet featDataSet = new FeatureDataSet();
            FeatureDataTable featDataTable = null;
            //将point的大地坐标转为经纬度
            Projection pj = new Projection();
            points = pj.GetLatLonFromXY(points,cfg.pjPara);


            //   获取feature数量
            uint featCount = (uint)shp.GetFeatureCount();
            for (uint index = 0; index < featCount; index++)
            {
                FeatureDataRow r = shp.GetFeature(index);
                GeoAPI.Geometries.Coordinate[] geomes = r.Geometry.Coordinates;
                double[] geomsX = new double[geomes.Length];
                double[] geomsY = new double[geomes.Length];
                for (int j = 0; j < geomes.Length; j++)
                {
                    geomsX[j] = geomes[j].X;
                    geomsY[j] = geomes[j].Y;
                }

                if ((points.X < geomsX.Min()) || (points.X > geomsX.Max()) || (points.Y < geomsY.Min()) || (points.Y > geomsY.Max()))
                {
                    continue;
                }

                PointF p1 = new PointF();
                p1.X = points.X;
                p1.Y = points.Y;
                if (InPolygon(geomes, p1))
                {
                    //首先把geomes传出去，供其他使用
                    ContourGeomes = geomes;
                    //如果在某区域内，选中某个区域，放入新图层
                    laySelected.DataSource = new SharpMap.Data.Providers.GeometryProvider(shp.GetFeature(index));
                    polygon = ((NetTopologySuite.Geometries.Polygon)r.Geometry);
                    laySelected.Style.Fill = new System.Drawing.SolidBrush(Color.HotPink);
                    laySelected.CoordinateTransformation = veclayer.CoordinateTransformation;
                }

            }
            return laySelected;
        }

        //获取与shp相切的bounding box
        private void getBbox(float res)
        {
            //首先通过ContourGeomes计算最大最小经纬度
            if (ContourGeomes == null)
                return;
            if (ContourGeomes.Length <= 0)
                return;
            //
            double maxLon = ContourGeomes[0].X;
            double minLon = ContourGeomes[0].X;
            double maxLat = ContourGeomes[0].Y;
            double minLat = ContourGeomes[0].Y;

            for (int i = 1; i < ContourGeomes.Length - 1; i++)
            {
                if (ContourGeomes[i].X > maxLon)
                    maxLon = ContourGeomes[i].X;
                if (ContourGeomes[i].Y > maxLat)
                    maxLat = ContourGeomes[i].Y;
                if (ContourGeomes[i].X < minLon)
                    minLon = ContourGeomes[i].X;
                if (ContourGeomes[i].Y < minLat)
                    minLat = ContourGeomes[i].Y;
            }

            //根据分辨率，计算经纬度的间距
            float res_deg = res * 360f / 2f /3.14f / 6371000f;
            //计算bbox内所有点的经纬坐标
            int width = (int)((maxLon - minLon) / res_deg) + 1;
            int height = (int)((maxLat - minLat) / res_deg) + 1;
            for(int i=0;i<width;i++)
                for(int j=0;j<height;j++)
                {
                    float x = (float)minLon + res_deg * i;  //lon
                    float y = (float)maxLat - res_deg * j;  //lat

                    if(y<=cfg.upperLeftLat&&y>=cfg.lowerRightLat&&x<=cfg.lowerRightLon&&x>=cfg.upperLeftLon)
                       bbox.Add(new PointF((float)minLon + res_deg * i, (float)maxLat - res_deg * j));

                }

        }

        //特征着色
        private SharpMap.Styles.VectorStyle FeatureColoured(SharpMap.Data.FeatureDataRow row)
        {
            SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();

            string NAME = row["NAME"].ToString().ToLower();
            if (NAME == featureName)
            {
                style.Fill = new System.Drawing.SolidBrush(Color.Green);
                style.Outline = new System.Drawing.Pen(Color.Transparent, 2);

                style.Line.Color = Color.Yellow;
                style.EnableOutline = true;

                return style;
            }
            else
            {
                //style.Fill = new SolidBrush(Color.Transparent);
                //style.Outline = new Pen(Color.ForestGreen, 0.4f);
                //style.Line.Width = 1;
                //style.Line.Color = Color.Green;
                style.Fill = new System.Drawing.SolidBrush(Color.Transparent);
                style.Outline = new System.Drawing.Pen(Color.ForestGreen, 0.4f);

                style.Line.Color = Color.Green;
                style.EnableOutline = true;
                return style;
            }

        }

        const int N = 100;
        const int offset = 1000;   //offset为多边形坐标上限
        const double eps = 1e-8;
        bool isZero(double x) { return (x > 0 ? x : -x) < eps; }
        double crossProd(PointF A, PointF B, PointF C)
        {
            return (B.X - A.X) * (C.Y - A.Y) - (B.Y - A.Y) * (C.X - A.X);
        }

        //判断点是否在多边形内
        private bool InPolygon(GeoAPI.Geometries.Coordinate[] geomes, PointF p1)
        {
            int n = geomes.Length;
            PointF[] p = new PointF[n];
            PointF p2 = new PointF();

            for (int index = 0; index < n; index++)
            {
                p[index].X = (float)geomes[index].X;
                p[index].Y = (float)geomes[index].Y;
            }

            int count = 0;
            int i = 0;
            p[n - 1] = p[0];
            Random ran = new Random();

            while (i < n - 1)
            {
                int aa = ran.Next(-1000, 1000);
                p2.X = aa + offset;  //随机取一个足够远的点p2 
                int bb = ran.Next(-1000, 1000);
                p2.Y = bb + offset;  //以p1为起点p2为终点做射线L 
                for (i = count = 0; i < n - 1; ++i)
                {//依次对多边形的每条边进行考察 
                    if (isZero(crossProd(p1, p[i], p[i + 1])) &&
                    (p[i].X - p1.X) * (p[i + 1].X - p1.X) < eps && (p[i].Y - p1.Y) * (p[i + 1].Y - p1.Y) < eps)
                        return true;//点p1在边上,返回点p1在边上的信息 
                    else if (isZero(crossProd(p1, p2, p[i]))) break;//点p[i]在射线p1p2上，停止本循环，另取p2 
                    else if (crossProd(p[i], p[i + 1], p1) * crossProd(p[i], p2, p[i + 1]) > eps &&//射线与边相交，统计交点数 
                             crossProd(p1, p2, p[i]) * crossProd(p1, p[i + 1], p2) > eps) ++count;
                }
            }
            int str = count & 1;
            if (str > 0)
                return true;
            else
                return false;

        }

        //获取polygon内部的点
        public List<PointF> getInnerPoints()
        {
            List<PointF> pts = new List<PointF>();

            getBbox(cfg.resolution);

            for (int i = 0; i < bbox.Count;i++ )
            {
                if (InPolygon(ContourGeomes, bbox[i]))


            //    //-----------nettopologysuite method-----
            //    //GeoAPI.Geometries.IGeometryFactory gfact = new GeometryFactory();
            //    //GeoAPI.Geometries.IPoint ipoint;
            //    //ipoint = gfact.CreatePoint(new GeoAPI.Geometries.Coordinate(bbox[i].X, bbox[i].Y));
            //    //bool a = polygon.Contains(ipoint);
            //    //if (polygon != null && a)

                {
                    //将经纬度坐标转为大地坐标
                    PointF tmppoint = new PointF();
                    Projection pj = new Projection();
                    tmppoint = pj.GetXYFromLatLon(bbox[i],cfg.pjPara);
                    pts.Add(tmppoint);
                }


            }

            ////random points builder
            //NetTopologySuite.Shape.Random.RandomPointsBuilder rpb = new NetTopologySuite.Shape.Random.RandomPointsBuilder();
            //rpb.SetExtent(polygon);
            //IGeometry testGeo = rpb.GetGeometry();
            //testGeo.
            
                return pts;
        }


        public List<PointF> getSelectedPts()
        {
            List<PointF> pts = new List<PointF>();
            pts = getInnerPoints();

            return pts;
        }
    }
}
