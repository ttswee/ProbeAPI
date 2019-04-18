using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;
using FileMaintenance;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.ServiceModel;
namespace GlobalAPI
{
    public class DriveSpaces
    {
        public string driveLetter { get; set; }
        public long freeSpace { get; set; }
        public long TotalSpace { get; set; }
    }
    [XmlType("MaintenanceSchedule")]
    [Serializable, DataContract]
    public class UserAuthenticity
    {
        public string macAddr { get; set; }
        public string UserID { get; set; }
    }

    [ServiceContract]
    public interface ISpaceProbe
    {

        [OperationContract]
        List<DriveSpaces> GetDriveInfo();

    }

    [ServiceContract]
    public interface IFolderMaintenance
    {
        [OperationContract]
        List<FileInfo> GetFolderInfo();

        [OperationContract]
        byte[] GetFile(string FileName);
    }

    [ServiceContract]
    public interface IJobMaintenance
    {
        [OperationContract]
        List<MaintSch> GetJobList();


    }

    public class PerceiverAPIs : ISpaceProbe, IFolderMaintenance, IJobMaintenance
    {
        public string clientMac { get; set; }
        public List<DriveSpaces> GetDriveInfo()
        {
            if (!validateClient())
            {
                throw new Exception ("Unauthorized client");
            }
            try
            {
                var dSpace = new List<DriveSpaces>();

                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        dSpace.Add(new DriveSpaces() { driveLetter = drive.Name, freeSpace = drive.TotalFreeSpace,TotalSpace=drive.TotalSize });
                    }
                }
                return dSpace;
            }
            catch
            {
                throw;
            }

        }

        public List<MaintSch> GetJobList()
        {
            var MJobs = new MSch();
            MJobs._AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(MJobs._AppPath);
            var _JobList = MJobs.GetAllJobs();
            Console.WriteLine(_JobList[0].JobName);
            return _JobList;
        }

        public List<FileInfo> GetFolderInfo()
        {
            foreach (DriveInfo dInfo in DriveInfo.GetDrives())
            {
                if (dInfo.IsReady)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dInfo.RootDirectory.ToString());
                    foreach (DirectoryInfo subDirInfo in dirInfo.EnumerateDirectories())
                    {
                    }
                }
            }
            return new List<FileInfo>();
        }

        public byte[] GetFile(string FileName)
        {
            byte[] FileNotExit=null;
            
            if (File.Exists(FileName))
                return File.ReadAllBytes(FileName);
            
            return FileNotExit;

        }

        private bool validateClient()
        {
            if (clientMac == "E4-A7-A0-28-53-E3")
                return true;
            return false;
        }

    }



}
