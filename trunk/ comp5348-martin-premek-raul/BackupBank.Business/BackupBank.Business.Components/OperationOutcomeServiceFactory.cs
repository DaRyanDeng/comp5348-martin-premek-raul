using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackupBank.Services.Interfaces;
using System.ServiceModel;

namespace BackupBank.Business.Components
{
    public class OperationOutcomeServiceFactory
    {
        public static IOperationOutcomeService GetOperationOutcomeService(String pAddress)
        {
            ChannelFactory<IOperationOutcomeService> lChannelFactory = new ChannelFactory<IOperationOutcomeService>(new NetTcpBinding(), new EndpointAddress(pAddress));
            return lChannelFactory.CreateChannel();
        }
    }
}
