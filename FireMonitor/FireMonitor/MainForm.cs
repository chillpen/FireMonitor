using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using FireMonitor.DataProvider;
using SharpMap.Layers;
namespace FireMonitor
{
    public partial class MainForm : RibbonForm
    {
        public MainForm()
        {
            InitializeComponent();
            InitSkinGallery();
            InitGrid();

            FY3AVirrL1DataProvider provider = new FY3AVirrL1DataProvider();

            //provider.DataChanged += new EventHandler(provider_DataChanged);
           
            provider.L1File = "C:\\Data\\FY3A_VIRRX_GBAL_L1_20090427_0255_1000M_MS.HDF";
            this.imgDispCtrl1.DataProvider = provider;
            this.imgDispCtrl1.BorderDataProvider = provider;
            
            provider.DataChange();

            this.imgDispCtrl1.EnableGdiLayerRender();
        }

        void provider_DataChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }
        BindingList<Person> gridDataList = new BindingList<Person>();
        void InitGrid()
        {
            gridDataList.Add(new Person("John", "Smith"));
            gridDataList.Add(new Person("Gabriel", "Smith"));
            gridDataList.Add(new Person("Ashley", "Smith", "some comment"));
            gridDataList.Add(new Person("Adrian", "Smith", "some comment"));
            gridDataList.Add(new Person("Gabriella", "Smith", "some comment"));
            //gridControl.DataSource = gridDataList;
        }

        private void iNew_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void iOpen_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

    }
}