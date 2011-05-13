using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Configuration;
using System.Messaging;
using System.ServiceModel;




namespace SystemWideLoggingClientNS
{
    public static class SystemWideLogging
    {

        static SystemWideLogClient.SystemLogService.SystemLogServiceClient _LogServiceClient;



        public static SystemWideLogClient.SystemLogService.SystemLogServiceClient LogServiceClient
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


            _LogServiceClient = new SystemWideLogClient.SystemLogService.SystemLogServiceClient();


        }


        static void Main(string[] args)
        {


        }

    }
    
}