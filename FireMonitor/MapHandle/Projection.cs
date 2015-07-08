using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proj4App;
using System.Drawing;

namespace MapHandle
{
    public class Projection
    {
        public enum ProjType
        {
            Mercator,
            Lambert,
            Stereographic,
            Albers,
            Latlon,
        }

        protected static double RAD2DEG = 180.0 / Math.PI;
        protected static double DEG2RAD = Math.PI / 180.0;

        //获取投影方式索引值
        private int getProjNum(ProjType pt)
        {
            int projNum=0;
            
            switch(pt)
            {
                case ProjType.Latlon:
                    projNum = 1;
                    break;
                case ProjType.Mercator:
                    projNum = 2;
                    break;
                case ProjType.Lambert:
                    projNum = 3;
                    break;
                case ProjType.Stereographic:
                    projNum = 4;
                    break;
                case ProjType.Albers:
                    projNum = 5;
                    break;
            }
      
            return projNum;
        }
        
        //获取shp投影方式
        public ICoordinateTransformation getmapTransform(config.ProjPara proj)
        {
            CoordinateTransformationFactory ctFac = new CoordinateTransformationFactory();
            CoordinateSystemFactory cFac = new CoordinateSystemFactory();
            ICoordinateTransformation transform = null;
            
            //等经纬，shp数据原始投影
            var epsg4326 = cFac.CreateFromWkt("GEOGCS[\"GCS_WGS_1984\",DATUM[\"WGS_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]");
            //目标坐标系
            var epsg3857 = cFac.CreateFromWkt(getSrcCoordinate(proj));
            transform = ctFac.CreateFromCoordinateSystems(epsg4326, epsg3857);

            return transform;
        }

