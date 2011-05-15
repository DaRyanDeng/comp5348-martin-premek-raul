using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemWideLogging.Business.Components.Interfaces;
using SystemWideLogging.Business.Entities;
using System.Transactions;
using System.ComponentModel.Composition;

namespace SystemWideLogging.Business.Components
{
    public class SystemLogProvider : ISystemLogProvider
    {
        public void LogEvent(string sSource, string sMessage)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (SystemWideLoggingEntities lContainer = new SystemWideLoggingEntities())
            {
                SystemLog SystemLogItem = new SystemLog();
                SystemLogItem.EventTime = DateTime.Now;
                SystemLogItem.Source = sSource;
                SystemLogItem.Message = sMessage;

                lContainer.SystemLog.AddObject(SystemLogItem);

                lContainer.SaveChanges();

                lScope.Complete();
            }
        }
    }
}

