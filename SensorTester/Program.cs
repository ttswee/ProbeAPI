using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace SensorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var ps = new PerceiverAPIs.SpaceProbeClient();
            var dSpace = ps.GetDriveInfo();
            for (int i = 0; i< dSpace.Count() ; i++)
            {
                Console.WriteLine("{0} : {1}", dSpace[i].driveLetter, dSpace[i].freeSpace.ToString());

            }
            Console.WriteLine(dSpace);

            var MaintJobsAPI = new PerceiverAPIs.JobMaintenanceClient();
            var JobList = MaintJobsAPI.GetJobList();
            Console.WriteLine(JobList.GetType());
            foreach (PerceiverAPIs.MaintSch J in JobList)
            {
                Console.WriteLine("Job Name : {0}", J.JobName);
                Console.WriteLine("Job Type : {0}", J.JobType);
                Console.WriteLine("Job Folder Name : {0}", J.FolderName);
            }
            Console.ReadKey();


        }
    }

    public class DriveSpaces
    {
        public string driveLetter { get; set; }
        public long freeSpace { get; set; }
    }
}
