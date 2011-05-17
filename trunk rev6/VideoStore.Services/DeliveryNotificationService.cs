using System;
using DeliveryCo.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;

namespace VideoStore.Services
{
    public class DeliveryNotificationService : IDeliveryNotificationService
    {
        static IDeliveryNotificationProvider Provider
        {
            get { return ServiceLocator.Current.GetInstance<IDeliveryNotificationProvider>(); }
        }

        public void NotifyDeliveryCompletion(Guid pDeliveryId, DeliveryInfoStatus status)
        {
            Provider.NotifyDeliveryCompletion(pDeliveryId, GetDeliveryStatusFromDeliveryInfoStatus(status));
        }

        public void NotifyDeliverySubmission(Guid orderNumber, Guid deliveryIdentifier)
        {
            Provider.NotifyDeliverySubmission(orderNumber, deliveryIdentifier);
        }

        private static DeliveryStatus GetDeliveryStatusFromDeliveryInfoStatus(DeliveryInfoStatus status)
        {
            if (status == DeliveryInfoStatus.Delivered)
            {
                return DeliveryStatus.Delivered;
            }
            if(status == DeliveryInfoStatus.Failed)
            {
                return DeliveryStatus.Failed;
            }
            if(status == DeliveryInfoStatus.Submitted)
            {
                return DeliveryStatus.Submitted;
            }
            throw new Exception("Unexpected delivery status received");
        }
        
    }
}
