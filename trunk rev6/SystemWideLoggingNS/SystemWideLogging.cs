using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components;
using VideoStore.Business.Entities;
using VideoStore.Business.Components.Interfaces;
using System.Transactions;
using System.ComponentModel.Composition;



namespace SystemWideLoggingNS
{
    public static class SystemWideLogging
    {

        static SystemLogService.SystemLogServiceClient _LogServiceClient;



        public static SystemLogService.SystemLogServiceClient LogServiceClient
        {
            get
            {
                return _LogServiceClient;
            }
            set
            {
                _LogServiceClient = value;
            }
        }



        public static void InitiateClass()
        {
            //this is a make up for constructor
            //static classes can not have one


            _LogServiceClient = new SystemLogService.SystemLogServiceClient();


        }



        public static void LogEvent_WithinOwnAppInstance(string sSource, string sMessage)
        {


            


            using (TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {

                
                    SystemLog SystemLogItem = new SystemLog();


                    SystemLogItem.EventTime = DateTime.Now;
                    SystemLogItem.Source = sSource;
                    SystemLogItem.Message = sMessage;

                    lContainer.SystemLogs.AddObject(SystemLogItem);

                    lContainer.SaveChanges();

                    lScope.Complete();

                

            }




        }




        static void Main(string[] args)
        {


        }

    }
    
}