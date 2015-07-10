using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestProject
{
    public partial class ImgDispCtrlForm : Form
    {
        public ImgDispCtrlForm()
        {
            InitializeComponent();
        }

        public MapHandle.ImgDispCtrl ImgDispCtrl
        {
            get { return this.imgDispCtrl1; }
        }
    }
}
