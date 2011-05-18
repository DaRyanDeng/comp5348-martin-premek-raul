using System;
using System.Runtime.Serialization;

namespace Common.Messages
{
    [DataContract]
    [KnownType(typeof(DeliveryRequestMessage))]
    [Serializable]
    public abstract class VideoMessage
    {
        [DataMember]
        public string Publisher { get; set; }

        [DataMember]
        public string TopicName { get; set; }
    }
}
