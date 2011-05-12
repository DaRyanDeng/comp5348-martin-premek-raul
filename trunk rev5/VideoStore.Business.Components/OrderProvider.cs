using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;
using System.Transactions;
using VideoStore.Business.Components.TransferService;
using VideoStore.Business.Components.DeliveryService;
using Microsoft.Practices.ServiceLocation;

namespace VideoStore.Business.Components
{
    public class OrderProvider : IOrderProvider
    {
        public IEmailProvider EmailProvider
        {
            get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        }

        public void SubmitOrder(Entities.Order pOrder)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {

                try
                {
                    pOrder.OrderNumber = Guid.NewGuid();
                    TransferFundsFromCustomer(pOrder.Customer.BankAccountNumber, pOrder.Total ?? 0.0, pOrder.OrderNumber.ToString());
                    //PlaceDeliveryForOrder(pOrder);
                    lContainer.Orders.ApplyChanges(pOrder);
                    pOrder.UpdateStockLevels();
                    lContainer.SaveChanges();
                    lScope.Complete();
                    //SendOrderPlacedConfirmation(pOrder);
                }
                catch (Exception lException)
                {
                    SendOrderErrorMessage(pOrder, lException);
                    throw;
                }

            }
        }



        private void SendOrderErrorMessage(Order pOrder, Exception pException)
        {
            EmailProvider.SendMessage(new EmailMessage()
            {
                ToAddress = pOrder.Customer.Email,
                Message = "There was an error in processsing your order " + pOrder.OrderNumber + ": "+ pException.Message +". Please contact Video Store"
            });
        }



        private void TransferFundsFromCustomer(int pCustomerAccountNumber, double pTotal, String pDescription)
        {
            TransferServiceClient lClient = new TransferServiceClient();
            lClient.Transfer(pTotal, pCustomerAccountNumber, RetrieveVideoStoreAccountNumber(), pDescription);
        }



        private int RetrieveVideoStoreAccountNumber()
        {
            return 123;
        }


        public Order  GetOrderByOrderNumber(Guid pOrderNumber)
        {
            using(VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                return lContainer.Orders.Include("Customer").Where((pOrder) => (pOrder.OrderNumber == pOrderNumber)).FirstOrDefault();
            }
        }
}
}
