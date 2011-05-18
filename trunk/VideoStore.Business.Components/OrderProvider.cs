using System;
using System.Linq;
using System.Transactions;
using Common;
using Microsoft.Practices.ServiceLocation;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Components.BankAsynchronousTransferService;
using VideoStore.Business.Entities;
using SystemWideLoggingClientNS;



namespace VideoStore.Business.Components
{
    public class OrderProvider : IOrderProvider
    {
        public IEmailProvider EmailProvider
        {
            get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        }

        public void SubmitOrder(Order pOrder)
        {


            SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "New Order has been submitted from WebClient ( pOrder.Id=" + pOrder.Id + " )");
           

            using (var lScope = new TransactionScope())
            using (var lContainer = new VideoStoreEntityModelContainer())
            {
                try
                {
                    pOrder.OrderNumber = Guid.NewGuid();

                    SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Requesting TransferFundsFromCustomer at Bank - asynchronously via MSMQ - ( pOrder.Id=" + pOrder.Id + " )");
                                     
                    TransferFundsFromCustomer(pOrder.Customer.BankAccountNumber, pOrder.Total ?? 0.0, pOrder.OrderNumber.ToString());
                    lContainer.Orders.ApplyChanges(pOrder);
                    pOrder.UpdateStockLevels();
                    lContainer.SaveChanges();
                    lScope.Complete();
                }
                catch (Exception lException)
                {
                    SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Error occured while requesting TransferFundsFromCustomer at Bank - asynchronously via MSMQ - ( pOrder.Id=" + pOrder.Id + ", lException.Message=" + lException.Message + " )");
                    ConsoleHelper.WriteLine(ConsoleColor.Red, "Error: " + lException.Message);
                    SendOrderErrorMessage(pOrder, lException);
                    throw;
                }
            }
        }

        private void SendOrderErrorMessage(Order pOrder, Exception pException)
        {
            SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: private void SendOrderErrorMessage(Order pOrder, Exception pException)", "Requesting EmailService to Send Order Error Message ( pOrder.Id=" + pOrder.Id + ", pException.Message=" + pException.Message + " )");

            try
            {
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

        public Order GetOrderByOrderNumber(Guid pOrderNumber)
        {
            using (var lContainer = new VideoStoreEntityModelContainer())
            {
                return lContainer.Orders.Include("Customer").Where((pOrder) => (pOrder.OrderNumber == pOrderNumber)).FirstOrDefault();
            }
        }

        private static int RetrieveVideoStoreAccountNumber()
        {
            return 123;
        }
    }
}
