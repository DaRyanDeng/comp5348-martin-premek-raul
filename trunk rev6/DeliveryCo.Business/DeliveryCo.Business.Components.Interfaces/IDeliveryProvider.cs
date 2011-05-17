using DeliveryCo.Business.Entities;

namespace DeliveryCo.Business.Components.Interfaces
{
    public interface IDeliveryProvider
    {
        void SubmitDelivery(DeliveryInfo pDeliveryInfo);
    }
}
