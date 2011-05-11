using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackupBank.Business.Components.Interfaces;
using BackupBank.Business.Entities;
using System.Transactions;
using BackupBank.Services.Interfaces;

namespace BackupBank.Business.Components
{
    public class TransferProvider : ITransferProvider
    {


        public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (BackupBankEntityModelContainer lContainer = new BackupBankEntityModelContainer())
            {
                //IOperationOutcomeService lOutcomeService = OperationOutcomeServiceFactory.GetOperationOutcomeService(pResultReturnAddress);
                try
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
                    lScope.Complete();
                    //lOutcomeService.NotifyOperationOutcome(new OperationOutcome() { Outcome = OperationOutcome.OperationOutcomeResult.Successful });
                }
                catch (Exception lException)
                {
                    Console.WriteLine("Error occured while transferring money:  " + lException.Message);
                    throw;
                    //lOutcomeService.NotifyOperationOutcome(new OperationOutcome() { Outcome = OperationOutcome.OperationOutcomeResult.Failure, Message = lException.Message });
                }
            }
        }

        private Account GetAccountFromNumber(int pToAcctNumber)
        {
            using (BackupBankEntityModelContainer lContainer = new BackupBankEntityModelContainer())
            {
                return lContainer.Accounts.Where((pAcct) => (pAcct.AccountNumber == pToAcctNumber)).FirstOrDefault();
            }
        }
    }
}
