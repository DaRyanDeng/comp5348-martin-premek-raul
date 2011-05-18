using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Services.Interfaces;
using VideoStore.Business.Components.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace VideoStore.Services
{
    public class TransferNotificationService : ITransferNotificationService
    {
        public ITransferNotificationProvider TransferNotificationProvider
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ITransferNotificationProvider>();
            }
        }
        public void NotifyTransferOutcome(bool pOutcome, string pMessage, string pDescription)
        {
            TransferNotificationProvider.NotifyTransferOutcome(pOutcome, pMessage, pDescription);
        }
    }
}
