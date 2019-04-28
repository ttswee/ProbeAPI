using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;
using System.Data;
namespace SensorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            EndpointAddress address = new EndpointAddress("http://localhost:8080/perceiverapi");

            var ps = new PerceiverAPIs.SpaceProbeClient(binding,address);
            
            var dSpace = ps.GetDriveInfo();
            
            for (int i = 0; i< dSpace.Count() ; i++)
            {
                Console.WriteLine("Drive {0} : Used Space{1} : Total Space {2}", dSpace[i].driveLetter, dSpace[i].freeSpace.ToString(), dSpace[i].TotalSpace.ToString());

            }
            Console.WriteLine(dSpace);

            var psFile = new PerceiverAPIs.FolderMaintenanceClient(binding, address);
            psFile.ClientCredentials.UserName.UserName = "test";
            var fileBytes = psFile.GetFile("c:\\APPLCRES\\TDE\\CRES2EFORM_002.TXT");
            if ( fileBytes !=null && fileBytes.Length>0)
            File.WriteAllBytes("c:\\swee\\fromserver3.txt", fileBytes);

            var MaintJobsAPI = new PerceiverAPIs.JobMaintenanceClient(binding, address);
            var JobList = MaintJobsAPI.GetJobList();
            foreach (PerceiverAPIs.MaintSch J in JobList)
            {
                Console.WriteLine("Job Name : {0}", J.JobName);
                Console.WriteLine("Job Type : {0}", J.JobType);
                Console.WriteLine("Job Folder Name : {0}", J.FolderName);
            }

            //var cresapi = new PerceiverAPIs.CRESapiClient();
            //DataSet dt = new DataSet();
            //dt = cresapi.GetProcessAudit("4000003");
            //Console.WriteLine("Total Record : {0}",dt.Tables[0].Rows.Count);
            Console.ReadKey();
        }
    }


}
