using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapHandle;
using HDF5DotNet;
using FireMonitor.HDFOper;
using System.Drawing.Imaging;
using System.Drawing;
using FireMonitor.SHPOperator;
using System.Windows.Forms;

namespace FireMonitor.Data
{
    public interface IDataCreator
    {
        IImageDataProvider ImageDataProvider { get; }

        IBorderDataProvider BorderDataProvider { get; }
    }
}
