using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bank.Services.Interfaces;
using Bank.Business.Components.Interfaces;
using System.ServiceModel;
using Microsoft.Practices.ServiceLocation;

namespace Bank.Services
{
    public class BankSynchronousTransferService : IBankSynchronousTransferService
    {
        private IBankSynchronousTransferProvider TransferProvider
        {
            get { return ServiceLocator.Current.GetInstance<IBankSynchronousTransferProvider>(); }
        }

        [OperationBehavior(TransactionScopeRequired=true)]
        public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber)
        {
            TransferProvider.Transfer(pAmount, pFromAcctNumber, pToAcctNumber);
        }
    }
}
