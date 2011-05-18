using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackupBank.Business.Entities
{
    public class OperationOutcome
    {
        public enum OperationOutcomeResult { Successful, Failure };

        public String Message { get; set; }
        public OperationOutcomeResult Outcome { get; set; }
    }
}
