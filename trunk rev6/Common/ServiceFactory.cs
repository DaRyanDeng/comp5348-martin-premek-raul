using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Practices.ServiceLocation;

namespace Common
{
    public static class ServiceFactory
    {
        public static T GetService<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }

        public static T GetService<T>(String pAddress)
        {
            return GetChannelFactory<T>(pAddress).CreateChannel();
        }

        private static ChannelFactory<T1> GetChannelFactory<T1>(string pHandlerAddress)
        {
            Binding lBinding;
            if (pHandlerAddress.Contains("net.tcp"))
            {
                lBinding = new NetTcpBinding();
            }
            else if (pHandlerAddress.Contains("net.msmq"))
            {
                lBinding = new NetMsmqBinding(NetMsmqSecurityMode.None) { Durable = true };
            }
            else
            {
                throw new Exception("Unrecognized address type");
            }
            var myEndpoint = new EndpointAddress(pHandlerAddress);
            var myChannelFactory = new ChannelFactory<T1>(lBinding, myEndpoint);
            return myChannelFactory;
        }
    }
}
