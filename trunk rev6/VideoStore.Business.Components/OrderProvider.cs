using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;
using System.Transactions;
using VideoStore.Business.Components.BankAsynchronousTransferService;
using VideoStore.Business.Components.DeliveryService;
using Microsoft.Practices.ServiceLocation;
using SystemWideLoggingClientNS;



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


           SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "New Order has been submitted via WebClient ( pOrder.Id=" + pOrder.Id + " )");
            

            using (TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {

                try
                {
                    pOrder.OrderNumber = Guid.NewGuid();

                    SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Requesting TransferFundsFromCustomer at Bank ( pOrder.Id=" + pOrder.Id + " )");
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
                    SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Error occured while requesting TransferFundsFromCustomer at Bank ( pOrder.Id=" + pOrder.Id + ", lException.Message=" + lException.Message + " )");
                    SendOrderErrorMessage(pOrder, lException);
                    throw;
                }

            }
        }



        private void SendOrderErrorMessage(Order pOrder, Exception pException)
        {

            SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: private void SendOrderErrorMessage(Order pOrder, Exception pException)", "Requesting EmailService to Send Order Error Message ( pOrder.Id=" + pOrder.Id + ", pException.Message=" + pException.Message + " )");
            
            try{
                String address = pOrder.Customer.Email;
                String message = "There was an error in processsing your order " + pOrder.OrderNumber + ": " + pException.Message + ". Please contact Video Store";
                EmailProvider.SendMessage(message, address);
            }

            catch (Exception lException)
            {
               SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: private void SendOrderErrorMessage(Order pOrder, Exception pException)", "Error occured while requesting EmailService to Send Order Error Message ( pOrder.Id=" + pOrder.Id + ", lException.Message=" + lException.Message + " )");
            }
        }



        private void TransferFundsFromCustomer(int pCustomerAccountNumber, double pTotal, String pDescription)
        {


            BankAsynchronousTransferServiceClient lClient = new BankAsynchronousTransferServiceClient();
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
