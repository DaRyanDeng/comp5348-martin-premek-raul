using System;
using System.Collections.Generic;
using System.Messaging;
using System.ServiceModel;
using Common;
using Common.Messages;
using VideoMessageBus.Interfaces;
using SystemWideLoggingClientNS;




namespace VideoMessageBus
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class PublisherService : IPublisherService
    {
        public void Publish(VideoMessage message)
        {



            if (message is DeliveryRequestMessage)
            {


                SystemWideLogging.LogServiceClient.LogEvent("MessageBus :: VideoMessageBus\\PublisherService.cs :: public void Publish(VideoMessage message)", "Received DeliveryRequestMessage Publish Request from VideoStore ( message.TopicName=" + message.TopicName + " )");
           
                
                var deliveryMessage = message as DeliveryRequestMessage;

                // Find the scubscriber who matches with region of delivery address
                string keyword = deliveryMessage.TopicName + "/" + deliveryMessage.RegionName;

                IEnumerable<string> subscribers = SubscriptionRegistry.GetSubscribers(keyword);

                if (subscribers == null ||
                    ((IList<string>)subscribers).Count == 0)
                {
                    // Save the message

                    SystemWideLogging.LogServiceClient.LogEvent("MessageBus :: VideoMessageBus\\PublisherService.cs :: public void Publish(VideoMessage message)", "Can't find any subscribers, storing message in queue ( message.TopicName=" + message.TopicName + " )");
           

                    PutMessageIntoQueue(message);
                    return;
                }


                foreach (string subscriberAddress in subscribers)
                {
                    var service = ServiceFactory.GetService<ISubscriberService>(subscriberAddress);

                    if (service == null)
                    {



                        SystemWideLogging.LogServiceClient.LogEvent("MessageBus :: VideoMessageBus\\PublisherService.cs :: public void Publish(VideoMessage message)", "Following service was unsubscribed because no such address is available: " + subscriberAddress);
           
                        
                        PutMessageIntoQueue(message);

                        // It must be a dead service so unsubscribe it
                        SubscriptionRegistry.RemoveSubscriber(message.TopicName, subscriberAddress);

                        ConsoleHelper.WriteLine(ConsoleColor.Yellow,
                                                "Following service was unsubscribed because no such address is available: " +
                                                subscriberAddress);
                        return;
                    }

                    try
                    {

                        SystemWideLogging.LogServiceClient.LogEvent("MessageBus :: VideoMessageBus\\PublisherService.cs :: public void Publish(VideoMessage message)", "Forwarding message to subscriber ( subscriberAddress="+subscriberAddress+" , message.TopicName=" + message.TopicName + " )");
           
                        
                        service.PublishToSubscriber(message);

                        Console.WriteLine("Request sent to " + subscriberAddress);
                    }
                    catch
                    {
                        // It must be a dead service so unsubscribe it
                        SubscriptionRegistry.RemoveSubscriber(message.TopicName, subscriberAddress);

                        ConsoleHelper.WriteLine(ConsoleColor.Red,
                                                "An failue has occurred to publish the message to " + subscriberAddress +
                                                " so the service was unsubscribed!");
                    }
                }
            }
        }

        /// <summary>
        /// Keep the message in abandon queue until a new subscriber becomes available
        /// </summary>
        private static void PutMessageIntoQueue(VideoMessage message)
        {
            var formatter = new BinaryMessageFormatter();

            using (var queue = new MessageQueue(Program.AbandonMessagesQueuePath))
            {
                queue.Formatter = formatter;

                // transactional queue
                using (var transaction = new MessageQueueTransaction())
                {
                    transaction.Begin();

                    if (message is DeliveryRequestMessage)
                    {
                        using (var msg = new Message(message, formatter)
                                            {
                                                ResponseQueue = queue,
                                                Priority = MessagePriority.Normal,
                                                Label = message.TopicName + "/" + (message as DeliveryRequestMessage).RegionName,
                                                Recoverable = true,
                                            })
                        {
                            queue.Send(msg, transaction);
                        }
                        ConsoleHelper.WriteLine(ConsoleColor.Cyan, "Request stored in queue until proper service becomes available");
                    }
                    transaction.Commit();
                } // using (transaction)
            } // using (queue)
        }
    }
}
