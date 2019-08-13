using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;
using acl;
using System.ServiceProcess;
using GlobalAPI;
using System.Collections.ObjectModel;

namespace PSSAKB
{
    sealed class mainViewModel //: INotifyPropertyChanged
    {


        private winProcesses _processstatus;
        public winProcesses processstatus
        {
            get { return _processstatus; }
            set
            {
                if (processstatus != value)
                {
                    _processstatus = value;
                }
            }
        }


        private winServices _servicestatus;
        public winServices servicestatus
        {
            get { return _servicestatus; }
            set
            {
                if (servicestatus != value)
                {
                    _servicestatus = value;
                }
            }
        }

        private XMLCaseNo _CaseNo;
        public XMLCaseNo CaseNo
        {
            get { return _CaseNo; }
            set
            {
                if (CaseNo != value)
                {
                    _CaseNo = value;
                }
            }
        }
        private logContent _lContent;
        public logContent lContent { 
            get {return _lContent;}
            set
            {
                if (lContent != value)
                {
                    _lContent = value;
                }
            }
        }

 

        private sqlStatmentHandler _sSqlStatement;
        public sqlStatmentHandler sqlToExecute
        {
            get { return _sSqlStatement; }
            set
            {
                if (sqlToExecute != value)
                {
                    _sSqlStatement = value;
                }
            }
        }



        private enqXML _xmlSysList;
        public enqXML xmlSysList
        {
            get { return _xmlSysList; }
            set
            {
                if (xmlSysList != value)
                {
                    _xmlSysList = value;
                }
            }
        }

        private comboItems _folderOptions;
        public comboItems folderOptions
        {
            get { return _folderOptions; }
            set
            {
                if (_folderOptions != value)
                {
                    _folderOptions = value;
                }
            }
        }


        public mainViewModel()
        {
            processstatus = new winProcesses { allProcesses = apiHandler.gChannel.GetProcesses() };
            servicestatus = new winServices { allServices = apiHandler.gChannel.GetServiceState() };
            lContent = new logContent();
            //lContent = new logContent { fList = apiHandler.gFileReader.GetFileList("c:\\ga") };
            //lContent = new logContent { fList = new List<FileInfo>() };


            folderOptions =new comboItems();
            List<GlobalAPI.ReadDirection> rdire = Enum.GetValues(typeof(ReadDirection)).Cast<ReadDirection>().ToList();
            folderOptions.rdirect = rdire;
            List<logFolderList> defCombo = new List<logFolderList>();
            defCombo.Add(new logFolderList{folderName="e:\\ebbs\\log"});
            defCombo.Add(new logFolderList{folderName="e:\\rls\\log"});
            defCombo.Add(new logFolderList { folderName = "e:\\rls_icdd\\log" });
            folderOptions.folderList = defCombo;
            CaseNo = new XMLCaseNo { CaseNo = "1234", DestFolder = "c:\\xml" , TotalFiles="Waiting"};
            
            sqlToExecute = new sqlStatmentHandler();
            
            xmlSysList = new enqXML { sysList = new List<string>() { "EBBS", "ICDD", "ICM" } };

        }


       

        private ICommand fileGet;
        private ICommand executeSQL;
        private ICommand GetFileList;
        private ICommand changeServiceState;
        public ICommand changeServState
    {
        get
        {
            if (this.changeServiceState == null)
            {
                this.changeServiceState = new RelayCommand(startstopService, CanTrigger);
            }
            return changeServiceState;
        }
    }


        private ICommand startStopService;
        public ICommand restartServ
        {
            get
            {
                if (this.startStopService == null)
                {
                    this.startStopService = new RelayCommand(RestartService, CanTrigger);
                }
                return startStopService;
            }
            
        }


        private ICommand GetFileContents;
        public ICommand LogLines
        {
            get
            {
                if (this.GetFileContents == null)
                {
                    this.GetFileContents = new RelayCommand(getLogs, CanTrigger);
                }
                return GetFileContents;
            }

        }

        public bool CanTrigger(object obj)
        {
            return true;
        }
        public ICommand getFilePressed
        {
            get
            {
                if (this.fileGet == null)
                {
                    this.fileGet = new RelayCommand(fileGetExecute,CanTrigger);
                }
                return this.fileGet;
            }
        }

