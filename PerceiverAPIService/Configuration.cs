using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Diagnostics;
namespace PerceiverAPI
{
    class Configuration
    {
        [XmlType("LogFilesFolder")]
        [Serializable, DataContract]
        public class LogFileFolder
        {
            [DataMember]
            public string systemName { get; set; }
            [DataMember]
            public string folderName { get; set; }
        }

        public class LogFolder
        {
            public string _AppPath { get; set; }
            public List<LogFileFolder> FolderList { get; set; }
            public LogFolder()
            {
                string _LogFileCongig = "LogFolders.xml";
                if (File.Exists(Path.Combine(_AppPath, _LogFileCongig)))
                {
                    using (Stream reader = new FileStream(Path.Combine(_AppPath, _LogFileCongig), FileMode.Open))
                    {
                        XmlSerializer DesSchedule = new XmlSerializer(typeof(List<LogFolder>));
                        FolderList = (List<LogFileFolder>)DesSchedule.Deserialize(reader);
                    }
                }
            }

            public bool saveConfig()
            {
                FolderList.Add(new LogFileFolder{systemName="EBBS",folderName="e:\\ebbs\\loggs"});
                FolderList.Add(new LogFileFolder { systemName = "ICDD", folderName = "e:\\ICDD\\loggs" });
                System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(FolderList.GetType());
                System.IO.FileStream file = System.IO.File.Create("c:\\xml\\LogFolders.xml");
                writer.Serialize(file, FolderList);
                file.Close();
                return true;
            }

        }
    }
}
