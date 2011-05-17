using System.ServiceModel;
using Common.Messages;
using DeliveryCo.Business.Components;
using DeliveryCo.Business.Components.Interfaces;
using DeliveryCo.Business.Entities;
using DeliveryCo.Services.Interfaces;

namespace DeliveryCo.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class DeliveryService : IDeliveryService
    {
        private IDeliveryProvider _deliveryProvider;
        private IDeliveryProvider DeliveryProvider
        {
            get { return _deliveryProvider ?? (_deliveryProvider = new DeliveryProvider()); }
        }

        public void SubmitDelivery(DeliveryInfo pDeliveryInfo)
        {
            DeliveryProvider.SubmitDelivery(pDeliveryInfo);
        }

        public void PublishToSubscriber(VideoMessage message)
        {
            if (message is DeliveryRequestMessage)
            {
                var deliveryMessage = message as DeliveryRequestMessage;

                var deliveryInfo = new DeliveryInfo
                {
                    OrderNumber = deliveryMessage.OrderNumber.ToString(),
                    SourceAddress = deliveryMessage.SourceAddress,
                    DestinationAddress = deliveryMessage.DestinationAddress,
                    DeliveryNotificationAddress = deliveryMessage.Publisher,
                    DeliveryIdentifier = deliveryMessage.Identifier,
                };

                DeliveryProvider.SubmitDelivery(deliveryInfo);
            }
        }
    }
}
