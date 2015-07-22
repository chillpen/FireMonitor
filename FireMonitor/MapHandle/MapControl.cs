using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpMap.Layers;
using SharpMap.Data.Providers;
using GeoAPI.Geometries;
using MapHandle.Selection;
using SharpMap.Rendering.Thematics;
using System.IO;

namespace MapHandle
{
    public partial class MapControl : UserControl
    {
        //SharpMap.Forms.MapBox mp = new SharpMap.Forms.MapBox();
        PointF p1 = new PointF();    //矩形框左上角点
        public PointF p_single = new PointF();   //鼠标移动时记录坐标的点
        public PointF p_shp = new PointF();      //点击shp时记录坐标的点
        //public PointF pts = new PointF();
        
        public selectType m_type = selectType.Rect;
        System.Drawing.Rectangle rect1 = new System.Drawing.Rectangle();    //绘制矩形框，不断刷新
        public System.Drawing.Rectangle rect2 = new System.Drawing.Rectangle();    //分析矩形框
        private bool isDraw = false;
        config.ProjPara pjPara;
        config cfg;
        Projection pj = new Projection();

        public SharpMap.Forms.MapBox CurrentMapBox{get;set;}

        private bool isShowToolbar = true;

        public bool isAnalyse { get; set; }

        public void setCfg(config incfg)
        {
            cfg = incfg;
            pjPara = incfg.pjPara;
        }

        public void setSelectType(selectType st)
        {
            m_type= st;
        }

        public enum selectType
        {
            Rect,
            Point,
            Polygon,
            NotValid,
        }
      
        public enum ProjType
        {
            Mercator,
            Lambert,
            Stereographic,
            Albers,
            Gauss,
            UTM,
            Latlon,
        }

        public struct ObjectBounds
        {
            public double LeftTopLatitude;//左上角纬度
            public double LeftTopLongitude;//左上角经度
            public double RightButtomLatitude;//右下角纬度
            public double RightButtomLongitude;//有下角经度
        }

        public MapControl()
        {
            InitializeComponent();
            this.mapBox.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.VisibleLayers;

            this.pictureBox1.Dock = this.mapBox.Dock;
            //this.pictureBox1.Width = this.mapBox.Width;
            //this.pictureBox1.Height = this.mapBox.Height;
            this.pictureBox1.Location = this.mapBox.Location;
            this.DoubleBuffered = true;
            this.pictureBox1.Visible = false;
            this.pictureBox1.Enabled = false;

            
            this.mapZoomToolStrip1.Visible = isShowToolbar;
            this.mapZoomToolStrip1.Items.RemoveAt(7);
            
        }

        
        public void SetMapshowBounds(ObjectBounds mapBounds)
        {
            Envelope envelop = new Envelope(mapBounds.LeftTopLongitude, mapBounds.RightButtomLongitude, mapBounds.LeftTopLatitude, mapBounds.RightButtomLatitude);
            // this.m_viewBox.Map.Zoom = 3;
            this.mapBox.Map.ZoomToBox(envelop);
            this.mapBox.Refresh();
        }

        private Bitmap KiResizeImage(Bitmap bmp, int newW, int newH, Color fillColor)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                b.MakeTransparent(fillColor);

