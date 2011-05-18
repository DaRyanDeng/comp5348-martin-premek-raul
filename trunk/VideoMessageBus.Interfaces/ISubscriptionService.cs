using System.ServiceModel;

namespace VideoMessageBus.Interfaces
{
    [ServiceContract]
    public interface ISubscriptionService
    {
        [OperationContract]
        void Subscribe(string topicName, string subscriberReference);

        [OperationContract]
        void Unsubscribe(string topicName, string subscriberReference);
    }
}
