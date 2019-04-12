using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileMaintenance;
using System.IO;
using System.Threading;
namespace MaintenanceJobClient
{
    class Program
    {
        internal static  bool bRun = true;
        static void Main(string[] args)
        {

            Thread fileMainJob = new Thread(new ThreadStart(FMThread));
            fileMainJob.Start();
            Console.ReadKey();
            bRun = false;

        }

        private static void FMThread()
        {
            while (bRun)
            {
                
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
                    Console.WriteLine("Running job : {0}", _Job.JobName);
                }
                _MaintenanceJobs = null;
                Thread.Sleep(60000);
            }
        }
    }
}