        //从ProjPara参数中获取坐标系
        string getSrcCoordinate(MapHandle.config.ProjPara proj)
        {
            string srcproj="";
            //获取投影方式对应的index
            int projType = getProjNum(proj.type);

            if (projType == 1)
            {
                //等经纬投影
                srcproj = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"WGS_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]";
            }
            if (projType == 2)
            {
                //麦卡托投影
                string wktparam2 = "PROJCS[\"Mercator-projection\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"WGS_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Mercator\"],PARAMETER[\"False_Easting\"," + proj.false_easting.ToString() + "],PARAMETER[\"False_Northing\"," + proj.false_northing.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"longitude_of_center\"," + proj.center_lon.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"latitude_of_center\"," + proj.center_lat.ToString() + "],PARAMETER[\"standard_parallel_1\"," + proj.std_paralle1.ToString() + "], PARAMETER[\"standard_parallel_2\", " + proj.std_paralle2.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"central_meridian\"," + proj.cen_meridian.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"latitude_of_origin\"," + proj.latitude_origin.ToString() + "], PARAMETER[\"scale_factor\", " + proj.scale_factor.ToString() + "], UNIT[\"Meter\", " + proj.unit.ToString() + "]]";
                srcproj += wktparam2;
            }
            else if (projType == 3)
            {
                //lambert投影
                string wktparam2 = "PROJCS[\"Lambert-projection\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"WGS_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"False_Easting\"," + proj.false_easting.ToString() + "],PARAMETER[\"False_Northing\"," + proj.false_northing.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"longitude_of_center\"," + proj.center_lon.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"latitude_of_center\"," + proj.center_lat.ToString() + "],PARAMETER[\"standard_parallel_1\"," + proj.std_paralle1.ToString() + "], PARAMETER[\"standard_parallel_2\", " + proj.std_paralle2.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"central_meridian\"," + proj.cen_meridian.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"latitude_of_origin\", " + proj.latitude_origin.ToString() + "], PARAMETER[\"scale_factor\", " + proj.scale_factor.ToString() + "], UNIT[\"Meter\", " + proj.unit.ToString() + "]]";
                srcproj += wktparam2;
                
            }
            else if (projType == 5)
            {
                //Albers投影
                string wktparam2 = "PROJCS[\"Albers-projection\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"WGS_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Albers\"],PARAMETER[\"False_Easting\"," + proj.false_easting.ToString() + "],PARAMETER[\"False_Northing\"," + proj.false_northing.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"longitude_of_center\"," + proj.center_lon.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"standard_parallel_1\"," + proj.std_paralle1.ToString() + "], PARAMETER[\"standard_parallel_2\", " + proj.std_paralle2.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"latitude_of_origin\", " + proj.latitude_origin.ToString() + "], PARAMETER[\"scale_factor\",  " + proj.scale_factor.ToString() + "], UNIT[\"Meter\", " + proj.unit.ToString() + "]]";
                srcproj += wktparam2;
               
            }
            else if (projType == 6) //未使用
            {
                //高斯投影
                string wktparam2 = "PROJCS[\"utm-projection\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"WGS_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\"," + proj.false_easting.ToString() + "],PARAMETER[\"False_Northing\"," + proj.false_northing.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"longitude_of_center\"," + proj.center_lon.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"standard_parallel_1\"," + proj.std_paralle1.ToString() + "], PARAMETER[\"standard_parallel_2\",  " + proj.std_paralle2.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"central_meridian\"," + proj.cen_meridian.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"latitude_of_origin\",  " + proj.latitude_origin.ToString() + "], PARAMETER[\"scale_factor\",  " + proj.scale_factor.ToString() + "], UNIT[\"Meter\", " + proj.unit.ToString() + "]]";
                srcproj += wktparam2;
                
            }
            else if (projType == 7)//未使用
            {
                //UTM投影              
                string wktparam2 = "PROJCS[\"World_Robinson\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"WGS_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Robinson\"],PARAMETER[\"False_Easting\"," + proj.false_easting.ToString() + "],PARAMETER[\"False_Northing\"," + proj.false_northing.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"longitude_of_center\"," + proj.center_lon.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"standard_parallel_1\"," + proj.std_paralle1.ToString() + "], PARAMETER[\"standard_parallel_2\", " + proj.std_paralle2.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"central_meridian\"," + proj.cen_meridian.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"latitude_of_origin\", " + proj.latitude_origin.ToString() + "], PARAMETER[\"scale_factor\", " + proj.scale_factor.ToString() + "], UNIT[\"Meter\", " + proj.unit.ToString() + "]]";
                srcproj += wktparam2;
                
            }
            else if (projType == 4)
            {
                //极射赤面
                string wktparam2 = "PROJCS[\"Stereographic\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"WGS_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Stereographic\"],PARAMETER[\"False_Easting\"," + proj.false_easting.ToString() + "],PARAMETER[\"False_Northing\"," + proj.false_northing.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"longitude_of_center\"," + proj.center_lon.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ",PARAMETER[\"latitude_of_center\"," + proj.center_lat.ToString() + "]";
                srcproj += wktparam2;
                wktparam2 = ", PARAMETER[\"scale_factor\", " + proj.scale_factor.ToString() + "], UNIT[\"Meter\", " + proj.unit.ToString() + "]]";
                srcproj += wktparam2;
                
            }

            return srcproj;
        }

