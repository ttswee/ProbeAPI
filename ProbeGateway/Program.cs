using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;
using System.Threading;
using FileMaintenance;
namespace SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://10.112.179.196:8080/ProbeAPI");
            using (ServiceHost host = new ServiceHost(typeof(GlobalAPI.ProbeSensor), baseAddress))
            {
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
                    if (!debug.IncludeExceptionDetailInFaults)
                    {
                        debug.IncludeExceptionDetailInFaults = true;
                    }
                }

                host.Open();

                Thread fileMainJob = new Thread(new ThreadStart(FMThread));
                fileMainJob.Start();
                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();
                fileMainJob.Abort();
                host.Close();
            }
        }


        private static void FMThread()
        {
            while (1 != 0)
            {
                Thread.Sleep(60000);
                MSch _MaintenanceJobs = new MSch();
                _MaintenanceJobs._AppPath = Directory.GetCurrentDirectory();
                List<MaintSch> _listOfJobs = _MaintenanceJobs.GetAllJobs();
                MaintenanceJobs _JobExcuter = new MaintenanceJobs();
                _JobExcuter._LogPath = Directory.GetCurrentDirectory();
                bool jStatus = false;
                foreach (MaintSch _Job in _listOfJobs)
                {
                    _JobExcuter._jobToExecute = _Job;
                    jStatus = _JobExcuter.RunJob();
                }
                _MaintenanceJobs = null;
            }
        }

    }
}