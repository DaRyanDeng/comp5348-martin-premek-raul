using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank.Business.Components.Interfaces
{
    public interface IBankSynchronousTransferProvider
    {
        void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber);
    }
}
