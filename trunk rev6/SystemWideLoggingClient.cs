using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




namespace SystemWideLoggingClientNS
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


        static void Main(string[] args)
        {


        }

    }
    
}