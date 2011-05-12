using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using VideoStore.Business.Entities;
using VideoStore.Business.Components.DeliveryService;
using Microsoft.Practices.ServiceLocation;
using VideoStore.Business.Components.Interfaces;

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
            using (TransactionScope lScope = new TransactionScope())
            {
                if (pOutcome)
                {
                    Order lOrder = ServiceLocator.Current.GetInstance<IOrderProvider>().GetOrderByOrderNumber(Guid.Parse(pDescription));
                    PlaceDeliveryForOrder(lOrder);
                    SendOrderPlacedConfirmation(lOrder);
                }
                else
                {
                    //roll back stock
                    //send message to user
                }
                lScope.Complete();
            }
        }

        private void PlaceDeliveryForOrder(Order pOrder)
        {
            using(VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            using (TransactionScope lScope = new TransactionScope())
            {
                lContainer.Orders.Attach(pOrder);

                Delivery lDelivery = new Delivery() { DeliveryStatus = DeliveryStatus.Submitted, SourceAddress = "Video Store Address", DestinationAddress = pOrder.Customer.Address };


                
                DeliveryServiceClient lClient = new DeliveryServiceClient();
                Guid lDeliveryIdentifier = lClient.SubmitDelivery(new DeliveryInfo()
                {
                    OrderNumber = pOrder.OrderNumber.ToString(),
                    SourceAddress = lDelivery.SourceAddress,
                    DestinationAddress = lDelivery.DestinationAddress,
                    DeliveryNotificationAddress = "net.tcp://localhost:9010/DeliveryNotificationService"
                });

                lDelivery.ExternalDeliveryIdentifier = lDeliveryIdentifier;
                lDelivery.Order = pOrder;
                lContainer.Deliveries.AddObject(lDelivery);
                lContainer.SaveChanges();
                lScope.Complete();

            }

        }

        private void SendOrderPlacedConfirmation(Order pOrder)
        {
            EmailProvider.SendMessage(new EmailMessage()
            {
                ToAddress = pOrder.Customer.Email,
                Message = "Your order " + pOrder.OrderNumber + " has been placed"
            });
        }
    }
}
