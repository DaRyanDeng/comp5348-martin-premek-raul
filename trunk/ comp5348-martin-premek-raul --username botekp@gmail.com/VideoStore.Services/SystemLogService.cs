using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Services.Interfaces;
using VideoStore.Business.Components.Interfaces;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;

namespace VideoStore.Services
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
