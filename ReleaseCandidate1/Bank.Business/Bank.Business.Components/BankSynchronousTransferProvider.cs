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
    public class BankSynchronousTransferProvider : IBankSynchronousTransferProvider
    {


        public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber)
        {
            String lLogMessage;
            
            try
            {
            using (TransactionScope lScope = new TransactionScope())
                using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
                {
                    //IOperationOutcomeService lOutcomeService = OperationOutcomeServiceFactory.GetOperationOutcomeService(pResultReturnAddress);
                     Account lFromAcct = GetAccountFromNumber(pFromAcctNumber);
                     Account lToAcct = GetAccountFromNumber(pToAcctNumber);
                     lFromAcct.Withdraw(pAmount);
                     lToAcct.Deposit(pAmount);
                     lContainer.Attach(lFromAcct);
                     lContainer.Attach(lToAcct);
                     lContainer.ObjectStateManager.ChangeObjectState(lFromAcct, System.Data.EntityState.Modified);
                     lContainer.ObjectStateManager.ChangeObjectState(lToAcct, System.Data.EntityState.Modified);
                     lContainer.SaveChanges();



                     lLogMessage = "Transfer Request Succeeded - received via WS* - (pAmount=" + pAmount + " , pFromAcctNumber=" + pFromAcctNumber + " , pToAcctNumber=" + pToAcctNumber + " )"; //we have to fill this within transaction scope or VS goes mad

                    
                    
                    lScope.Complete();
                    

                    //lOutcomeService.NotifyOperationOutcome(new OperationOutcome() { Outcome = OperationOutcome.OperationOutcomeResult.Successful });
                    
                }



            SystemWideLogging.LogServiceClient.LogEvent("Bank :: Bank.Business\\Bank.Business.Components\\BankSynchronousTransferProvider.cs  :: public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber)", lLogMessage);
                


            }
                catch (Exception lException)
                {

                    SystemWideLogging.LogServiceClient.LogEvent("Bank :: Bank.Business\\Bank.Business.Components\\BankSynchronousTransferProvider.cs  :: public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber)", "Transfer Request Failed - received via WS* - (pAmount=" + pAmount + " , pFromAcctNumber=" + pFromAcctNumber + " , pToAcctNumber=" + pToAcctNumber + " )");
               
                    
                    Console.WriteLine("Error occured while transferring money:  " + lException.Message);
                    throw;
                    //lOutcomeService.NotifyOperationOutcome(new OperationOutcome() { Outcome = OperationOutcome.OperationOutcomeResult.Failure, Message = lException.Message });
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
