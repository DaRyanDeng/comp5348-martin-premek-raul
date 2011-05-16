using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bank.Business.Components.Interfaces;
using Bank.Business.Entities;
using System.Transactions;
using Bank.Services.Interfaces;
using SystemWideLoggingClientNS;

namespace Bank.Business.Components
{
    public class TransferProvider : ITransferProvider
    {


        public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber, String pDescription)
        {
            bool lOutcome = true;
            String lMessage = "TransferSuccessful";
            String lLogMessage;
            try
            {
                using (TransactionScope lScope = new TransactionScope())
                using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
                {
                    Account lFromAcct = GetAccountFromNumber(pFromAcctNumber);
                    Account lToAcct = GetAccountFromNumber(pToAcctNumber);
                    lFromAcct.Withdraw(pAmount);
                    lToAcct.Deposit(pAmount);
                    lContainer.Attach(lFromAcct);
                    lContainer.Attach(lToAcct);
                    lContainer.ObjectStateManager.ChangeObjectState(lFromAcct, System.Data.EntityState.Modified);
                    lContainer.ObjectStateManager.ChangeObjectState(lToAcct, System.Data.EntityState.Modified);
                    lContainer.SaveChanges();
                    
                    lLogMessage = "Transfer Request Succeeded (pAmount=" + pAmount + " , pFromAcctNumber=" + pFromAcctNumber + " , pToAcctNumber=" + pToAcctNumber + " , pDescription=" + pDescription + " , lMessage= " + lMessage + " )"; //we have to fill this within transaction scope or VS goes mad

                    lScope.Complete();
                    }

                SystemWideLogging.LogServiceClient.LogEvent("Bank :: Bank.Business\\Bank.Business.Components\\TransferProvider.cs  :: public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber, String pDescription)", lLogMessage);
                

            }
            catch (Exception lException)
            {

                SystemWideLogging.LogServiceClient.LogEvent("Bank :: Bank.Business\\Bank.Business.Components\\TransferProvider.cs  :: public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber, String pDescription)", "Transfer Request Failed (pAmount=" + pAmount + " , pFromAcctNumber=" + pFromAcctNumber + " , pToAcctNumber=" + pToAcctNumber + " , pDescription=" + pDescription + " , lMessage= " + lMessage + " )");
                
                Console.WriteLine("Error occured while transferring money:  " + lException.Message);
                lMessage = lException.Message;
                lOutcome = false;
                throw;
            }
            finally
            {

                try
                {

                    TransferNotificationService.TransferNotificationServiceClient lClient = new TransferNotificationService.TransferNotificationServiceClient();
                    lClient.NotifyTransferOutcome(lOutcome, lMessage, pDescription);

                }
                catch (Exception lException)
                {
                    SystemWideLogging.LogServiceClient.LogEvent("Bank :: Bank.Business\\Bank.Business.Components\\TransferProvider.cs  :: public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber, String pDescription)", "Error occured when sending Notification of Transfer Outcome to VideoStore (lException.Message=" + lException.Message+ ")");
                }


            }

        }

        private Account GetAccountFromNumber(int pToAcctNumber)
        {
            using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
            {
                return lContainer.Accounts.Where((pAcct) => (pAcct.AccountNumber == pToAcctNumber)).FirstOrDefault();
            }
        }
    }
}
