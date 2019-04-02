using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;
using System.Threading;

namespace SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://10.112.179.196:8080/ProbeAPI");
            //var APIs = new ProbeGateway.APIs();
            // Create the ServiceHost.
            using (ServiceHost host = new ServiceHost(typeof(ProbeGateway.ProbeSensor), baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

                host.Description.Behaviors.Add(smb);

                ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();

                if (debug == null)
                {
                    host.Description.Behaviors.Add(
                         new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
                }
                else
                {
                    // make sure setting is turned ON
                    if (!debug.IncludeExceptionDetailInFaults)
                    {
                        debug.IncludeExceptionDetailInFaults = true;
                    }
                }

                host.Open();

                //Start file maintenance job
                Thread fileMainJob = new Thread(new ThreadStart(FMThread));

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();
               
                // Close the ServiceHost.
                host.Close();
            }
        }


        private static void FMThread()
        {
            //while 
        }

    }
}