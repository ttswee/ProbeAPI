using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace FileMaintenance
{
    enum Interval
    { Day = 0, Month = 1, Year = 2 }

    enum SpecificDay
    {
        FirstDayOfMonth = 0,
        LastDayOfMonth = 1
    }

    [XmlType("MaintenanceSchedule")]
    [Serializable]
    public class MaintSch
    {
        //[System.Xml.Serialization.XmlAnyElement("FolderName")]
        public string FolderName { get; set; }
        // [System.Xml.Serialization.XmlAnyElement("IntervalType")]
        public int Interval { get; set; }
        // [System.Xml.Serialization.XmlAnyElement("IntervalNum")]
        public int IntervalNum { get; set; }
        //  [System.Xml.Serialization.XmlAnyElement("SpecificDay")]
        public int SpecificDay { get; set; }
        //  [System.Xml.Serialization.XmlAnyElement("TopFolderOnly")]
        public bool IncludeSubFolder { get; set; }


        public MaintSch()
        {
        }
    }

    public class MSch
    {
        [System.Xml.Serialization.XmlArray]
        public List<MaintSch> MSchedule = new List<MaintSch>();

        public bool addSchedule(string folderName, int IntervalType, int IntervalNum, int SpecificDay, bool TopOnly)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml"))
            {
                using (Stream reader = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml", FileMode.Open))
                {
                    XmlSerializer DesSchedule = new XmlSerializer(typeof(List<MaintSch>));
                    MSchedule = (List<MaintSch>)DesSchedule.Deserialize(reader);
                }

                var existingfolder = MSchedule.Any(x => x.FolderName == folderName);
                try
                {
                    if (existingfolder)
                    {
                        throw new Exception("Folder setting Exist");
                    }
                    MSchedule.Add(new MaintSch { FolderName = folderName, Interval = IntervalType, IntervalNum = IntervalNum, SpecificDay = SpecificDay, IncludeSubFolder = TopOnly });
                }
                catch
                {
                    return false;
                }

            }
            else
            {
                //using (Stream reader = new FileStream("MaintenanceSchedule.xml", FileMode.OpenOrCreate))
                //{
                MSchedule.Add(new MaintSch { FolderName = folderName, Interval = IntervalType, IntervalNum = IntervalNum, SpecificDay = SpecificDay, IncludeSubFolder = TopOnly });
                //}
            }

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml";
            System.Xml.Serialization.XmlSerializer writer =
                     new System.Xml.Serialization.XmlSerializer(MSchedule.GetType());
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, MSchedule);
            return true;
        }

    }

    public class JobHistory
    {
        public DateTime runDate { get; set; }
        public string jobName { get; set; }
        public bool status { get; set; }
        public int fileDeleted { get; set; }
    }

}
