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
    public class TransferNotificationProvider : ITransferNotificationProvider
    {
        public IEmailProvider EmailProvider
        {
            get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        }

        public void NotifyTransferOutcome(bool pOutcome, string pMessage, string pDescription)
        {
            String sLogMessage1 ="";
            
            
         

           using (var lScope = new TransactionScope())
           {


               switch (pMessage)
               {

                   case "SynchronousTransferCallDestination:::Bank":
                       sLogMessage1 = "Bank via WS*";
                       break;


                   case "SynchronousTransferCallDestination:::BackupBank":
                       sLogMessage1 = "BackupBank via WS*";
                       break;

                   default:
                       sLogMessage1 = "Bank via MSMQ";
                       break;
               }
               
               
               SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\TransferNotificationProvider.cs :: public void NotifyTransferOutcome(bool pOutcome, string pMessage, string pDescription)", "Transfer Confirmation Notification Received from " + sLogMessage1 + " (pOutcome=" + pOutcome + " , pMessage=" + pMessage + " , pDescription=" + pDescription + " )");
               lScope.Complete();
           }





            using (var lScope = new TransactionScope())
            {
                Order lOrder = ServiceLocator.Current.GetInstance<IOrderProvider>().GetOrderByOrderNumber(Guid.Parse(pDescription));
                if (pOutcome)
                {
                    SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\TransferNotificationProvider.cs :: public void NotifyTransferOutcome(bool pOutcome, string pMessage, string pDescription)", "Placing Delivery Request for order - via Pub/Sub - (pOutcome=" + pOutcome + " , pMessage=" + pMessage + " , pDescription=" + pDescription + " )");

                    PlaceDeliveryForOrder(lOrder);
                    SendEmailNotification(pOutcome, lOrder); 
                }
                else
                {
                    SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\TransferNotificationProvider.cs :: public void NotifyTransferOutcome(bool pOutcome, string pMessage, string pDescription)", "Rolling back my stock (pOutcome=" + pOutcome + " , pMessage=" + pMessage + " , pDescription=" + pDescription + " )");

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

            SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Requesting to Send Email Notification - via MSMQ - ( pOrder.Customer.Email = "+pOrder.Customer.Email+" , message="+message+" )");

            try
            {

                EmailProvider.SendMessage(message, pOrder.Customer.Email);
            }

            catch (Exception lException)
            {
                SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Error occured while requesting to Send Email Notification - via MSMQ - ( lException.Message=" + lException.Message + " ,pOrder.Customer.Email = " + pOrder.Customer.Email + " , message=" + message + " )");
                throw;
            }


        }
    }
}
