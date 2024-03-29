﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using BackupBank.Business.Entities;
using System.ServiceModel;
using BackupBank.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using SystemWideLoggingClientNS;




namespace BackupBank.Process
{
    class Program
    {
        static void Main(string[] args)
        {

            SystemWideLogging.InitiateClass();


            SystemWideLogging.LogServiceClient.LogEvent("BackupBank :: BackupBank.Application\\BackupBank.Process\\Program.cs :: public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber, String pDescription)", "Service started");
           
            
            ResolveDependencies();
            CreateDummyEntities();
            HostServices();


            SystemWideLogging.LogServiceClient.LogEvent("BackupBank :: BackupBank.Application\\BackupBank.Process\\Program.cs :: public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber, String pDescription)", "Service ended");

        }

        private static void HostServices()
        {
            using (ServiceHost lHost = new ServiceHost(typeof(TransferService)))
            {
                lHost.Open();
                Console.WriteLine("BackupBank Services started. Press Q to quit.");
                while (Console.ReadKey().Key != ConsoleKey.Q) ;
            }
        }

        private static void CreateDummyEntities()
        {
            using (TransactionScope lScope = new TransactionScope())
            using (BackupBankEntityModelContainer lContainer = new BackupBankEntityModelContainer())
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