        //从ProjPara参数中获取Proj4的投影参数
        string getSrcProj(MapHandle.config.ProjPara proj)
        {
            string srcproj = "";

            //获取投影方式对应的index
            int projType = getProjNum(proj.type);

            switch(projType)
            {
                case 1:   //等经纬
                    srcproj = ProjParam("latlong", ProjectionAPI.Setting.ProjParamSetting.Ellps.WGS84, ProjectionAPI.Setting.ProjParamSetting.Daunm.WGS84, proj.std_paralle1, proj.std_paralle2, proj.center_lon, proj.false_easting, proj.false_northing, ProjectionAPI.Setting.ProjParamSetting.Units.m, proj.scale_factor, "-1");
                    break;
                case 2:   //麦卡托
                    srcproj = ProjParam("merc", ProjectionAPI.Setting.ProjParamSetting.Ellps.WGS84, ProjectionAPI.Setting.ProjParamSetting.Daunm.WGS84, proj.std_paralle1, proj.std_paralle2, proj.center_lon, proj.false_easting, proj.false_northing, ProjectionAPI.Setting.ProjParamSetting.Units.m, proj.scale_factor, "-1");
                    break;
                case 3:   //lambert
                    srcproj = ProjParam("lcc", ProjectionAPI.Setting.ProjParamSetting.Ellps.WGS84, ProjectionAPI.Setting.ProjParamSetting.Daunm.WGS84, proj.std_paralle1, proj.std_paralle2, proj.center_lon, proj.false_easting, proj.false_northing, ProjectionAPI.Setting.ProjParamSetting.Units.m, proj.scale_factor, "-1");
                    //srcproj = "+proj=lcc +lat_1=25 +lat_2=47 +lat_0= 36 +lon_0=115 +x_0=0 +y_0=0 ";
                    break;
                case 4:   //stereographic
                    srcproj = ProjParam("stere", ProjectionAPI.Setting.ProjParamSetting.Ellps.WGS84,90.0f, proj.std_paralle1, proj.std_paralle2, proj.center_lon, proj.false_easting, proj.false_northing, ProjectionAPI.Setting.ProjParamSetting.Units.m, 1.0, "-1");
                    break;
                case 5:   //albers
                    srcproj = ProjParam("aea", ProjectionAPI.Setting.ProjParamSetting.Ellps.WGS84, ProjectionAPI.Setting.ProjParamSetting.Daunm.WGS84, proj.std_paralle1, proj.std_paralle2, proj.center_lon, proj.false_easting, proj.false_northing, ProjectionAPI.Setting.ProjParamSetting.Units.m, proj.scale_factor, "-1");
                    break;
                case 6:   //Gauss
                    srcproj = ProjParam("gstmerc", ProjectionAPI.Setting.ProjParamSetting.Ellps.WGS84, ProjectionAPI.Setting.ProjParamSetting.Daunm.WGS84, proj.std_paralle1, proj.std_paralle2, proj.center_lon, proj.false_easting, proj.false_northing, ProjectionAPI.Setting.ProjParamSetting.Units.m, proj.scale_factor, "-1");
                    break;
                case 7:   //utm
                    srcproj = ProjParam("utm", ProjectionAPI.Setting.ProjParamSetting.Ellps.WGS84, ProjectionAPI.Setting.ProjParamSetting.Daunm.WGS84, proj.std_paralle1, proj.std_paralle2, proj.center_lon, proj.false_easting, proj.false_northing, ProjectionAPI.Setting.ProjParamSetting.Units.m, proj.scale_factor, "-1");
                    break;
            }

            return srcproj;
        }

        /// <summary>
        /// 设置投影参数
        /// </summary>
        /// <param name="projType">投影类型</param>
        /// <param name="ellpse">参考椭球体</param>
        /// <param name="lat_1">标准纬度1</param>
        /// <param name="lat_2">标准纬度2</param>
        /// <param name="lon_0">中心经线</param>
        /// <param name="x_0">x偏移</param>
        /// <param name="y_0">y偏移</param>
        /// <param name="units">单位</param>
        /// <param name="k">缩放</param>
        /// <param name="daunm">基准面</param> Daunm daunm,
        public string ProjParam(string projType, ProjectionAPI.Setting.ProjParamSetting.Ellps ellpse, float lat_0, float lat_1, float lat_2, float lon_0, float x_0, float y_0, ProjectionAPI.Setting.ProjParamSetting.Units units, double k, string daunm = "-1")
        {
            string m_projParam = "";
            m_projParam += "+proj=" + projType;
            m_projParam += " +ellps=" + ellpse.ToString();
            m_projParam += " +lat_0=" + lat_0.ToString();
            m_projParam += " +lat_1=" + lat_1.ToString();
            m_projParam += " +lat_2=" + lat_2.ToString();
            m_projParam += " +lon_0=" + lon_0.ToString();
            m_projParam += " +x_0=" + x_0.ToString();
            m_projParam += " +y_0=" + y_0.ToString();
            m_projParam += " +units=" + units.ToString();
            m_projParam += " +k_0=" + k.ToString();

            return m_projParam;
        }
        public string ProjParam(string projType, ProjectionAPI.Setting.ProjParamSetting.Ellps ellpse, ProjectionAPI.Setting.ProjParamSetting.Daunm daum, float lat_1, float lat_2, float lon_0, float x_0, float y_0, ProjectionAPI.Setting.ProjParamSetting.Units units, double k, string daunm = "-1")
        {
            string m_projParam = "";
            m_projParam += "+proj=" + projType;
            m_projParam += " +ellps=" + ellpse.ToString();
            m_projParam += " +datum=" + daum.ToString();

            m_projParam += " +lat_1=" + lat_1.ToString();
            m_projParam += " +lat_2=" + lat_2.ToString();
            m_projParam += " +lon_0=" + lon_0.ToString();
            m_projParam += " +x_0=" + x_0.ToString();
            m_projParam += " +y_0=" + y_0.ToString();
            m_projParam += " +units=" + units.ToString();
            m_projParam += " +k_0=" + k.ToString();

            return m_projParam;
        }

