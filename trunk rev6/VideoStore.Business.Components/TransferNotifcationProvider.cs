using System;
using System.Linq;
using System.Transactions;
using Common.Messages;
using Microsoft.Practices.ServiceLocation;
using SystemWideLoggingClientNS;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Components.PublisherService;
using VideoStore.Business.Entities;

namespace VideoStore.Business.Components
{
    public class TransferNotifcationProvider : ITransferNotificationProvider
    {
        public IEmailProvider EmailProvider
        {
            get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        }

        public void NotifyTransferOutcome(bool pOutcome, string pMessage, string pDescription)
        {
            SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\TransferNotifcationProvider.cs :: public void NotifyTransferOutcome(bool pOutcome, string pMessage, string pDescription)", "Asynchronous Transfer Notification Received from Bank (pOutcome=" + pOutcome + " , pMessage=" + pMessage + " , pDescription=" + pDescription + " )");

            using (var lScope = new TransactionScope())
            {
                Order lOrder = ServiceLocator.Current.GetInstance<IOrderProvider>().GetOrderByOrderNumber(Guid.Parse(pDescription));
                if (pOutcome)
                {
                    PlaceDeliveryForOrder(lOrder);
                    SendEmailNotification(pOutcome, lOrder); 
                }
                else
                {
                    //roll back stock
                    RollbackOrder(lOrder.OrderNumber);
                    //send message to user
                    SendEmailNotification(pOutcome, lOrder); 
                }
                lScope.Complete();
            }
        }

        private static void RollbackOrder(Guid orderNumber)
        {
            using (var lScope = new TransactionScope())
            using (var lContainer = new VideoStoreEntityModelContainer())
            {
                Order order = lContainer.Orders.Where(o => o.OrderNumber == orderNumber).SingleOrDefault();
                if (order != null)
                {
                    order.RollbackStockLevels();
                    lContainer.SaveChanges();
                }
                lScope.Complete();
            }
        }

        private void PlaceDeliveryForOrder(Order pOrder)
        {
            var deliveryMessage = new DeliveryRequestMessage
            {
                Identifier = Guid.NewGuid(),
                DestinationAddress = pOrder.Customer.Address,
                SourceAddress = "Video Store Address",
                Publisher = "VideoStore.App",
                OrderNumber = pOrder.OrderNumber,
                TopicName = "Delivery",
                RegionName = "APAC/Australia",
            };

            PublisherService.Publish(deliveryMessage);

            var lDelivery = new Delivery
            {
                DeliveryStatus = DeliveryStatus.Submitted,
                SourceAddress = deliveryMessage.SourceAddress,
                DestinationAddress = deliveryMessage.DestinationAddress,
                Order = pOrder,
                ExternalDeliveryIdentifier = deliveryMessage.Identifier,
            };
            pOrder.Delivery = lDelivery;

            Console.WriteLine("Order '{0}' has submitted.", pOrder.OrderNumber);
        }


        private IPublisherService _publisherService;
        private IPublisherService PublisherService
        {
            get { return _publisherService ?? (_publisherService = new PublisherServiceClient()); }
        }

        private void SendEmailNotification(bool pOutcome, Order pOrder)
        {
            String message;
            if (pOutcome)
            {
                message = "your order " + pOrder.OrderNumber + " has been placed";
            }
            else
            {
                message = "Your order " + pOrder.OrderNumber + " has not been placed due to a problem with your credit";
            }
            EmailProvider.SendMessage(message, pOrder.Customer.Email);
        }
    }
}
