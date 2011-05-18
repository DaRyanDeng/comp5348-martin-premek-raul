using System;
using Common;
using VideoMessageBus.Interfaces;

namespace VideoMessageBus
{
    public class SubscriptionService : ISubscriptionService
    {
        public void Subscribe(string topicName, string subscriberReference)
        {
            ConsoleHelper.WriteLine(ConsoleColor.Green, "Subscription for " + topicName + " received");
            SubscriptionRegistry.AddSubscriber(topicName, subscriberReference);
        }

        public void Unsubscribe(string topicName, string subscriberReference)
        {
            ConsoleHelper.WriteLine(ConsoleColor.Yellow, "Unsubscription for " + topicName + " received");
            SubscriptionRegistry.RemoveSubscriber(topicName, subscriberReference);
        }
    }
}
