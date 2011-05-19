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
    public class BackupBankSynchronousTransferService : IBackupBankSynchronousTransferService
    {
        private IBackupBankSynchronousTransferProvider TransferProvider
        {
            get { return ServiceLocator.Current.GetInstance<IBackupBankSynchronousTransferProvider>(); }
        }

        [OperationBehavior(TransactionScopeRequired=true)]
        public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber)
        {
            TransferProvider.Transfer(pAmount, pFromAcctNumber, pToAcctNumber);
        }
    }
}
