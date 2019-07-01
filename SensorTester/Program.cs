using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
namespace SensorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //BasicHttpBinding binding = new BasicHttpBinding();
            //binding.Security.Mode = BasicHttpSecurityMode.None;
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            //EndpointAddress address = new EndpointAddress(string.Format(ConfigurationManager.AppSettings["globalapiuri"], ConfigurationManager.AppSettings["serverIP"]));
            //ChannelFactory<GlobalAPI.ISpaceProbe> SpaceChannel = new ChannelFactory<GlobalAPI.ISpaceProbe>(binding, address);
            //GlobalAPI.ISpaceProbe _SpaceChannel = SpaceChannel.CreateChannel();


            //var dSpace = _SpaceChannel.GetDriveInfo();
            //for (int i = 0; i < dSpace.Count(); i++)
            //{
            //    Console.WriteLine("Drive {0} : Used Space{1} : Total Space {2}", dSpace[i].driveLetter, dSpace[i].freeSpace.ToString(), dSpace[i].TotalSpace.ToString());

            //}
            //Console.WriteLine(dSpace);

            ////var psFile = new PerceiverAPIs.FolderMaintenanceClient(binding, address);
            ////psFile.ClientCredentials.UserName.UserName = "test";
            ////var fileBytes = psFile.GetFile("c:\\APPLCRES\\TDE\\CRES2EFORM_002.TXT");
            ////if (fileBytes != null && fileBytes.Length > 0)
            ////    File.WriteAllBytes("c:\\swee\\fromserver3.txt", fileBytes);


            //ChannelFactory<GlobalAPI.IJobMaintenance> JobChannel = new ChannelFactory<GlobalAPI.IJobMaintenance>(binding, address);
            //GlobalAPI.IJobMaintenance _Jobchannel = JobChannel.CreateChannel();
            //var JobList = _Jobchannel.GetJobList();
            //foreach (FileMaintenance.MaintSch J in JobList)
            //{
            //    Console.WriteLine("Job Name : {0}", J.JobName);
            //    Console.WriteLine("Job Type : {0}", J.JobType);
            //    Console.WriteLine("Job Folder Name : {0}", J.FolderName);
            //}

            try
            {
                BasicHttpBinding CRESbinding = new BasicHttpBinding();
                CRESbinding.Security.Mode = BasicHttpSecurityMode.None;
                CRESbinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                CRESbinding.MaxReceivedMessageSize = 2000000;
                EndpointAddress CRESaddress = new EndpointAddress(string.Format(ConfigurationManager.AppSettings["CRESapiUri"], ConfigurationManager.AppSettings["serverIP"]));

                ChannelFactory<CRESapi.ICRESapi> CRESApiFac = new ChannelFactory<CRESapi.ICRESapi>(CRESbinding, CRESaddress);
                var CRESServiceContract = CRESApiFac.CreateChannel();
                int stop = 0;
                string caseno = "";
                List<CRESapi.interfaceFiles> Results = new List<CRESapi.interfaceFiles>();
                while (stop == 0)
                {
                    Console.WriteLine("Enter case number or enter to end :");
                    caseno = Console.ReadLine();
                    if (caseno == "")
                    {
                        stop = 1;
                        break;
                    }
                    string secToken = genSecToken();
                    Results = CRESServiceContract.GetInterFaceFile(caseno, "91,92,93,94,95,96,97", "EBBS", secToken);
                    if (Results.Count > 0)
                    {
                        string destfolder = "c:\\xml\\";
                        if (!Directory.Exists(destfolder))
                        {
                            Directory.CreateDirectory(destfolder);
                        }
                        if (!Directory.Exists(Path.Combine(destfolder,caseno)))
                        {
                            Directory.CreateDirectory(Path.Combine(destfolder,caseno));
                        }
                        foreach (CRESapi.interfaceFiles ff in Results)
                        {

                            File.WriteAllBytes(Path.Combine(Path.Combine(destfolder, caseno), ff.fileName), decryptedFile( ff.encFile,secToken));
                        }
                        Console.WriteLine(string.Format("Total files copied : {0}", Results.Count));
                    }
                    else
                    {
                        Console.WriteLine("          _ ._  _ , _ ._");
                        Console.WriteLine("        (_ ' ( `  )_  .__)");
                        Console.WriteLine("      ( (  (    )   `)  ) _)");
                        Console.WriteLine("     (__ (_   (_ . _) _) ,__)");
                        Console.WriteLine("         `~~`\\ ' . /`~~`");
                        Console.WriteLine("              ;   ;");
                        Console.WriteLine("              /   \\");
                        Console.WriteLine("_____________/_ __ \\_____________");
                    }
                } 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }

        }


        private static string genSecToken()
        {
            string secToken5mins = "";
            string macName = Environment.MachineName;
            string userID = Environment.UserName;
            DateTime forENC = DateTime.Now;
            string unEnc = macName + "!" + userID + "!" + forENC;
            byte[] uncText = Encoding.Unicode.GetBytes(unEnc);
            string EncryptKey = "cresrocks";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pik = new Rfc2898DeriveBytes(EncryptKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pik.GetBytes(32);
                encryptor.IV = pik.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(uncText, 0, uncText.Length);
                        cs.Close();
                    }
                    secToken5mins = Convert.ToBase64String(ms.ToArray());
                }
            }


            return secToken5mins;
        }

        public static byte[] decryptedFile(byte[] encFile, string EncryptionKey = "")
        {
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encFile, 0, encFile.Length);
                        cs.Close();
                    }
                    return ms.ToArray();
                }
            }
            
        }


    }
}