                // b.MakeTransparent(Color.White);//取消注释
                return b;
            }
            catch
            {
                return null;
            }
        }

        public void addSHP(VectorLayer vl)
        {
            //vl.Envelope.Init(this.mapBox.Map.Envelope);
            
            this.mapBox.Map.Layers.Add(vl);
            //this.mapBox.Map.ZoomToExtents();
            this.mapBox.Refresh();
        }

        private void mapBox_MouseDown(Coordinate worldPos, MouseEventArgs imagePos)
        {
            //p1.X = imagePos.X;
            //p1.Y = imagePos.Y;
            //pictureBox1.Enabled = true;

            //if(m_type == selectType.Rect)
            //{
            //    this.pictureBox1.Enabled = true;
            //    this.pictureBox1.Visible = true;
            //    pictureBox1.BackgroundImage = mapBox.Map.GetMap();
            //    //pictureBox1.BackgroundImageLayout = ImageLayout.Center;


            //    Graphics g = pictureBox1.CreateGraphics();
            //    g.Clear(Color.Black);
            //}

        }

        private void mapBox_MouseUp(Coordinate worldPos, MouseEventArgs imagePos)
        {
            //PointF p2 = new PointF();
            //p2.X = imagePos.X;
            //p2.Y = imagePos.Y;
            //var rect = new System.Drawing.Rectangle((int)p1.X, (int)p1.Y, (int)Math.Abs(p2.X - p1.X), (int)Math.Abs(p1.Y - p2.Y));
            //var brush = new System.Drawing.SolidBrush(Color.Black);
            //var g = this.mapBox.CreateGraphics();
            //var p = new Pen(brush);
            //g.DrawRectangle(p, rect);
            //mapBox.Refresh();
            //g.Dispose();
            //p.Dispose();

            ////输出两点的大地坐标

            //sr.setPoints(mapBox.Map.ImageToWorld(p1), mapBox.Map.ImageToWorld(p2), rect.Width, rect.Height);
            //pictureBox1.Enabled = false;
            if (m_type == selectType.Polygon)
            {
                p_shp.X = (float)worldPos.X;
                p_shp.Y = (float)worldPos.Y;

                getPoints();
            }


        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraw)
            {
                //准备矩形框
                Point p2 = new Point();
                p2.X = e.X;
                p2.Y = e.Y;
                rect1 = new System.Drawing.Rectangle((int)p1.X, (int)p1.Y, (int)Math.Abs(p2.X - p1.X), (int)Math.Abs(p1.Y - p2.Y));

                //触发事件，传出矩形框
                if (RectangleSizeChanged != null)
                {
                    RectangleSizeChanged(rect1);
                }

                //isDraw = true;

                pictureBox1.Invalidate();

                //获取鼠标移动坐标
                p_single.X = e.X;
                p_single.Y = e.Y;

                //selectType tmp = m_type;
                //m_type = selectType.Point;
                List<PointF> pts = getPoints();

                if (pts.Count == 0)
                    return;

                p_single = pts[0];
                p_single = pj.GetLatLonFromXY(pts[0], pjPara);
                //m_type = tmp;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_type == selectType.Rect)
            {
                rect2.Width = 0;
                p1.X = e.X;
                p1.Y = e.Y;
                isDraw = true;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_type == selectType.Rect && pictureBox1.Visible == true)
            {
                Point p2 = new Point();
                p2.X = e.X;
                p2.Y = e.Y;
                rect2 = new System.Drawing.Rectangle((int)p1.X, (int)p1.Y, (int)Math.Abs(p2.X - p1.X), (int)Math.Abs(p1.Y - p2.Y));

                pictureBox1.Invalidate();
                isDraw = false;
                //pictureBox1.Visible = false;
                //pictureBox1.Enabled = false;

                //Graphics g = mapBox.CreateGraphics();

                if (m_type == selectType.Rect)
                {
                    //setSelectType(selectType.NotValid);
                    pictureBox1.Visible = false;
                    pictureBox1.Enabled = false;
                }

                if (RectangleSizeChangedCompleted != null)
                {
                    RectangleSizeChangedCompleted(this, null);
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (rect1 != null && rect1.Width > 0 && rect1.Height > 0 && isDraw == true)//移动中的矩形
            {
                pictureBox1.Invalidate();
                var brush = new System.Drawing.SolidBrush(Color.Red);
                var p = new Pen(brush);
                e.Graphics.DrawRectangle(p, rect1);
            }

            if (rect2 != null && rect2.Width > 0 && rect2.Height > 0)//mouse up时的最终分析矩形
            {
                var brush = new System.Drawing.SolidBrush(Color.Red);
                var p = new Pen(brush);
                e.Graphics.DrawRectangle(p, rect2);
            }

        }

        public List<PointF> getPoints()
        {
            //ISelectRegion ir;
            List<PointF> pts = new List<PointF>();
            switch (m_type)
            {
                case selectType.Rect:    //矩形框选
                    SelectRect ir = new SelectRect();
                    PointF tmpP = new PointF();
                    tmpP.X = rect2.Right;
                    tmpP.Y = rect2.Bottom;

                    ir.setPoints(mapBox.Map.ImageToWorld(p1), mapBox.Map.ImageToWorld(tmpP), rect2.Width, rect2.Height);
                    if (isAnalyse)
                    {
                        pts = ir.getSelectedPts();
                        isAnalyse = false;
                    }
                    break;
                case selectType.Point:
                    SelectPoint ip = new SelectPoint();
                    ip.setPoint(mapBox.Map.ImageToWorld(p_single));

                    pts = ip.getSelectedPts();


                    break;
                case selectType.Polygon:
                    SelectShp ishp = new SelectShp();
                    ishp.setConfig(cfg);
                    //高亮选中区域
                    VectorLayer veclayer = (VectorLayer)mapBox.Map.GetLayerByName("province");
                    VectorLayer v2 = ishp.getPolygonPoints(p_shp, veclayer);
                    if (mapBox.Map.GetLayerByName("Selection") != null)
                        removeShpLayer("Selection");
                    addSHP(v2);
                    this.mapBox.Refresh();
                    //获取shp多边形内的点
                    if (isAnalyse)
                    {
                        pts = ishp.getSelectedPts();
                        isAnalyse = false;
                    }
                    break;
                case selectType.NotValid:
                    break;

            }

            //if(pts.Count()>0&&m_type !=selectType.Point)
            //{
            //    for (int i = 0; i < pts.Count(); i++)
            //        pts[i] = this.mapBox.Map.WorldToImage(new Coordinate(pts[i].Y,pts[i].X));
            //}

            return pts;

            //ir.getSelectedPts();
        }

        private void mapBox_MouseMove(Coordinate worldPos, MouseEventArgs imagePos)
        {
            if (m_type == selectType.Point)
            {
                p_single.X = imagePos.X;
                p_single.Y = imagePos.Y;

                List<PointF> pts = getPoints();

                p_single = pts[0];

                if (pjPara.center_lat == pjPara.std_paralle1)
                    return;

                p_single = pj.GetLatLonFromXY(pts[0], pjPara);   //坐标转换

            }
        }

        public void removeShpLayer(string layername)
        {
            ILayer layer = this.mapBox.Map.GetLayerByName(layername);

            if (layer != null)
            {
                this.mapBox.Map.Layers.Remove(layer);
                this.mapBox.Refresh();
            }
        }

        public void removeImgLayer()
        {
            ILayer layer = null;
            if (this.mapBox.Map.Layers.Count != 0)
            {
                layer = this.mapBox.Map.Layers[0];
            }

            if (layer != null)
            {
                this.mapBox.Map.Layers.Remove(layer);
                ((Layer)layer).Dispose();
                this.mapBox.Refresh();
            }
        }

        //修改shp图层的外观，包括颜色和线宽
        public void setShpLayer(string layername, Color color, int width)
        {
            ILayer layer = this.mapBox.Map.GetLayerByName(layername);

            if (layer != null)
            {
                SharpMap.Styles.VectorStyle vs = new SharpMap.Styles.VectorStyle();
                //Line
                vs.Line.Color = color;
                vs.Line.Width = width;
                //Point
                vs.PointColor = new SolidBrush(color);
                vs.PointSize = width;
                //Polygon
                vs.Outline = new Pen(color, width);
                vs.Fill = new SolidBrush(Color.Transparent);
                vs.EnableOutline = true;

                (layer as VectorLayer).Style = vs;

                this.mapBox.Refresh();
            }
        }

        public double getPixelsize()
        {
            return mapBox.Map.PixelSize;
        }

        public void hidePicturebox()
        {
            this.pictureBox1.Visible = false;
            this.pictureBox1.Enabled = false;
        }

        public void showPicturebox()
        {
            this.pictureBox1.Enabled = true;
            this.pictureBox1.Visible = true;
        }

        public void setAnalysis(bool a)
        {
            isAnalyse = a;
        }

        //从路径加载图片
        public void addGDIlayer(string layername, string filepath, config cfg)
        {

            ImageGDILayer gdilayer = new ImageGDILayer(layername, filepath, cfg);
            this.mapBox.Map.Layers.Insert(0, gdilayer);
            this.mapBox.Map.ZoomToBox(gdilayer.Envelope);
            this.mapBox.Refresh();
        }


        //generate image
        IntPtr p;
        Image img;
        Bitmap mp;
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        //从内存加载图片
        public void addGDIlayer(string layername, IImageDataProvider datapro, config cfg)
        {
            //delete existing same layer
            ILayer layer = this.mapBox.Map.GetLayerByName(layername);

            if (layer != null)
            {

                if (mp != null)
                {
                    mp.Dispose();
                    mp = null;
                }

                if (img != null)
                {
                    img.Dispose();
                    img = null;
                }

                if (p != null)
                {
                    DeleteObject(p);
                }

                this.mapBox.Map.Layers.Remove(layer);
                ((Layer)layer).Dispose();

                GC.Collect();
            }
            //get data from data provider

            if (datapro == null)
            {
                DefaultData dd = new DefaultData();

                mp = dd.GetData();
            }
            else
            {
                mp = datapro.GetData();
            }

            p = mp.GetHbitmap();
            img = Image.FromHbitmap(p);

            ImageGDILayer gdilayer = new ImageGDILayer(layername, img, cfg);
            this.mapBox.Map.Layers.Insert(0, gdilayer);
            this.mapBox.Map.ZoomToBox(gdilayer.Envelope);
            this.mapBox.Refresh();

            //System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet = new System.IntPtr(5);
        }

        //
        public void setToolbarVis(bool isShow)
        {
            isShowToolbar = isShow;
            this.mapZoomToolStrip1.Visible = isShowToolbar;
        }

        public double[] getZoomEnvelope(config cfg)
        {
            double[] scale = new double[4];

            Envelope tmpEv = this.mapBox.Map.Envelope;

            double x_start = (tmpEv.MinX - cfg.minX) / (cfg.maxX - cfg.minX);
            double x_end = (tmpEv.MaxX - cfg.minX) / (cfg.maxX - cfg.minX);
            double y_start = (tmpEv.MinY - cfg.minY) / (cfg.maxY - cfg.minY);
            double y_end = (tmpEv.MaxY - cfg.minY) / (cfg.maxY - cfg.minY);

            scale[0] = x_start;
            scale[1] = x_end;
            scale[2] = y_start;
            scale[3] = y_end;

            for (int i = 0; i < scale.Count(); i++)
            {
                if (scale[i] < 0) scale[i] = 0;
                if (scale[i] > 1) scale[i] = 1;
            }
            return scale;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (m_type != selectType.Rect)
            {
                setSelectType(selectType.Rect);
            }
            //else
            //{
            //    if (m_type == selectType.Rect)
            //    {
            //        setSelectType(selectType.NotValid);
            //        pictureBox1.Visible = false;
            //        pictureBox1.Enabled = false;
            //    }
            //}
            this.pictureBox1.Enabled = true;
            this.pictureBox1.Visible = true;
            pictureBox1.BackgroundImage = mapBox.Map.GetMap();

            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.Black);
        }

        private void MapControl_Load(object sender, EventArgs e)
        {
            this.CurrentMapBox = mapBox;
        }

        //public PointF getPoint_0( )
        //{

        //    return this.mapBox.Map.WorldToImage(new Coordinate(cfg.minX,cfg.maxY));

        //}


        #region   比对相关


        /// <summary>
        /// 作比较相关
        /// </summary>

        public delegate void MapZoomChangedDelegate();
        public event MapZoomChangedDelegate MapZoomChanged;

        public delegate void MapMouseDragDelegate();
        public event MapMouseDragDelegate MapMouseDrag;

        public delegate void RectangleSizeChangedDelegate(Rectangle rect);
        public event RectangleSizeChangedDelegate RectangleSizeChanged;

        public event EventHandler RectangleSizeChangedCompleted;


        /// <summary>
        /// 大小变化时触发
        /// </summary>
        /// <param name="zoom"></param>
        private void mapBox_MapZoomChanged(double zoom)
        {
            if (MapZoomChanged != null)
            {
                MapZoomChanged();
            }
        }

        /// <summary>
        /// 鼠标拖动时触发
        /// </summary>
        /// <param name="worldPos"></param>
        /// <param name="imagePos"></param>
        private void mapBox_MouseDrag(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            if (MapMouseDrag != null)
            {
                MapMouseDrag();
            }
        }

        public ObjectBounds GetMapCenter()
        {
            PointF point1 = new PointF(0, 0);
            Size size = mapBox.ClientSize;
            PointF point2 = new PointF(size.Width, size.Height);
            ObjectBounds Showboduns = new ObjectBounds();
            Showboduns.LeftTopLatitude = (float)mapBox.Map.ImageToWorld(point1).Y;
            Showboduns.LeftTopLongitude = (float)mapBox.Map.ImageToWorld(point1).X;
            Showboduns.RightButtomLatitude = (float)mapBox.Map.ImageToWorld(point2).Y;
            Showboduns.RightButtomLongitude = (float)mapBox.Map.ImageToWorld(point2).X;
            return Showboduns;
        }

        #endregion

        private void mapBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(Pens.Red, rect2);
        }
    }
}