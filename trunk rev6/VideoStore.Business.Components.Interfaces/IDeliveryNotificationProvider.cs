using System;
using VideoStore.Business.Entities;

namespace VideoStore.Business.Components.Interfaces
{
    public interface IDeliveryNotificationProvider
    {
        void NotifyDeliverySubmission(Guid orderNumber, Guid pDeliveryId);

        void NotifyDeliveryCompletion(Guid pDeliveryId, DeliveryStatus status);
    }
}
