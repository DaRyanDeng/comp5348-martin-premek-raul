using System;
using System.Messaging;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Common
{
    public class SubscriberServiceHost : IDisposable
    {
        private readonly ServiceHost _host;

        public SubscriberServiceHost(Type pServiceHostType, string pAddress, string pMexAddress, bool pCreateQueueIfNotExist = false, string pQueueAddress = null)
        {
            if (pCreateQueueIfNotExist && (pQueueAddress != null))
            {
                CreateQueueIfNotExist(pQueueAddress);
            }
            _host = new ServiceHost(pServiceHostType);
            
            var smb = new ServiceMetadataBehavior();
            _host.Description.Behaviors.Add(smb);
            
            _host.AddServiceEndpoint(typeof (ISubscriberService),
                                     new NetMsmqBinding(NetMsmqSecurityMode.None) {Durable = true}, pAddress);

            _host.AddServiceEndpoint(typeof (IMetadataExchange),
                                     MetadataExchangeBindings.CreateMexTcpBinding(),
                                     pMexAddress);
            _host.Open();
        }

        /// <summary>
        /// Create the transacted MSMQ queue if necessary.
        /// </summary>
        private static void CreateQueueIfNotExist(String pQueueAddress)
        {
            if (!MessageQueue.Exists(pQueueAddress))
                MessageQueue.Create(pQueueAddress, true);
        }

        public void Dispose()
        {
            if (_host != null)
            {
                _host.Close();
            }
        }
    }
}
