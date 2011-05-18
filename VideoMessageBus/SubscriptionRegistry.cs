using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Threading;
using Common;
using Common.Messages;
using VideoMessageBus.Interfaces;


namespace VideoMessageBus
{
    public class SubscriptionRegistry
    {
        private static readonly Dictionary<string, List<string>> SubscribersList = new Dictionary<string, List<string>>();

        public static Dictionary<string, List<string>> Subscribers
        {
            get
            {
                lock (typeof(SubscriptionRegistry))
                {
                    return SubscribersList;
                }
            }
        }

        static public IEnumerable<string> GetSubscribers(String topicName)
        {
            lock (typeof(SubscriptionRegistry))
            {
                if (Subscribers.ContainsKey(topicName))
                {
                    return Subscribers[topicName];
                }
                return null;
            }
        }

        static public void AddSubscriber(String topicName, string subscriberReference)
        {
            lock (typeof(SubscriptionRegistry))
            {
                if (Subscribers.ContainsKey(topicName))
                {
                    if (!Subscribers[topicName].Contains(subscriberReference))
                    {
                        Subscribers[topicName].Add(subscriberReference);
                    }
                }
                else
                {
                    var newSubscribersList = new List<string> {subscriberReference};
                    Subscribers.Add(topicName, newSubscribersList);
                }

                // Check abandon queue to respond to any message during no available subscriber (in a thread)
                ThreadPool.QueueUserWorkItem(callback => CheckAbandoneMessages(topicName));
            }
        }

        static private void CheckAbandoneMessages(String topicName)
        {
            using (var queue = new MessageQueue(Program.AbandonMessagesQueuePath))
            {
                var formatter = new BinaryMessageFormatter();
                
                try
                {
                    //queue.Formatter = new XmlMessageFormatter(new [] { typeof(VideoMessage) });
                    queue.Formatter = formatter;
                    queue.MessageReadPropertyFilter = new MessagePropertyFilter {Body = true, Label = true,};

                    // Wait 0.1 second for a message to appear in queue 
                    int noOfMessage = 0;
                    Message message;
                    while((message = queue.Peek(TimeSpan.FromSeconds(.1))) != null)
                    {
                        if (message.Label == topicName &&
                            message.Body is VideoMessage)
                        {
                            noOfMessage++;

                            var vMessage = message.Body as VideoMessage;

                            // publish to subscriber
                            IPublisherService publisher = ServiceFactory.GetService<IPublisherService>(Program.PublisherServiceEndpointAddress);
                            publisher.Publish(vMessage);

                            // Take it out from queue
                            queue.ReceiveById(message.Id);
                        }
                        message.Dispose();
                    }
                    if (noOfMessage > 0)
                    {
                        ConsoleHelper.WriteLine(ConsoleColor.Blue, "{0} message(s) published to subscribers", noOfMessage);
                    }
                }
                catch
                {
                    // No matched message in queue 
                }
            }
        }

        static public void RemoveSubscriber(String topicName, string subscriberReference)
        {
            lock (typeof(SubscriptionRegistry))
            {
                if (Subscribers.ContainsKey(topicName))
                {
                    if (Subscribers[topicName].Contains(subscriberReference))
                    {
                        Subscribers[topicName].Remove(subscriberReference);
                    }
                }
            }
        }
    }
}
