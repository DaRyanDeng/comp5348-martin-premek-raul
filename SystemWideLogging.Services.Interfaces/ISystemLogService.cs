using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SystemWideLogging.Business.Entities;
using System.ServiceModel;

namespace SystemWideLogging.Services.Interfaces
{
    [ServiceContract]
    public interface ISystemLogService
    {
        [OperationContract(IsOneWay = true)]
        void LogEvent(string sSource, string sMessage);

    }
}
