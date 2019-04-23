using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;
namespace SensorTester
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            var ps = new PerceiverAPIs.SpaceProbeClient();
            var dSpace = ps.GetDriveInfo();
            
            for (int i = 0; i< dSpace.Count() ; i++)
            {
                Console.WriteLine("Drive {0} : Used Space{1} : Total Space {2}", dSpace[i].driveLetter, dSpace[i].freeSpace.ToString(), dSpace[i].TotalSpace.ToString());

            }
            Console.WriteLine(dSpace);

            var psFile = new PerceiverAPIs.FolderMaintenanceClient();
            var fileBytes = psFile.GetFile("c:\\APPLCRES\\TDE\\CRES2EFORM_002.TXT");
            File.WriteAllBytes("c:\\swee\\fromserver3.txt", fileBytes);

            var MaintJobsAPI = new PerceiverAPIs.JobMaintenanceClient();
            var JobList = MaintJobsAPI.GetJobList();
            foreach (PerceiverAPIs.MaintSch J in JobList)
            {
                Console.WriteLine("Job Name : {0}", J.JobName);
                Console.WriteLine("Job Type : {0}", J.JobType);
                Console.WriteLine("Job Folder Name : {0}", J.FolderName);
            }


            Console.ReadKey();


        }
    }


}
