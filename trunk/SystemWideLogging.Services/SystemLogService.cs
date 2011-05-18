using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemWideLogging.Services.Interfaces;
using SystemWideLogging.Business.Components.Interfaces;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;

namespace SystemWideLogging.Services
{
    public class SystemLogService : ISystemLogService
    {
        private ISystemLogProvider SystemLogProvider
        {
            get
            {
                return ServiceFactory.GetService<ISystemLogProvider>();
            }
        }

        public void LogEvent(string sSource, string sMessage)
        {
            SystemLogProvider.LogEvent(sSource, sMessage);
        }


    }
}
