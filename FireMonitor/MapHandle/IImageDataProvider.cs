using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapHandle
{
    public interface IImageDataProvider
    {
        //System.Drawing.Bitmap CreateImageData();

        System.Drawing.Bitmap Image { get; }

        event EventHandler ImageDataChangedEvent;

        void OnDataChange();
    }
}