        //多点投影转换-正投影
        public void ProjConvertToXY(int ProjIndex, string desProjParam, int pointCount, double[] X, double[] Y)
        {
            double[] Z = new double[X.Length];
            for (int i = 0; i < Z.Length; i++)
            {
                X[i] = (float)(X[i] * DEG2RAD);
                Y[i] = (float)(Y[i] * DEG2RAD);
                Z[i] = 0;
            }
            ProjConvert("+proj=latlong +ellps=WGS84", desProjParam, pointCount, X, Y, Z);
            //如果投影是等经纬，由弧度转角度
            if (ProjIndex == 1)
            {
                for (int i = 0; i < Z.Length; i++)
                {
                    X[i] = (float)(X[i] * RAD2DEG);
                    Y[i] = (float)(Y[i] * RAD2DEG);
                }
            }
            Z = null;

        }

        //多点投影转换-反投影
        public void ProjConvertToLatlon(string desProjParam, int pointCount, double[] X, double[] Y)
        {
            double[] Z = new double[X.Length];
            for (int i = 0; i < Z.Length; i++)
            {
                Z[i] = 0;
            }
            ProjConvert(desProjParam, "+proj=latlong +ellps=WGS84", pointCount, X, Y, Z);
            //如果投影是等经纬，由弧度转角度
            for (int i = 0; i < Z.Length; i++)
            {
                X[i] = (float)(X[i] * RAD2DEG);
                Y[i] = (float)(Y[i] * RAD2DEG);
            }

            Z = null;
        }

        //投影转换
        public void ProjConvert(string srcProjParam, string desProjParam, int pointCount, double[] X, double[] Y, double[] Z)
        {
            ////投影参数初始化
            IntPtr pjSrc = ProjWrapper.pj_init_plus(srcProjParam);
            IntPtr pjDes = ProjWrapper.pj_init_plus(desProjParam);
            ProjWrapper.pj_transform(pjSrc, pjDes, pointCount, 0, X, Y, Z);
            // X = null;
            // Y = null;
            Z = null;
        }
        
        //单点-反投影
        public PointF GetLatLonFromXY(PointF pointXY, MapHandle.config.ProjPara proj)
        {
            //投影参数初始化
            PointF result = new PointF();
            IntPtr pj = ProjWrapper.pj_init_plus(getSrcProj(proj));
            projUV temp = new projUV(pointXY.X, pointXY.Y);
            projUV coord = ProjWrapper.pj_inv(temp, pj);
            result.X = (float)(coord.U);
            result.Y = (float)(coord.V);
            result.X = (float)(result.X * RAD2DEG);
            result.Y = (float)(result.Y * RAD2DEG);
            return result;
        }

        //单点-正投影
        public PointF GetXYFromLatLon(PointF pointOrg, MapHandle.config.ProjPara proj)
        {
            pointOrg.X = (float)(pointOrg.X * DEG2RAD);
            pointOrg.Y = (float)(pointOrg.Y * DEG2RAD);
            //投影参数初始化
            IntPtr pj = ProjWrapper.pj_init_plus(getSrcProj(proj));
            projUV temp = new projUV(pointOrg.X, pointOrg.Y);
            projUV coord = ProjWrapper.pj_fwd(temp, pj);
            PointF pointPrj = new PointF();
            pointPrj.X = (float)(coord.U);
            pointPrj.Y = (float)(coord.V);
            return pointPrj;
        }

    }
}
