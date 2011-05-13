using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemWideLogging.Business.Entities;

namespace SystemWideLogging.Business.Components.Interfaces
{
    public interface ISystemLogProvider
    {
        void LogEvent(string sSource, string sMessage);
    }
}
