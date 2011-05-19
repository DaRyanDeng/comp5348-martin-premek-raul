using System;
using System.ServiceModel;

namespace DeliveryCo.Services.Interfaces
{
    public enum DeliveryInfoStatus { Submitted, Delivered, Failed }

    [ServiceContract]
    public interface IDeliveryNotificationService
    {
        [OperationContract]
        void NotifyDeliveryCompletion(Guid pDeliveryId, DeliveryInfoStatus status);

        [OperationContract]
        void NotifyDeliverySubmission(Guid orderNumber, Guid pDeliveryId);
    }
}
