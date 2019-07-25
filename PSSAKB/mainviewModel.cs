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
namespace PSSAKB
{
    sealed class mainViewModel : INotifyPropertyChanged
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

        private CaseNo _nCaseNo;
        public CaseNo nCaseNo
        {
            get { return _nCaseNo; }
            set
            {
                if (nCaseNo != value)
                {
                    _nCaseNo = value;
                }
            }
        }

        public string sXMLCase{
            get { return nCaseNo.nCaseNo; }
            set{
                nCaseNo.nCaseNo = value;
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


        public mainViewModel()
        {
            processstatus = new winProcesses { allProcesses = apiHandler.gChannel.GetProcesses() };
            servicestatus = new winServices { allServices = apiHandler.gChannel.GetServiceState() };
            nCaseNo = new CaseNo{nCaseNo = "333",sDestFolder="c:\\xml"};
            sqlToExecute = new sqlStatmentHandler();
            xmlSysList = new enqXML { sysList = new List<string>() { "EBBS", "ICDD", "ICM" } };

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ICommand fileGet;
        private ICommand executeSQL;
        private ICommand startStopProcess;
        private ICommand startStopService;
        public ICommand restartServ
        {
            get
            {
                if (this.startStopProcess == null)
                {
                    this.startStopProcess = new RelayCommand(RestartService, CanTrigger);
                }
                return startStopProcess;
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
            //MessageBox.Show(string.Format("Total records affected : {0}",totalrecord));
        }

        private void startstopService(object sender)
        {
            //todo : call the api to restart the service

            var process = (GlobalAPI.WindowsServices) sender;
//            if (process.serviceStatus == ServiceControllerStatus.Running)
//           {
                bool restarted = apiHandler.gChannel.PostRestartService(process.serviceName,serviceAction.Restart);
//            }
            //else
            //{

            //}
        }

        private void RestartService(object sender)
        {
            //todo : call the api to restart the service

            var process = (GlobalAPI.WindowsServices)sender;
            //if (process.serviceStatus == ServiceControllerStatus.Running)
            //{
                bool restarted = apiHandler.gChannel.PostRestartService(process.serviceName,serviceAction.ChangeState);
            //}
            
        }


        private void fileGetExecute(object obj)
        {
            try
            {

                string caseno = nCaseNo.nCaseNo;// parameter.ToString();
                List<CRESapi.interfaceFiles> Results = new List<CRESapi.interfaceFiles>();
                string secToken = acl.ACLs.genSecToken();
                Results = apiHandler.gCRESChannel.GetInterFaceFile(caseno, "91,92,93,94,95,96,97", "e:\\ebbs\\done", secToken);//CRESServiceContract.GetInterFaceFile(caseno, "91,92,93,94,95,96,97", "e:\\ebbs\\done", secToken);
                if (Results.Count > 0)
                {
                    string destfolder = nCaseNo.sDestFolder;//"c:\\xml\\";
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
                    
                }
                else
                {

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
