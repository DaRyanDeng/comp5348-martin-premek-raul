using System;
using System.Configuration;
using System.ServiceModel;
using Common;
using DeliveryCo.Process.SubscriptionService;
using DeliveryCo.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;

namespace DeliveryCo.Process
{
    public class DeliveryLauncher : IDisposable
    {
        internal static readonly string DeliveryServiceEndpointAddress = "net.tcp://localhost:9030/DeliveryService";

        internal static readonly string SubscriptionServiceEndpointAddress = "net.tcp://localhost:9011/SubscriptionService";

        public DeliveryLauncher()
        {
            SubscribeForEvents();
            Console.WriteLine("Delivery Service has subscribed to receive events.");
            ConsoleHelper.WriteLine(ConsoleColor.Green, "Press Q to quit. DO NOT CLOSE ME!");
        }

        public void Run()
        {
            ResolveDependencies();
            using (var lHost = new ServiceHost(typeof(DeliveryService)))
            {
                lHost.Open();
                Console.WriteLine("Delivery Service started.");

                while (Console.ReadKey().Key != ConsoleKey.Q);
            }
        }

        public void Dispose()
        {
            ConsoleHelper.WriteLine(ConsoleColor.Yellow, "Delivery Service has unsubscribed.");
            UnsubscribeForEvents();
        }

        private static string DeliveryCentre
        {
            get { return ConfigurationManager.AppSettings["DeliveryCentreName"]; }
        }

        private static string TopicServicePath
        {
            get { return "Delivery/" + DeliveryCentre; }
        }

        private static void SubscribeForEvents()
        {
            // Subscribe for events
            ISubscriptionService subscriptionService = ServiceFactory.GetService<ISubscriptionService>(SubscriptionServiceEndpointAddress);

            subscriptionService.Subscribe(TopicServicePath, DeliveryServiceEndpointAddress);
        }

        private static void UnsubscribeForEvents()
        {
            // Subscribe for events
            ISubscriptionService subscriptionService = ServiceFactory.GetService<ISubscriptionService>(SubscriptionServiceEndpointAddress);

            subscriptionService.Unsubscribe(TopicServicePath, DeliveryServiceEndpointAddress);
        }

        private static void ResolveDependencies()
        {
            var lContainer = new UnityContainer();
            var lSection = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            lSection.Containers["containerOne"].Configure(lContainer);
            var locator = new UnityServiceLocator(lContainer);
            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }
}
