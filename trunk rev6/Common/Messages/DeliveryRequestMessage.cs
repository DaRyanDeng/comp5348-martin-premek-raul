using System;
using System.Runtime.Serialization;

namespace Common.Messages
{
    [DataContract]
    [Serializable]
    public class DeliveryRequestMessage : VideoMessage
    {
        [DataMember]
        public Guid Identifier { get; set; }

        [DataMember]
        public Guid OrderNumber { get; set; }

        [DataMember]
        public string SourceAddress { get; set; }

        [DataMember]
        public string DestinationAddress { get; set; }

        [DataMember]
        public string RegionName { get; set; }
    }
}
