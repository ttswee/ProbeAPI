using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;
using acl;
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

        private ICommand mAction;
        public ICommand fileGetCommand
        {
            get
            {
                if (mAction == null)
                    mAction = new fileGet();
                return mAction;
            }
            set
            {
                mAction = value;
            }
            
        }


        public mainViewModel()
        {
            //List<GlobalAPI.WindowsProcesses> serverService = new List<GlobalAPI.WindowsProcesses>();
            processstatus = new winProcesses { allProcesses = apiHandler.gChannel.GetProcesses() };
            servicestatus = new winServices { allServices = apiHandler.gChannel.GetServiceState() };
            //var r  = apiHandler.gChannel.GetProcesses();
            //processstatus.allProcesses = r;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }

     class fileGet : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            try
            {
                
                string caseno = "123";//this.txtCaseNo.Text;
                List<CRESapi.interfaceFiles> Results = new List<CRESapi.interfaceFiles>();
                string secToken = acl.ACLs.genSecToken();
                Results = apiHandler.gCRESChannel.GetInterFaceFile(caseno, "91,92,93,94,95,96,97", "e:\\ebbs\\done", secToken);//CRESServiceContract.GetInterFaceFile(caseno, "91,92,93,94,95,96,97", "e:\\ebbs\\done", secToken);
                if (Results.Count > 0)
                {
                    string destfolder = "c:\\xml\\";
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
                    // MessageBox.Show(string.Format("Total files copied : {0}", Results.Count));
                }
                else
                {
                    //MessageBox.Show("No files found");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
