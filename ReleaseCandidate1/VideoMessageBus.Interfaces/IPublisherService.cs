using System.ServiceModel;
using Common.Messages;

namespace VideoMessageBus.Interfaces
{
    [ServiceContract]
    //[ServiceKnownType(typeof(VideoMessage))]
    //[ServiceKnownType(typeof(DeliveryRequestMessage))]
    public interface IPublisherService
    {
        [OperationContract(IsOneWay = true)]
        void Publish(VideoMessage message);
    }
}
