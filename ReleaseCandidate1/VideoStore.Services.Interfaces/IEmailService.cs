using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VideoStore.Services.Interfaces
{
    [ServiceContract]
    public interface IEmailService
    {
        [OperationContract(IsOneWay = true)]
        void SendEmail(String message, String address);
    }
}
