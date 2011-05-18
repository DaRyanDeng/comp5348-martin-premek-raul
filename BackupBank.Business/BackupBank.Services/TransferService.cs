using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackupBank.Services.Interfaces;
using BackupBank.Business.Components.Interfaces;
using System.ServiceModel;
using Microsoft.Practices.ServiceLocation;

namespace BackupBank.Services
{
    public class TransferService : ITransferService
    {
        private ITransferProvider TransferProvider
        {
            get { return ServiceLocator.Current.GetInstance<ITransferProvider>(); }
        }

        [OperationBehavior(TransactionScopeRequired=true)]
        public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber)
        {
            TransferProvider.Transfer(pAmount, pFromAcctNumber, pToAcctNumber);
        }
    }
}
