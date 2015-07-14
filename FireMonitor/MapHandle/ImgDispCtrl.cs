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
                GdiImageLayer gdilayer = new GdiImageLayer("Test", value);
                this.mapBox1.Map.Layers.Add(gdilayer);


                this.mapBox1.Map.ZoomToExtents();
                this.mapBox1.Refresh();
            }
        }
    }
}
