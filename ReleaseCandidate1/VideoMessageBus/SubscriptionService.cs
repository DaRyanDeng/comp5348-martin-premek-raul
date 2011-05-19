using System;
using Common;
using VideoMessageBus.Interfaces;
using SystemWideLoggingClientNS;



namespace VideoMessageBus
{
    public class SubscriptionService : ISubscriptionService
    {
        public void Subscribe(string topicName, string subscriberReference)
        {

            SystemWideLogging.LogServiceClient.LogEvent("MessageBus :: VideoMessageBus\\SubscriptionService.cs :: public void Subscribe(string topicName, string subscriberReference)", "Subscription Request for " + topicName + " received");
           
            ConsoleHelper.WriteLine(ConsoleColor.Green, "Subscription for " + topicName + " received");
            SubscriptionRegistry.AddSubscriber(topicName, subscriberReference);
        }

        public void Unsubscribe(string topicName, string subscriberReference)
        {

            SystemWideLogging.LogServiceClient.LogEvent("MessageBus :: VideoMessageBus\\SubscriptionService.cs :: public void Unsubscribe(string topicName, string subscriberReference)", "Unsubscription Request for " + topicName + " received");
            
            ConsoleHelper.WriteLine(ConsoleColor.Yellow, "Unsubscription for " + topicName + " received");
            SubscriptionRegistry.RemoveSubscriber(topicName, subscriberReference);
        }
    }
}
