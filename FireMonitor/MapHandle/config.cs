using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapHandle
{
    public class config
    {

        public string fullPath { get; set; }
        //public string ProjPara { get; set; }
        public float resolution { get; set; }

        public int height{ get; set; }
        public int width{ get; set; }

        public float minX { get; set; }
        public float minY { get; set; }
        public float maxX { get; set; }
        public float maxY { get; set; }

        public float upperLeftLat { get; set; }
        public float upperLeftLon { get; set; }
        public float lowerRightLat { get; set; }
        public float lowerRightLon { get; set; }

        public int fillValue{ get; set; }

        public ProjPara pjPara = new ProjPara();

        public struct ProjPara
        {
            public MapHandle.Projection.ProjType type;

            public float false_easting;     //假东，通常为0
            public float false_northing;    //假北，通常为0
            public float latitude_origin;   //原点纬度，通常为0，即赤道
            public float scale_factor;      //比例尺，通常为1
            public float unit;              //单位，通常为1，即1米

            public float center_lat;        //中心纬度
            public float center_lon;        //中心经度
            public float std_paralle1;      //标准纬圈1
            public float std_paralle2;      //标准纬圈2
            public float cen_meridian;      //标准经度
        }

        public void initProjPara( )
        {
            pjPara.false_easting = 0;
            pjPara.false_northing = 0;
            pjPara.latitude_origin = 0;
            pjPara.scale_factor = 1;
            pjPara.unit = 1;

            pjPara.type = Projection.ProjType.Lambert;
            pjPara.center_lat = 36;
            pjPara.center_lon = 116;
            pjPara.cen_meridian = 116;
            pjPara.std_paralle1 = 25;
            pjPara.std_paralle2 = 47;

            
        }
    }
}
