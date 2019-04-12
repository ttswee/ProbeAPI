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
        public MaintSch _jobToExecute{get;set;}
        public string _LogPath { get; set; }
        private List<FileInfo> _allFileList { get; set; }
        private DateTime? _dateToCheck { get; set; }
        public string DetailLogFileName { get; set; }
        public string HeaderLogFileName { get; set; }
        private List<string> LogDetails = new List<string>();
        private List<string> LogHeader = new List<string>();
        public bool  GetJobConfig()
        {
            return false;
        }


        public  MaintenanceJobs()
        {

        }
        

        public bool RunJob()
        {
            _allFileList = new List<FileInfo>();

            DetailLogFileName = Path.Combine(_LogPath, _jobToExecute.JobName + "_d.txt");
            HeaderLogFileName = Path.Combine(_LogPath, _jobToExecute.JobName + "_h.txt");       

            if (_jobToExecute.IsJobActive != true)
                return true;

            if (_jobToExecute.SpecialDay==SpecialDay.LastDayOfMonth)
                if (DateTime.Now.Day != DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month))
                    return true;

            _dateToCheck = CheckRange();

            if (_jobToExecute.JobType == JobType.Move)
            {
                return MoveJobs();
            }

            if (_jobToExecute.JobType == JobType.Delete)
            {
                return DeleteJobs();
            }

            if (_jobToExecute.JobType == JobType.Compress)
            {
                return CompressJob();
            }

            return false;
        }

        private bool WriteLogs(string LogFileName, string[] sLogText)
        {
            
            FileStream file;
            if (!File.Exists(LogFileName))
            {
                file = File.Create(LogFileName);
                file.Close();
            }
            File.AppendAllLines(LogFileName,sLogText);
            return true;
        }




        private bool MoveJobs()
        {
            LogHeader.Clear();
            LogDetails.Clear();
            try
            {
                if (_jobToExecute.TargetFolderName == "")
                    throw new Exception("No destination");
                if (!Directory.Exists(_jobToExecute.FolderName))
                    throw new Exception("Source Not Exist");
            }
            catch (Exception ex)
            {
                if (ex.Message == "No destination")
                {
                    WriteLogs(HeaderLogFileName,  new string[] {"Destination Folder no specified"});
                }
                if (ex.Message == "Source Not Exist")
                {
                    WriteLogs(HeaderLogFileName, new string[] { "Souce Folder does not exist" });  
                }
                return true;
            }

            
            GetAllFilesList();
            List<FileInfo> _filesToMove = _allFileList.Where(Fi => Fi.LastWriteTime <= _dateToCheck).ToList();
            string _SourceFile = "";
            string _TargerFile = "";

            LogHeader.Add(string.Format("Job Start Time : {0} " ,DateTime.Now.ToString()));
            foreach (FileInfo _fileToMove in _filesToMove)
            {
                _SourceFile = _fileToMove.FullName;
                _TargerFile = _fileToMove.FullName.Replace(_jobToExecute.FolderName, _jobToExecute.TargetFolderName);
                LogDetails.Add(string.Format("Source file = {0}, TargetFile = {1}, File Last Write Date : {2}", _SourceFile, _TargerFile,_fileToMove.LastWriteTime));
                if (!_jobToExecute.DebugMode)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(_TargerFile)))
                        Directory.CreateDirectory(Path.GetDirectoryName(_TargerFile));
                    try
                    {
                        File.Move(_SourceFile, _TargerFile);
                    }
                    catch (Exception ex)
                    {
                        LogDetails.Add(string.Format("File {0} cannot be moved, Error :  {1}", _fileToMove.FullName, ex.Message));
                    }
                }
            }
            LogHeader.Add(string.Format("Job End Time : {0} ", DateTime.Now.ToString()));
            LogHeader.Add(string.Format("Total Files moved : {0} ",_filesToMove.Count().ToString()));
            
            WriteLogs(DetailLogFileName,LogDetails.ToArray());
            WriteLogs(HeaderLogFileName, LogHeader.ToArray());
            return false;
        }

        private bool DeleteJobs()
        {
            LogHeader.Clear();
            LogDetails.Clear();
            if (!Directory.Exists(_jobToExecute.FolderName))
            {
                throw new Exception("Folder does not exist!");
            }


            GetAllFilesList();
            List<FileInfo> _filesToDelete = _allFileList.Where(Fi => Fi.LastWriteTime <= _dateToCheck).ToList();
 
            string _targetFile = "";
            foreach (FileInfo _fileToDelete in _filesToDelete)
            {
                _targetFile = _fileToDelete.FullName;
                LogDetails.Add(string.Format("Target file = {0}", _targetFile));
                if (!_jobToExecute.DebugMode)
                {
                    try
                    {
                        File.Delete(_targetFile);
                    }
                    catch (Exception ex)
                    {
                        LogDetails.Add(string.Format("File {0} cannot be deleted, Error :  {1}", _fileToDelete.FullName, ex.Message));
                    }
                }
            }
            LogHeader.Add(string.Format("Job End Time : {0} ", DateTime.Now.ToString()));
            LogHeader.Add(string.Format("Total Files moved : {0} ", _filesToDelete.Count().ToString()));
            WriteLogs(DetailLogFileName, LogDetails.ToArray());
            WriteLogs(HeaderLogFileName, LogHeader.ToArray());
            return false;
        }


        private bool CompressJob()
        {
            if (!Directory.Exists(_jobToExecute.FolderName))
            {
                throw new Exception("Folder does not exist!");
            }


            List<FileInfo> _DeleteList = _allFileList.Where(Fi => Fi.LastWriteTime <= _dateToCheck).ToList();

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
                    }

                }
            }

            return false;
        }

        private DateTime? CheckRange()
        {
            DateTime? dateOfInterest = null;
            if (_jobToExecute.KeepIntervalsType == KeepIntervalType.Days)
            {
                dateOfInterest = DateTime.Now.AddDays(_jobToExecute.IntervalToKeep * -1);
            }
            if (_jobToExecute.KeepIntervalsType == KeepIntervalType.Month)
            {
                dateOfInterest= DateTime.Now.AddMonths(_jobToExecute.IntervalToKeep * -1);
            }
            return dateOfInterest; 
        }

        private void GetAllFilesList()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_jobToExecute.FolderName);
            List<FileInfo> _byExt = new List<FileInfo>();
            if (_jobToExecute.IncludeSubFolder)
                foreach( var ext in _jobToExecute.FileExt.Split(';'))
                {
                    _byExt = dirInfo.EnumerateFiles(ext, SearchOption.AllDirectories).ToList();
                    _allFileList = _allFileList.Concat(_byExt).ToList();
                }
            else
                foreach (var ext in _jobToExecute.FileExt.Split(';'))
                {
                    _allFileList = _allFileList.Concat(dirInfo.EnumerateFiles(ext, SearchOption.TopDirectoryOnly)).ToList();
                }
        }

        

    }
}
