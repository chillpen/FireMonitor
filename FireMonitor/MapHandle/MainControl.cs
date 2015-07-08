using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapHandle.Selection;
using GeoAPI.Geometries;

namespace MapHandle
{
    public partial class MainControl : UserControl
    {
        //ImageLayer imglayer;
        ImageLayer imglayer = new ImageLayer();
        
        public MainControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetPalette(IPalette palette)
        {
            new ImageLayer().Palette = palette;
        }

        private void MainControl_Load(object sender, EventArgs e)
        {
            this.mapControl1.MapZoomChanged += new MapControl.MapZoomChangedDelegate(mapControl1_MapZoomChanged);
            this.mapControl1.MapMouseDrag += new MapControl.MapMouseDragDelegate(mapControl1_MapMouseDrag);
            this.mapControl1.RectangleSizeChanged += new MapControl.RectangleSizeChangedDelegate(mapControl1_RectangleSizeChanged);
            this.mapControl1.RectangleSizeChangedCompleted += new EventHandler(mapControl1_RectangleSizeChangedCompleted);
        }

        void mapControl1_RectangleSizeChangedCompleted(object sender, EventArgs e)
        {
            if (RectangleSizeChangedCompleted != null)
            {
                RectangleSizeChangedCompleted(this, null);
            }
        }

        public void SetRect(Rectangle rect)
        {
            if (rect != null)
            {
                this.mapControl1.rect2 = rect;
                this.mapControl1.Refresh();
            }
        }

        void mapControl1_RectangleSizeChanged(Rectangle rect)
        {
            if (RectangleSizeChanged != null)
            {
                RectangleSizeChanged(rect);
            }
        }

        void mapControl1_MapMouseDrag()
        {
            if (MapMouseDrag != null)
            {
                MapMouseDrag();
            }
        }

        void mapControl1_MapZoomChanged()
        {
            if (MapZoomChanged != null)
            {
                MapZoomChanged();
            }
        }


        public delegate void MapZoomChangedDelegate();
        public event MapZoomChangedDelegate MapZoomChanged;

        public delegate void MapMouseDragDelegate();
        public event MapMouseDragDelegate MapMouseDrag;

        public delegate void RectangleSizeChangedDelegate(Rectangle rect);
        public event RectangleSizeChangedDelegate RectangleSizeChanged;

        public event EventHandler RectangleSizeChangedCompleted;

        public ObjectBounds GetMapCenter()
        {
            //PointF point1 = new PointF(0, 0);
            //Size size = mapControl1.ClientSize;
            //PointF point2 = new PointF(size.Width, size.Height);
            //ObjectBounds Showboduns = new ObjectBounds();
            //Showboduns.LeftTopLatitude = (float)mapControl1.CurrentMapBox.Map.ImageToWorld(point1).Y;
            //Showboduns.LeftTopLongitude = (float)mapControl1.CurrentMapBox.Map.ImageToWorld(point1).X;
            //Showboduns.RightButtomLatitude = (float)mapControl1.CurrentMapBox.Map.ImageToWorld(point2).Y;
            //Showboduns.RightButtomLongitude = (float)mapControl1.CurrentMapBox.Map.ImageToWorld(point2).X;
            //return Showboduns;

            Envelope envelop = mapControl1.CurrentMapBox.Map.Envelope;
            ObjectBounds Showboduns = new ObjectBounds();

            Showboduns.LeftTopLatitude = envelop.MinX;
            Showboduns.LeftTopLongitude = envelop.MinY;
            Showboduns.RightButtomLatitude = envelop.MaxX;
            Showboduns.RightButtomLongitude = envelop.MaxY;

            return Showboduns;
        }

        public void ChangedMapCenter(ObjectBounds Bounds,MainControl mainControl)
        {
            if (mainControl.mapControl1.CurrentMapBox.Map != null)
            {
                //    if (Bounds.LeftTopLatitude > 0 && Bounds.RightButtomLatitude > 0 && Bounds.LeftTopLongitude > 0 && Bounds.RightButtomLongitude > 0)
                //    { 
                Envelope envelop = new Envelope(Bounds.LeftTopLatitude, Bounds.RightButtomLatitude, Bounds.LeftTopLongitude, Bounds.RightButtomLongitude);
                mainControl.mapControl1.CurrentMapBox.Map.ZoomToBox(envelop);
                mainControl.mapControl1.CurrentMapBox.Refresh();
                //}
            }
        }


