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

namespace MapHandle
{
    public partial class ImgDispCtrl : UserControl
    {
        private IDataProvider m_dataProvider = null;
        public ImgDispCtrl()
        {
            InitializeComponent();
        }



        public SharpMap.Forms.MapBox MapBox
        {
            get { return this.mapBox1; }
            //set { m_MapBox = value; }
        }

        public IDataProvider DataProvider
        {
            set
            {
                m_dataProvider = value;
                GdiImageLayer gdilayer = new GdiImageLayer("Data", m_dataProvider);
                this.mapBox1.Map.Layers.Add(gdilayer);
                m_dataProvider.DataChangedEvent += new EventHandler(m_dataProvider_DataChangedEvent);

            }
        }

        void m_dataProvider_DataChangedEvent(object sender, EventArgs e)
        {
            this.mapBox1.Map.ZoomToExtents();
            this.mapBox1.Refresh();
        }
    }
}
