using System;
using System.Linq;
using System.Transactions;
using Common;
using Microsoft.Practices.ServiceLocation;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;

namespace VideoStore.Business.Components
{
    public class DeliveryNotificationProvider : IDeliveryNotificationProvider
    {
        public IEmailProvider EmailProvider
        {
            get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        }

        public void NotifyDeliverySubmission(Guid orderNumber, Guid pDeliveryId)
        {
            using (var lScope = new TransactionScope())
            using (var lContainer = new VideoStoreEntityModelContainer())
            {
                Order order = lContainer.Orders.Include("Customer").Where(o => o.OrderNumber == orderNumber).FirstOrDefault();
                Delivery delivery = lContainer.Deliveries.Where(d => d.ExternalDeliveryIdentifier == pDeliveryId).FirstOrDefault();

                // Link them together
                if (order != null && delivery != null)
                {
                    order.Delivery = delivery;
                    lContainer.SaveChanges();
                    lScope.Complete();

                    EmailProvider.SendMessage(order.Customer.Email, "Your order " + orderNumber + " has been placed");
                }
            }
        }

        public void NotifyDeliveryCompletion(Guid pDeliveryId, DeliveryStatus status)
        {
            Order lAffectedOrder = RetrieveDeliveryOrder(pDeliveryId);

            if (lAffectedOrder == null)
            {
                ConsoleHelper.WriteLine(ConsoleColor.Red, "Failed to find order with delivery id='{0}'", pDeliveryId);
                return;
            }

            UpdateDeliveryStatus(pDeliveryId, status);

            switch(status)
            {
                case DeliveryStatus.Delivered:
                    EmailProvider.SendMessage(lAffectedOrder.Customer.Email,
                                              "Our records show that your order" + lAffectedOrder.OrderNumber +
                                              " has been delivered. Thank you for shopping at video store");
                    break;

                case DeliveryStatus.Failed:
                    // Rollback items to stock
                    RollbackOrder(pDeliveryId);
                    EmailProvider.SendMessage(lAffectedOrder.Customer.Email,
                                              "Our records show that there was a problem" + lAffectedOrder.OrderNumber +
                                              " delivering your order. Please contact Video Store");
                    break;
            }
        }

        private static void RollbackOrder(Guid pDeliveryId)
        {
            using (var lScope = new TransactionScope())
            using (var lContainer = new VideoStoreEntityModelContainer())
            {
                Delivery lDelivery =  lContainer.Deliveries.Include("Order.Customer").Where((pDel) => pDel.ExternalDeliveryIdentifier == pDeliveryId).FirstOrDefault();
                lDelivery.Order.RollbackStockLevels();
                lContainer.SaveChanges();
                lScope.Complete();
            }
        }

        private static void UpdateDeliveryStatus(Guid pDeliveryId, DeliveryStatus status)
        {
            using (var lScope = new TransactionScope())
            using (var lContainer = new VideoStoreEntityModelContainer())
            {
                Delivery lDelivery = lContainer.Deliveries.Where((pDel) => pDel.ExternalDeliveryIdentifier == pDeliveryId).FirstOrDefault();
                if (lDelivery != null)
                {
                    lDelivery.DeliveryStatus = status;
                    lContainer.SaveChanges();
                }
                lScope.Complete();
            }
        }

        private static Order RetrieveDeliveryOrder(Guid pDeliveryId)
        {
 	        using(var lContainer = new VideoStoreEntityModelContainer())
            {
                Delivery lDelivery =  lContainer.Deliveries.Include("Order.Customer").Where((pDel) => pDel.ExternalDeliveryIdentifier == pDeliveryId).FirstOrDefault();
                
                return (lDelivery == null) ? null : lDelivery.Order;
            }
        }
    }


}
