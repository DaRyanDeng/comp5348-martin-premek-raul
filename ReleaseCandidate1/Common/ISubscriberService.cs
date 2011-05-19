using System.ServiceModel;
using Common.Messages;

namespace Common
{
    [ServiceContract]
    public interface ISubscriberService
    {
        [OperationContract(IsOneWay = true)]
        void PublishToSubscriber(VideoMessage message);
    }
}
