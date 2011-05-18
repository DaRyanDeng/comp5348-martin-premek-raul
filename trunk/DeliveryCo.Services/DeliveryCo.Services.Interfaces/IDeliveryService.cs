using System.ServiceModel;
using Common;
using DeliveryCo.Business.Entities;

namespace DeliveryCo.Services.Interfaces
{
    [ServiceContract]
    public interface IDeliveryService : ISubscriberService
    {
        [OperationContract]
        void SubmitDelivery(DeliveryInfo pDeliveryInfo);
    }
}
