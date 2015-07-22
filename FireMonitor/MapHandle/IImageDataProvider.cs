using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapHandle
{
    public interface IImageDataProvider
    {
        System.Drawing.Bitmap GetData();

        event EventHandler ImageDataChangedEvent;

        void DataChange();
    }
}
