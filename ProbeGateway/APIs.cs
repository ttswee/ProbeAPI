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

            [OperationContract]
            string GetFolderInfo();
        }

    
    public  class ProbeSensor : ISpaceProbe
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

            public string GetFolderInfo()
            {
                return "here!!!";
            }
        }



}
