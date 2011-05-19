using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace BackupBank.Services.Interfaces
{
    [ServiceContract]
    public interface IBackupBankSynchronousTransferService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber);
    }
}
