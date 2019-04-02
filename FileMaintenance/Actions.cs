using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileMaintenance;
using System.IO.Compression;
using System.IO;

namespace FileMaintenance
{
    public class MaintenanceJobs 
    {
        public MaintSch _jobToExectute{get;set;}
        public string _LogPath { get; set; }
        public bool  GetJobConfig()
        {
            return false;
        }
        public bool RunJob()
        {
            if (_jobToExectute.IsJobActive != true)
                return true;

            if (_jobToExectute.SpecialDay==SpecialDay.LastDayOfMonth)
                if (DateTime.Now.Day != DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month))
                    return true;
            


            if (_jobToExectute.JobType == JobType.Move)
            {
                return MoveJobs();
            }

            if (_jobToExectute.JobType == JobType.Delete)
            {
                return DeleteJobs();
            }

            if (_jobToExectute.JobType == JobType.Compress)
            {
                return CompressJob();
            }

            return false;
        }

        public bool WriteLogs(string[] sLogText)
        {
            string LogFileName=Path.Combine( _LogPath , _jobToExectute.JobName + ".txt");
            FileStream file;
            if (!File.Exists(LogFileName))
            {
                file = File.Create(LogFileName);
                file.Close();
            }
            File.WriteAllLines(LogFileName,sLogText);
            return true;
        }


        public bool MoveJobs()
        {
            if (_jobToExectute.TargetFolderName=="")
                throw new Exception ("Move Job has no destination folder specified");
            //if (!Directory.Exists(_jobToExectute.TargetFolderName))
            //    throw new Exception ("Move Job targer folder does not exist!");
            if (!Directory.Exists(_jobToExectute.FolderName))
                throw new Exception ("Move Job source folder does not exist!");
            DirectoryInfo dirInfo = new DirectoryInfo(_jobToExectute.FolderName);

            List<FileInfo> _fInfo = dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).ToList();
            DateTime _ToMove = DateTime.Now;
            if (_jobToExectute.KeepIntervalsType==KeepIntervalType.Days)
            {
                _ToMove=DateTime.Now.AddDays(_jobToExectute.IntervalToKeep * -1);
            }
            if (_jobToExectute.KeepIntervalsType == KeepIntervalType.Month)
            {
                _ToMove=DateTime.Now.AddMonths(_jobToExectute.IntervalToKeep * -1);
            }

            List<FileInfo> _FileToMove = _fInfo.Where(Fi => Fi.CreationTime <= _ToMove).ToList();
            List<string> LogDetails = new List<string>();

            string _SourceFile = "";
            string _TargerFile = "";
            foreach (FileInfo _toMove in _FileToMove)
            {
                _SourceFile = _toMove.FullName;
                //_TargerFile = Path.Combine(_jobToExectute.TargetFolderName,_toMove.Name);
                _TargerFile = _toMove.FullName.Replace(_jobToExectute.FolderName, _jobToExectute.TargetFolderName);
                LogDetails.Add(string.Format("Source file = {0}, TargetFile = {1}", _SourceFile, _TargerFile));
                if (!Directory.Exists(Path.GetDirectoryName(_TargerFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(_TargerFile));
                File.Move(_SourceFile, _TargerFile);
            }
            WriteLogs(LogDetails.ToArray());
            return false;
        }

        public bool DeleteJobs()
        {
            if (!Directory.Exists(_jobToExectute.FolderName))
            {
                throw new Exception("Folder does not exist!");
            }
            DirectoryInfo dirInfo = new DirectoryInfo(_jobToExectute.FolderName);

            List<FileInfo> _fInfo = dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).ToList();
            DateTime _toDelete = DateTime.Now;
            if (_jobToExectute.KeepIntervalsType == KeepIntervalType.Days)
            {
                _toDelete = DateTime.Now.AddDays(_jobToExectute.IntervalToKeep * -1);
            }
            if (_jobToExectute.KeepIntervalsType == KeepIntervalType.Month)
            {
                _toDelete = DateTime.Now.AddMonths(_jobToExectute.IntervalToKeep * -1);
            }

            List<FileInfo> _DeleteList = _fInfo.Where(Fi => Fi.CreationTime <= _toDelete).ToList();
            List<string> LogDetails = new List<string>();
            string _SourceFile = "";
            foreach (FileInfo _filemarkfordelete in _DeleteList)
            {
                _SourceFile = _filemarkfordelete.FullName;
                LogDetails.Add(string.Format("File to delete : {0}, Last Access Date {1}", _SourceFile, _filemarkfordelete.LastAccessTime));
            }
            WriteLogs(LogDetails.ToArray());
            return false;
        }


        public bool CompressJob()
        {
            if (!Directory.Exists(_jobToExectute.FolderName))
            {
                throw new Exception("Folder does not exist!");
            }
            DirectoryInfo dirInfo = new DirectoryInfo(_jobToExectute.FolderName);

            List<FileInfo> _fInfo = dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).ToList();
            DateTime _toDelete = DateTime.Now;
            if (_jobToExectute.KeepIntervalsType == KeepIntervalType.Days)
            {
                _toDelete = DateTime.Now.AddDays(_jobToExectute.IntervalToKeep * -1);
            }
            if (_jobToExectute.KeepIntervalsType == KeepIntervalType.Month)
            {
                _toDelete = DateTime.Now.AddMonths(_jobToExectute.IntervalToKeep * -1);
            }

            List<FileInfo> _DeleteList = _fInfo.Where(Fi => Fi.CreationTime <= _toDelete).ToList();

            string _SourceFile = "";
            foreach (FileInfo fileToCompress in _DeleteList)
            {
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & 
                   FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, 
                           CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);

                        }
                    }
                    FileInfo info = new FileInfo(_LogPath + Path.DirectorySeparatorChar + fileToCompress.Name + ".gz");
                    //Console.WriteLine($"Compressed {fileToCompress.Name} from {fileToCompress.Length.ToString()} to {info.Length.ToString()} bytes.");
                }

            }
                //_SourceFile = _filemarkfordelete.FullName;
                //WriteLogs(string.Format("File to delete : {0}, Last Access Date {1}", _SourceFile, _filemarkfordelete.LastAccessTime));
            }

            return false;
        }
    }
}
