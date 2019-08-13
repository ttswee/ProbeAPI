using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
namespace PSSAKB
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class winProcesses
    {
        public List<GlobalAPI.WindowsProcesses> allProcesses { get; set; }
    }

    public class winServices
    {
        public List<GlobalAPI.WindowsServices> allServices { get; set; }
    }

    public class XMLCaseNo : NotifyPropertyChanged
    {
        public string CaseNo { get; set; }
        public string DestFolder { get; set; }
        private string _TotalFiles;
        public string TotalFiles
        {
            get {return _TotalFiles;}

            set
            {
                _TotalFiles = value;
                OnPropertyChange("TotalFiles");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class sqlStatmentHandler:NotifyPropertyChanged
    {
        public string sSqlStatement{get;set;}
        private string _totalRecords;
        public string totalRecords
        {
            get { return _totalRecords; }
            set
            {
                _totalRecords = value;
                OnPropertyChange("totalRecords");
            }
        }
                
    }

    public class enqXML
    {
        public List<string> sysList { get; 
            set; }
    }

    public class logContent:NotifyPropertyChanged
    {
        private List<FileInfo> _fList;
        public List<FileInfo> fList
        {
            get { return _fList; }
            set
            {
                _fList = value;
                OnPropertyChange("fList");
            }
        }

        private string _LogFileLines;
        public string LogFileLines
        {
            get { return _LogFileLines; }
            set
            {
                _LogFileLines = value;
                OnPropertyChange("LogFileLines");
            }
        }
        public int linesToRead { get; set; }
        public GlobalAPI.ReadDirection readDirection { get; set; }
        
    }


}
