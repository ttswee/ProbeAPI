using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;

namespace ProbeGateway
{
    public class DriveSpaces
    {
        public string driveLetter { get; set; }
        public long freeSpace { get; set; }
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
    }

    public class ProbeSensor : ISpaceProbe, IFolderMaintenance
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
                        dSpace.Add(new DriveSpaces() { driveLetter = drive.VolumeLabel, freeSpace = drive.TotalFreeSpace });
                    }
                }
                return dSpace;
            }
            catch
            {
                throw;
            }

        }

        public List<FileInfo> GetFolderInfo()
        {
            foreach(DriveInfo dInfo in DriveInfo.GetDrives())
            {
                if (dInfo.IsReady)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo( dInfo.RootDirectory.ToString());
                    foreach (DirectoryInfo subDirInfo in dirInfo.EnumerateDirectories())
                    {
                        //subDirInfo.FullName
                    }


                }
            }
          
            
            return new List<FileInfo>();

        }
    }



}
