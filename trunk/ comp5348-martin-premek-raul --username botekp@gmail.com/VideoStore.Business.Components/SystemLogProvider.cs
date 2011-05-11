using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;
using System.Transactions;
using System.ComponentModel.Composition;

namespace VideoStore.Business.Components
{
    public class SystemLogProvider : ISystemLogProvider
    {
        public void LogEvent(string sSource, string sMessage)
        {
            
            VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer();
            
            SystemLog SystemLogItem = new SystemLog();

            SystemLogItem.EventTime = DateTime.Now;
            SystemLogItem.Source = sSource;
            SystemLogItem.Message = sMessage;

            lContainer.SystemLogs.AddObject(SystemLogItem);

            lContainer.SaveChanges();

        }



    }
}

