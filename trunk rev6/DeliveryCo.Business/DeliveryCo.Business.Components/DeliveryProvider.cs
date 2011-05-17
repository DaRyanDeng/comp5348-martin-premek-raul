using System;
using System.Threading;
using System.Transactions;
using Common;
using DeliveryCo.Business.Components.Interfaces;
using DeliveryCo.Business.Entities;
using DeliveryCo.Services.Interfaces;

namespace DeliveryCo.Business.Components
{
    public class DeliveryProvider : IDeliveryProvider
    {
        private const string DeliveryNotificationServiceEndpointAddress = "net.tcp://localhost:9010/DeliveryNotificationService";

        public void SubmitDelivery(DeliveryInfo pDeliveryInfo)
        {
            using(var lScope = new TransactionScope())
            using(var lContainer = new DeliveryDataModelContainer())
            {
                pDeliveryInfo.DeliveryIdentifier = pDeliveryInfo.DeliveryIdentifier;
                pDeliveryInfo.Status = 0;
                lContainer.DeliveryInfoes.AddObject(pDeliveryInfo);
                lContainer.SaveChanges();
                ThreadPool.QueueUserWorkItem(pObj => ScheduleDelivery(pDeliveryInfo));
                lScope.Complete();
            }
            
            // Send notification back to VideoStore App
            var notificationService = ServiceFactory.GetService<IDeliveryNotificationService>(DeliveryNotificationServiceEndpointAddress);
            notificationService.NotifyDeliverySubmission(Guid.Parse(pDeliveryInfo.OrderNumber), pDeliveryInfo.DeliveryIdentifier);

            Console.WriteLine("Delivery request for order '{0}' has submitted", pDeliveryInfo.OrderNumber);
        }

        private static void ScheduleDelivery(DeliveryInfo pDeliveryInfo)
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));

            // Send final delivery message to VideoStore App
            var notificationService = ServiceFactory.GetService<IDeliveryNotificationService>(DeliveryNotificationServiceEndpointAddress);
            notificationService.NotifyDeliveryCompletion(pDeliveryInfo.DeliveryIdentifier, DeliveryInfoStatus.Delivered);

            ConsoleHelper.WriteLine(ConsoleColor.Green, "Order item with number='{0}' to address '{1}' has delivered",
                                    pDeliveryInfo.OrderNumber,
                                    pDeliveryInfo.DestinationAddress);
        }
    }
}
