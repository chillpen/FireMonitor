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

namespace MapHandle
{
    public partial class ImgDispCtrl : UserControl
    {
        private IImageDataProvider m_dataProvider = null;
        private IBorderDataProvider m_BorderDataProvider = null;

        private GdiImageLayer m_GdiImageLayer = new GdiImageLayer();
        public ImgDispCtrl()
        {
            InitializeComponent();
            m_GdiImageLayer.Enabled = false;
            this.mapBox1.Map.Layers.Add(m_GdiImageLayer);

            this.mapBox1.MouseClick += new MouseEventHandler(mapBox1_MouseClick);
            
        }

        void mapBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            Point pt = e.Location;
           // this.mapBox1.c
        }



        public SharpMap.Forms.MapBox MapBox
        {
            get { return this.mapBox1; }
            //set { m_MapBox = value; }
        }

        public IImageDataProvider DataProvider
        {
            set
            {
                m_dataProvider = value;
               // GdiImageLayer gdilayer = new GdiImageLayer("Data", m_dataProvider);
                m_GdiImageLayer.ImageDataProvider = m_dataProvider;
                
               // m_dataProvider.ImageDataChangedEvent += new EventHandler(m_dataProvider_DataChangedEvent);

            }
        }

        public IBorderDataProvider BorderDataProvider
        {
            set
            {
                m_BorderDataProvider = value;
                //GdiImageLayer gdilayer = new GdiImageLayer("Data", m_dataProvider);
               // this.mapBox1.Map.Layers.Add(m_BorderDataProvider);
                m_GdiImageLayer.BorderDataProvider = m_BorderDataProvider;
               // m_dataProvider.ImageDataChangedEvent += new EventHandler(m_dataProvider_DataChangedEvent);

            }
        }


        public void EnableGdiLayerRender()
        {
            m_GdiImageLayer.Enabled = true;
            Envelope envelope = new Envelope(0,1,0, this.m_dataProvider.Image.Height);
            this.mapBox1.Map.ZoomToBox(envelope);
            //this.mapBox1.Map.ZoomToExtents();
            this.mapBox1.Refresh();
        }

        //void m_dataProvider_DataChangedEvent(object sender, EventArgs e)
        //{
        //    //this.mapBox1.Map.z;
           
        //    this.mapBox1.Map.ZoomToExtents();
        //    this.mapBox1.Refresh();
        //}
    }
}
