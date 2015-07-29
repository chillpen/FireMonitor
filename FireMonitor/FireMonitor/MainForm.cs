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
using FireMonitor.Data;
using SharpMap.Layers;
namespace FireMonitor
{
    public partial class MainForm : RibbonForm
    {
        public MainForm()
        {
            InitializeComponent();
            InitSkinGallery();
           // InitGrid();



            
        }

        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }
        BindingList<Person> gridDataList = new BindingList<Person>();
        void InitGrid()
        {

        }

        private void iNew_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void iOpen_ItemClick(object sender, ItemClickEventArgs e)
        {
            FY3AVirrDataCreator dataCreator = new FY3AVirrDataCreator();



            dataCreator.L1File = "C:\\Data\\FY3A_VIRRX_GBAL_L1_20090427_0255_1000M_MS.HDF";
            this.imgDispCtrl1.ImageDataProvider = dataCreator.ImageDataProvider;
            this.imgDispCtrl1.BorderDataProvider = dataCreator.BorderDataProvider;

            dataCreator.ImageDataProvider.Update();

            this.imgDispCtrl1.EnableGdiLayerRender();
        }

    }
}