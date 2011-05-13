using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoStore.Business.Components.Interfaces
{
    public interface ITransferNotificationProvider
    {
        void NotifyTransferOutcome(bool pOutcome, string pMessage, string pDescription);
      
    }
}
