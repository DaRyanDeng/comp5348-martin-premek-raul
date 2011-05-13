using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VideoStore.Services.Interfaces
{
    [ServiceContract]
    public interface ITransferNotificationService
    {
        [OperationContract(IsOneWay=true)]
        void NotifyTransferOutcome(bool pOutcome, String pMessage, String pDescription);
    }
}
