using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireMonitor.Data
{
    public interface IDataFile
    {
        bool ReadData(string fileName);

        event EventHandler DataChangedEvent;
    }
}