        public ICommand getFilesList
        {
            get
            {
                if (this.GetFileList == null)
                {
                    this.GetFileList = new RelayCommand(FileListGetExec, CanTrigger);
                }
                return this.GetFileList;
            }
        }

        public ICommand ExecuteSQLPressed
        {
            get
            {
                if (this.executeSQL == null)
                {
                    this.executeSQL = new RelayCommand(actExecuteSQL, CanTrigger);
                }
                return this.executeSQL;
            }
        }


        private void actExecuteSQL(object obj)
        {
            int totalrecord = 0;
            string secToken = acl.ACLs.genSecToken();
            totalrecord = apiHandler.gCRESChannel.UpdateQueues(sqlToExecute.sSqlStatement , secToken);
            sqlToExecute.totalRecords = string.Format("Total updated records : {0}", totalrecord);
        }

        private void startstopService(object sender)
        {
            var process = (GlobalAPI.WindowsServices) sender;
                bool restarted = apiHandler.gChannel.PostRestartService(process.serviceName,serviceAction.ChangeState);
                processstatus = new winProcesses { allProcesses = apiHandler.gChannel.GetProcesses() };
        }

        private void RestartService(object sender)
        {
            var process = (GlobalAPI.WindowsServices)sender;
                bool restarted = apiHandler.gChannel.PostRestartService(process.serviceName,serviceAction.Restart);
            
        }

        private void FileListGetExec(object obj)
        {
            lContent.fList = apiHandler.gFileReader.getFileList(lContent.selectedFolder.folderName);
        }

        private void getLogs(object sender)
        {
            var logc = (FileInfo)sender;
            var contents = apiHandler.gFileReader.getFileContent(logc.FullName, lContent.linesToRead , ReadDirection.Backward);
            lContent.LogFileLines = "";
            foreach (string cnt in contents)
            {
                lContent.LogFileLines += cnt + Environment.NewLine;
            }

        }

        private void fileGetExecute(object obj)
        {
            try
            {

                string caseno = CaseNo.CaseNo;// parameter.ToString();
                List<CRESapi.interfaceFiles> Results = new List<CRESapi.interfaceFiles>();
                string secToken = acl.ACLs.genSecToken();
               
                Results = apiHandler.gCRESChannel.GetInterFaceFile(caseno, "91,92,93,94,95,96,97", "e:\\ebbs\\done", secToken);
                if (Results.Count > 0)
                {
                    string destfolder = CaseNo.DestFolder;//"c:\\xml\\";
                    if (!Directory.Exists(destfolder))
                    {
                        Directory.CreateDirectory(destfolder);
                    }
                    if (!Directory.Exists(System.IO.Path.Combine(destfolder, caseno)))
                    {
                        Directory.CreateDirectory(Path.Combine(destfolder, caseno));
                    }
                    foreach (CRESapi.interfaceFiles ff in Results)
                    {

                        File.WriteAllBytes(Path.Combine(Path.Combine(destfolder, caseno), ff.fileName), ACLs.decryptedFile(ff.encFile, secToken));
                    }
                    CaseNo.TotalFiles = string.Format("Total files Retrieved : {0}",Results.Count.ToString());
                }
                else
                {
                    CaseNo.TotalFiles = "There is no files meet the criteria";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

     public class RelayCommand : ICommand
     {
         #region Miembros

         readonly Action<object> _execute;
         readonly Predicate<object> _canExecute;

         #endregion

         #region Constructor

         public RelayCommand(Action<object> execute)
             : this(execute, null)
         {
         }

         public RelayCommand(Action<object> execute, Predicate<object> canExecute)
         {
             if (execute == null)
             {
                 throw new ArgumentNullException("execute");
             }

             _execute = execute;
             _canExecute = canExecute;
         }

         #endregion

         #region Miembros de ICommand

         public bool CanExecute(object parameter)
         {
             return _canExecute == null || _canExecute(parameter);
         }

         public event EventHandler CanExecuteChanged
         {
             add { CommandManager.RequerySuggested += value; }
             remove { CommandManager.RequerySuggested -= value; }
         }

         public void Execute(object parameter)
         {
             _execute(parameter);
         }

         #endregion
     }


}
