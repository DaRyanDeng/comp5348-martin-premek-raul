using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackupBank.Business.Components.Interfaces
{
    public interface ITransferProvider
    {
        void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber);
    }
}
