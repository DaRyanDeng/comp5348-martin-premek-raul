using System;
using System.Linq;
using System.Transactions;
using Common;
using Microsoft.Practices.ServiceLocation;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Components.BankSynchronousTransferService;
using VideoStore.Business.Components.BankAsynchronousTransferService;
using VideoStore.Business.Components.BackupBankSynchronousTransferService;
using VideoStore.Services;
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

            String sLogMessage1 = "";
            String sLogMessage2 = "";
            String sLogMessage3 = "";
            String sLogMessage4 = "";
            String sSynchronousTransferRequestResultString="";
            String sOrderNumber = "";



            SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "New Order has been submitted from WebClient ( pOrder.Id=" + pOrder.Id + " )");


            using (var lScope = new TransactionScope())
            using (var lContainer = new VideoStoreEntityModelContainer())
            {
              
                
                
                pOrder.OrderNumber = Guid.NewGuid();

                sOrderNumber=pOrder.OrderNumber.ToString();


                //Payment Request High-Availability High-Reliability Magic :-):-):-)


                try  // try a Synchronous call to Bank first
                {

                    SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Requesting TransferFundsFromCustomer at Bank - synchronously via WS* - ( pOrder.Id=" + pOrder.Id + " )");

                    TransferFundsFromCustomer_BankSynchronousCall(pOrder.Customer.BankAccountNumber, pOrder.Total ?? 0.0);

                    sSynchronousTransferRequestResultString = "SynchronousTransferCallDestination:::Bank";


                }

                catch (Exception lException1)
                {
                    try  // if it fails, try a Synchronous call to BackupBank
                    {


                        SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Previous synchronous TransferFundsFromCustomer Request to Bank failed. Requesting TransferFundsFromCustomer at BackupBank - synchronously via WS* - ( pOrder.Id=" + pOrder.Id + " )");


                        TransferFundsFromCustomer_BackupBankSynchronousCall(pOrder.Customer.BankAccountNumber, pOrder.Total ?? 0.0);

                        sSynchronousTransferRequestResultString = "SynchronousTransferCallDestination:::BackupBank";


                    }

                    catch (Exception lException2)
                    {

                        try
                        {  // if it fails, try an Synchronous call to BackupBank


                            SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Previous synchronous TransferFundsFromCustomer Request to BackupBank failed. Requesting TransferFundsFromCustomer at Bank - asynchronously via MSMQ - ( pOrder.Id=" + pOrder.Id + " )");

                            TransferFundsFromCustomer_BankAsynchronousCall(pOrder.Customer.BankAccountNumber, pOrder.Total ?? 0.0, pOrder.OrderNumber.ToString());

                            //no need to call NotifyTransferOutcome here, Bank will do that itself via MSMQ

                        }
                        catch (Exception lException3)  // all calls failed, now we are really stuffed
                        {

                            SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "All three attempts to TransferFundsFromCustomer failed. We now have to apologize to the user and shut the business until probs are resolved - ( pOrder.Id=" + pOrder.Id + " )");

                        }

                    }
                }



                

                lContainer.Orders.ApplyChanges(pOrder);
                pOrder.UpdateStockLevels();
                lContainer.SaveChanges();
                lScope.Complete();

                

                if (sSynchronousTransferRequestResultString!="")
                {
                    try
                    {// let TransferNotificationProvider instance know that Transfer was successful
                        ServiceLocator.Current.GetInstance<ITransferNotificationProvider>().NotifyTransferOutcome(true, sSynchronousTransferRequestResultString, sOrderNumber);
                    }
                    catch (Exception lException4)  // all calls failed, now we are really stuffed
                    {

                        SystemWideLogging.LogServiceClient.LogEvent("VideoStore :: VideoStore.Business.Components\\OrderProvider.cs  :: public void SubmitOrder(Entities.Order pOrder)", "Error contacting TransferNotificationProvider instance ( lException4.Message=" + lException4.Message+" )");

                    }


                }


                //End of Payment Request High-Availability High-Reliability Magic :-):-):-)




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



        private void TransferFundsFromCustomer_BankSynchronousCall(int pCustomerAccountNumber, double pTotal)
        {
            BankSynchronousTransferServiceClient lClient = new BankSynchronousTransferServiceClient();
            lClient.Transfer(pTotal, pCustomerAccountNumber, RetrieveVideoStoreAccountNumber());
        }



        private void TransferFundsFromCustomer_BankAsynchronousCall(int pCustomerAccountNumber, double pTotal, String pDescription)
        {
            BankAsynchronousTransferServiceClient lClient = new BankAsynchronousTransferServiceClient();
            lClient.Transfer(pTotal, pCustomerAccountNumber, RetrieveVideoStoreAccountNumber(), pDescription);
        }



        private void TransferFundsFromCustomer_BackupBankSynchronousCall(int pCustomerAccountNumber, double pTotal)
        {

            BackupBankSynchronousTransferServiceClient lClient = new BackupBankSynchronousTransferServiceClient();
            lClient.Transfer(pTotal, pCustomerAccountNumber, RetrieveVideoStoreAccountNumber());
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