        //add shp
        public void addSHPLayer(string layername, string fullPath, Color color, int width, MapHandle.config.ProjPara pjPara)
        {
            SHPLayer shplayer = new SHPLayer();
            //shplayer.getCFG(cfg);

            mapControl1.addSHP(shplayer.getSHPLayer(layername, fullPath,color, width, pjPara));
        }

        public config getCoordinate(config cfg)
        {
            Projection pj = new Projection();
            PointF p1 = new PointF();
            PointF p2 = new PointF();
            PointF p1_proj = new PointF();
            PointF p2_proj = new PointF();
            
            p1.X = cfg.upperLeftLon; p1.Y = cfg.upperLeftLat;
            p1_proj = pj.GetXYFromLatLon(p1,cfg.pjPara);

            p2.X = cfg.lowerRightLon; p2.Y = cfg.lowerRightLat;
            p2_proj = pj.GetXYFromLatLon(p2, cfg.pjPara);

            //cfg.maxX = p2_proj.X;
            //cfg.minX = p1_proj.X;
            //cfg.maxY = p1_proj.Y;
            //cfg.minY = p2_proj.Y;

            cfg.minX = p1_proj.X;
            cfg.maxX = cfg.minX + cfg.resolution * cfg.width;
            cfg.minY = p2_proj.Y;
            cfg.maxY = cfg.minY + cfg.resolution * cfg.height;

            return cfg;
        }

        public void setSelectType(MapControl.selectType st)
        {
            mapControl1.setSelectType(st);
        }

        public void setcfg(config cfg)
        {
            mapControl1.setCfg(cfg);

        }

        public short[] getpoints(config cfg )
        {
            //得到选取的所有点大地坐标
            List<PointF> pts = mapControl1.getPoints();

            //选取的点与数据进行筛选
            short[] selectData;
            SelectImageData imgdata = new SelectImageData();


            selectData = imgdata.getSelectData(pts, cfg.width * cfg.height, cfg,mapControl1.getPixelsize());

            return selectData;
            
        }

        public PointF getSinglePoint()
        {
            return mapControl1.p_single;
        }

        public void removeshplayer(string layername)
        {
            mapControl1.removeShpLayer(layername);
        }

        public void removeimglayer( )
        {
            mapControl1.removeImgLayer();
        }

        public void setshplayer(string layername, Color color, int width)
        {
            mapControl1.setShpLayer(layername, color, width);
        }

        public void SetImageLayer(IDataProvider provider)
        {
            imglayer.dataPro = provider;
        }

        public void hidepicturebox()
        {
            mapControl1.hidePicturebox();
        }

        public void setanalysis(bool a)
        {
            mapControl1.setAnalysis(a);
        }

        //test gdi layer
        public void addgdilayer(string layername, string filepath,config incfg)
        {
            //config cfg = getCoordinate(incfg);
            Envelope en = new Envelope(incfg.minX, incfg.maxX, incfg.minY, incfg.maxY);
            mapControl1.addGDIlayer(layername,filepath,incfg);
        }

        public void addgdilayer(string layername, IDataProvider datapro, config incfg)
        {
            //config cfg = getCoordinate(incfg);
            Envelope en = new Envelope(incfg.minX, incfg.maxX, incfg.minY, incfg.maxY);
            mapControl1.addGDIlayer(layername, datapro, incfg);
        }
       
        public void settoolbarvis(bool isShow)
        {
            mapControl1.setToolbarVis(isShow);
        }

        public double[] getEagleEnvelope(config cfg)
        {
            double[] result = mapControl1.getZoomEnvelope(cfg);
            return result;
        }

    }

    /// <summary>
    /// 图片的左上个角点坐标和右下角点坐标
    /// </summary>
    public struct ObjectBounds
    {
        public double LeftTopLatitude;//左上角纬度
        public double LeftTopLongitude;//左上角经度
        public double RightButtomLatitude;//右下角纬度
        public double RightButtomLongitude;//有下角经度
    }
}
