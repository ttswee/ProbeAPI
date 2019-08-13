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
//using PerceiverDAL;
using System.Data;
using System.Configuration;
using PerceiverAPI;
using System.ServiceProcess;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
namespace GlobalAPI
{
    public enum serviceAction { ChangeState = 1, Restart = 2 }
    public enum ReadDirection { Forward=1,Backward = 2}

 

    public class DriveSpaces
    {
        public string driveLetter { get; set; }
        public long freeSpace { get; set; }
        public long TotalSpace { get; set; }
    }

    public class ProcessAudit 
    {
        public string process_stage {get;set;}
        public DateTime recordingdate {get;set;}

    }
    
    public class WindowsServices
    {
        [Description("Service Name")]
        public string serviceName {get;set;}
        [Description("Service Display Name")]
        public string serviceDisplayName {get;set;}
        [Description("Service Status")]
        public ServiceControllerStatus serviceStatus {get;set;}
    }

    public class WindowsProcesses
    {
        public string processName {get;set;}
        public bool processResponding { get; set; }
        public Int16 processThread { get; set; }
        public string processCommandLine { get; set; }
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
        [OperationContract(IsOneWay = false)]
        [WebGet]
        List<DriveSpaces> GetDriveInfo();
    }

    [ServiceContract]
    public interface IFolderMaintenance
    {
        [OperationContract(IsOneWay = false)]
        [WebGet]
        List<FileInfo> GetFolderInfo();
    }

    [ServiceContract]
    public interface IJobMaintenance
    {
        [OperationContract(IsOneWay = false)]
        [WebGet]
        List<MaintSch> GetJobList();
    }

    [ServiceContract]
    public interface IServerAdministration
    {
        [OperationContract(IsOneWay = false)]
        List<WindowsServices> GetServiceState();

        [OperationContract(IsOneWay = false)]
        List<WindowsProcesses> GetProcesses();

        [OperationContract(IsOneWay = false)]
        List<Process> GetProcessesComplete();

        [OperationContract(IsOneWay = false)]
        bool PostRestartService(string ServiceName,serviceAction act);
    }

    [ServiceContract]
    public interface IFileReader
    {
        [OperationContract]
        List<FileInfo> getFileList(string folderName);
        [OperationContract]
        List<string> getFileContent(string fileName,int nLines, ReadDirection rDirection);
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public  class PerceiverAPIs : ISpaceProbe, IFolderMaintenance, IJobMaintenance,IServerAdministration,IFileReader
    {
       

        public List<DriveSpaces> GetDriveInfo()
        {
            try
            {

                var dSpace = new List<DriveSpaces>();
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        dSpace.Add(new DriveSpaces() { driveLetter = drive.Name, freeSpace = drive.TotalFreeSpace, TotalSpace = drive.TotalSize });
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
            var _JobList = MJobs.GetAllJobs();
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


        public List<WindowsServices> GetServiceState()
        {

            List<WindowsServices> sList = new List<WindowsServices>();
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController serv in services)
            {
                sList.Add(new WindowsServices { serviceName = serv.ServiceName, serviceDisplayName = serv.DisplayName, serviceStatus = serv.Status });
            }
            return sList;

        }




        public List<WindowsProcesses> GetProcesses()
        {
            List<WindowsProcesses> pList = new List<WindowsProcesses>();
            Process[] CurrentProcesses = Process.GetProcesses();
            foreach (Process pro in CurrentProcesses)
            {
                try
                {
                    pList.Add(new WindowsProcesses { processName = pro.ProcessName, processResponding = pro.Responding, processThread = (Int16)pro.Threads.Count, processCommandLine = pro.MainModule.FileName });
                }
                catch { }
            }
            return pList;
        }

        public List<Process> GetProcessesComplete()
        {
            return Process.GetProcesses().ToList();
        }



        public bool PostRestartService(string ServiceName, serviceAction act)
        {

            ServiceController sc = new ServiceController(ServiceName);
            if (act == serviceAction.Restart)
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    sc.Start();
                    return true;
                }
                else
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                        return true;
                    }
                return false;
            }
            if (act == serviceAction.ChangeState)
            {
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                    return true;
                }
                else
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        sc.Stop();
                        return true;
                    }
            }
            return true;
        }

        public List<FileInfo> getFileList(string folderName)
        {
            DirectoryInfo dinfo = new DirectoryInfo(folderName);
            return dinfo.EnumerateFiles().ToList();
        }

        public List<string> getFileContent(string fileName, int nLines, ReadDirection rDirection)
        {
            string[] fileContent;
            fileContent = File.ReadAllLines(fileName);
            if (rDirection == ReadDirection.Backward)
            {
                if (fileContent.Count() < nLines)
                {
                    return fileContent.ToList<string>();
                }
                    return new List<string>(fileContent).GetRange(fileContent.Count() - nLines, nLines);
            }
            else
            {
                if (fileContent.Count() < nLines)
                {
                    return fileContent.ToList<string>();
                }
                return fileContent.Skip(0).Take(nLines).ToList<string>();
            }
//

        }
    }



}
