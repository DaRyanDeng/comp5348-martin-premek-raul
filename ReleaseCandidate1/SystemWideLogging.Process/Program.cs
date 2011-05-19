using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using SystemWideLogging.Services;
using System.ServiceModel.Configuration;
using System.Configuration;
using System.ComponentModel.Composition.Hosting;
using SystemWideLogging.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SystemWideLogging.Business.Entities;
using System.Transactions;
using System.ServiceModel.Description;
using SystemWideLogging.Business.Components.Interfaces;
using System.Messaging;
//using SystemWideLoggingNS;



namespace SystemWideLogging.Process
{
    public class Program
    {

        private static readonly String sPublishQueuePath = ".\\private$\\SystemWideLoggingServiceMessageQueue";



        static void Main(string[] args)
        {

            //SystemWideLogging.LogEvent_WithinOwnAppInstance("SystemWideLogging :: SystemWideLogging.Process\\Program.cs  :: static void Main(string[] args)", "Service started");
            


            EnsureMessageQueuesExists();
            ResolveDependencies();
            //InsertDummyEntities();
            HostServices();


            //SystemWideLogging.LogEvent_WithinOwnAppInstance("SystemWideLogging :: SystemWideLogging.Process\\Program.cs  :: static void Main(string[] args)", "Service ended");

        }




        private static void ResolveDependencies()
        {

            UnityContainer lContainer = new UnityContainer();
            UnityConfigurationSection lSection
                    = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            lSection.Containers["containerOne"].Configure(lContainer);
            UnityServiceLocator locator = new UnityServiceLocator(lContainer);
            ServiceLocator.SetLocatorProvider(() => locator);
        }

        private static void EnsureMessageQueuesExists()
        {
            // Create the transacted MSMQ queue if necessary.
            if (!MessageQueue.Exists(sPublishQueuePath))
                MessageQueue.Create(sPublishQueuePath, true);
        }


        private static void HostServices()
        {
            List<ServiceHost> lHosts = new List<ServiceHost>();
            try
            {

                Configuration lAppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ServiceModelSectionGroup lServiceModel = ServiceModelSectionGroup.GetSectionGroup(lAppConfig);

                System.ServiceModel.Configuration.ServicesSection lServices = lServiceModel.Services;
                foreach (ServiceElement lServiceElement in lServices.Services)
                {
                    ServiceHost lHost = new ServiceHost(Type.GetType(GetAssemblyQualifiedServiceName(lServiceElement.Name)));
                    lHost.Open();
                    lHosts.Add(lHost);
                }
                Console.WriteLine("SystemWideLogging Service Started, press Q key to quit");


                
                while (Console.ReadKey().Key != ConsoleKey.Q) ;
            }
            finally

            {

                foreach (ServiceHost lHost in lHosts)
                {
                    lHost.Close();
                }
            }
        }





        private static String GetAssemblyQualifiedServiceName(String pServiceName)
        {
            return String.Format("{0}, {1}", pServiceName, System.Configuration.ConfigurationManager.AppSettings["ServiceAssemblyName"].ToString());
        }
    }
}
