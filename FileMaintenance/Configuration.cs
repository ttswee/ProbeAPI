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
    public enum JobInterval
    { Day = 0, Month = 1 }

    public enum SpecialDay
    {
        NotInUse = 0,
        LastDayOfMonth = 1
    }

    public enum KeepIntervalType { Days = 0, Month = 1, Year = 2 }

    
    public  enum  JobType { Delete = 0, Move = 1, Compress = 2 }

    [XmlType("MaintenanceSchedule")]
    [Serializable]
    public class MaintSch
    {
        public string JobName { get; set; }
        public JobType JobType { get; set; }
        public string FolderName { get; set; }
        public string TargetFolderName { get; set; }
        public SpecialDay SpecialDay { get; set; } // the job only run on specific day of the month
        public bool IncludeSubFolder { get; set; }
        public bool IsJobActive { get; set; }
        public int IntervalToKeep { get; set; } //How many days/month/year to be purge/move
        public KeepIntervalType KeepIntervalsType { get; set; }
        public int SpecificDay { get; set; }

        public override bool Equals(object obj)
        {
            var NewJob = obj as MaintSch;
            if (JobType != NewJob.JobType || FolderName != NewJob.FolderName || IncludeSubFolder != NewJob.IncludeSubFolder || JobType != NewJob.JobType)
                return false;
            if (TargetFolderName != NewJob.TargetFolderName || SpecificDay != NewJob.SpecificDay || SpecificDay != NewJob.SpecificDay)
                return false;
            return true;
        }



    }

    public class MSch
    {
        [System.Xml.Serialization.XmlArray]
        public List<MaintSch> MSchedule = new List<MaintSch>();
        public string _AppPath { get; set; }
        private string  _JobConfigurateFileName = "JobsConfig.xml";
        public bool addSchedule(MaintSch newJob)
        {
            if (File.Exists(Path.Combine(_AppPath,_JobConfigurateFileName)))
            {
                MSchedule = GetAllJobs();
                var existingfolder = MSchedule.Exists(x => x.Equals(newJob));
                try
                {
                    if (existingfolder)
                    {
                        throw new Exception("Folder setting Exist");
                    }
                    MSchedule.Add(newJob);
                }
                catch
                {
                    return false;
                }

            }
            else
            {
                MSchedule.Add(newJob);
            }

            var path = Path.Combine(_AppPath,_JobConfigurateFileName);//Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml";
            System.Xml.Serialization.XmlSerializer writer =
                     new System.Xml.Serialization.XmlSerializer(MSchedule.GetType());
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, MSchedule);
            return true;
        }

        public List<MaintSch> GetAllJobs()
        {
            if (File.Exists(Path.Combine(_AppPath, _JobConfigurateFileName)))
            {
                using (Stream reader = new FileStream(Path.Combine(_AppPath, _JobConfigurateFileName), FileMode.Open))
                {
                    XmlSerializer DesSchedule = new XmlSerializer(typeof(List<MaintSch>));
                    MSchedule = (List<MaintSch>)DesSchedule.Deserialize(reader);
                }
                return MSchedule;
            }
            return new List<MaintSch>();
        }

        public bool ExecuteJobs()
        {
            List<MaintSch> _JobList = GetAllJobs();
            MaintenanceJobs _JobExecuter = new MaintenanceJobs();
            bool JobResult = false;
            foreach (MaintSch _Job in _JobList)
            {
                _JobExecuter._jobToExectute = _Job;
                JobResult = _JobExecuter.RunJob();
            }
            return false;
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
