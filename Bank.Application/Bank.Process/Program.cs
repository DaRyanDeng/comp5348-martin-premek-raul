﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Bank.Business.Entities;
using System.ServiceModel;
using Bank.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using System.Messaging;
using SystemWideLoggingClientNS;

namespace Bank.Process
{
    class Program
    {
        static void Main(string[] args)
        {
   
           SystemWideLogging.InitiateClass();

           SystemWideLogging.LogServiceClient.LogEvent("Bank :: Bank.Application\\Bank.Process\\Program.cs :: static void Main(string[] args)", "Service started");

            EnsureMessageQueuesExists();
            ResolveDependencies();
            EnsureMessageQueuesExists();
            CreateDummyEntities();
            HostServices();


           SystemWideLogging.LogServiceClient.LogEvent("Bank :: Bank.Application\\Bank.Process\\Program.cs :: static void Main(string[] args)", "Service ended");
        }

        private static void HostServices()
        {
            using (ServiceHost lHost = new ServiceHost(typeof(BankAsynchronousTransferService)))
            {
                lHost.Open();
                Console.WriteLine("Bank Services started. Press Q to quit.");
                while (Console.ReadKey().Key != ConsoleKey.Q) ;
            }
        }

        private static readonly String sPublishQueuePath = ".\\private$\\BankAsynchronousTransferQueueTransacted";
        private static void EnsureMessageQueuesExists()
        {
            // Create the transacted MSMQ queue if necessary.
            if (!MessageQueue.Exists(sPublishQueuePath))
                MessageQueue.Create(sPublishQueuePath, true);
        }





        private static void CreateDummyEntities()
        {
            using (TransactionScope lScope = new TransactionScope())
            using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
            {
                if (lContainer.Accounts.Count() == 0)
                {
                    Customer lVideoStore = new Customer();
                    Account lVSAccount = new Account() { AccountNumber = 123, Balance = 0 };
                    lVideoStore.Accounts.Add(lVSAccount);

                    Customer lCustomer = new Customer();
                    Account lCustAccount = new Account() { AccountNumber = 456, Balance = 20 };
                    lCustomer.Accounts.Add(lCustAccount);


                    lContainer.Customers.AddObject(lVideoStore);
                    lContainer.Customers.AddObject(lCustomer);


                    lContainer.SaveChanges();
                    lScope.Complete();
                }
            }
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
    }











}
