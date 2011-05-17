using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackupBank.Business.Entities;

namespace BackupBank.Services.Interfaces
{
    public interface IOperationOutcomeService
    {
        void NotifyOperationOutcome(OperationOutcome pOutcome); 
    }
}
