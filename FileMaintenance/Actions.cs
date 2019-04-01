using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileMaintenance;
using System.IO;
namespace FileMaintenance
{
    public class MaintenanceJobs 
    {
        public MaintSch _jobToExectute{get;set;}

        public bool  GetJobConfig()
        {
            return false;
        }
        public bool RunJob()
        {
            if (_jobToExectute.IsJobActive != true)
                return true;

            if (_jobToExectute.JobType == JobType.Move)
            {
                return MoveJobs();
            }

            if (_jobToExectute.JobType == JobType.Delete)
            {
                return DeleteJobs();
            }
            


            return false;
        }

        public bool WriteLogs()
        {
            return true;
        }


        public bool MoveJobs()
        {
            if (_jobToExectute.TargetFolderName=="")
                throw new Exception ("Move Job has no destination folder specified");
            if (!Directory.Exists(_jobToExectute.TargetFolderName))
                throw new Exception ("Move Job targer folder does not exist!");
            if (!Directory.Exists(_jobToExectute.FolderName))
                throw new Exception ("Move Job source folder does not exist!");
            DirectoryInfo dirInfo = new DirectoryInfo(_jobToExectute.FolderName);

            List<FileInfo> _fInfo = dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).ToList();
            DateTime _ToMove = DateTime.Now;
            if (_jobToExectute.Interval==JobInterval.Day)
            {
                _ToMove=DateTime.Now.AddDays(_jobToExectute.IntervalToKeep * -1);
            }
            if (_jobToExectute.Interval==JobInterval.Month)
            {
                _ToMove=DateTime.Now.AddMonths(_jobToExectute.IntervalToKeep * -1);
            }

            List<FileInfo> _FileToMove = _fInfo.Where(Fi => Fi.CreationTime >= _ToMove).ToList();

            string _SourceFile = "";
            string _TargerFile = "";
            foreach (FileInfo _toMove in _FileToMove)
            {
                _SourceFile = _toMove.FullName;
                _TargerFile = Path.Combine(_jobToExectute.TargetFolderName,_toMove.Name + _toMove.Extension);
                Console.WriteLine("Source file = {0}, TargetFile = {1}",_SourceFile,_TargerFile);
            }

            return false;
        }

        public bool DeleteJobs()
        {
            return false;
        }

    }
}
