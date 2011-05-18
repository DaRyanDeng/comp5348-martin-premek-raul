using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank.Business.Components.Interfaces
{
    public interface IBankAsynchronousTransferProvider
    {
        void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber, String pDescription);
    }
}
