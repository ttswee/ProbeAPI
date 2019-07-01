using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using FileMaintenance;
using System.Threading;
using System.IO;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using CRESapi;
namespace PerceiverAPI
{
    public partial class PerceiverAPIService : ServiceBase
    {
        public PerceiverAPIService()
        {
            InitializeComponent();
        }
        private static bool bRun = true;
        private static string _AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static string _LogPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static int EnableRestful = Convert.ToInt32(ConfigurationManager.AppSettings["EnableRestful"]);
        private static int RunInterval = Convert.ToInt32(ConfigurationManager.AppSettings["SleepInterval"]);

        private static string hostURI = ConfigurationManager.AppSettings["APIUri"];
        private static string CRESapiURI = ConfigurationManager.AppSettings["CRESAPIUri"];
        private static string bindingAdd = ConfigurationManager.AppSettings["BindingIP"];

        private ServiceHost host = null;
        private ServiceHost _CREShost = null;
        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }


        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                PerceiverAPIService svc = new PerceiverAPIService();
                svc.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
            { 
                new PerceiverAPIService() 
            };
                ServiceBase.Run(ServicesToRun);
            }
        }

        protected override void OnStart(string[] args)
        {
            using (EventLog eLog = new EventLog("Application"))
            {
                EventLog.Source = "PerceiverService";
                EventLog.WriteEntry(string.Format("AppPath : {0}", _AppPath), EventLogEntryType.Information);
                EventLog.WriteEntry(string.Format("LogPath : {0}", _LogPath), EventLogEntryType.Information);
            }
            bRun = true;
            List<string> logMsg = new List<string>();

            Thread fileMainJob = new Thread(new ThreadStart(FMThread));
            fileMainJob.Start();

            //Thread Globalapi = new Thread(new ThreadStart(() => InstanciateAPI(host, hostURI.Replace("{0}", bindingAdd), typeof(GlobalAPI.PerceiverAPIs))));
            //Globalapi.Start();

            Thread CREShost = new Thread(new ThreadStart(() => InstanciateAPI(_CREShost, CRESapiURI.Replace("{0}", bindingAdd), typeof(CRESapi.CRESapi))));
            CREShost.Start();

            ProcessCode.setProcessCodes();
        }

        protected override void OnStop()
        {
            bRun = false;
        }

        private void FMThread()
        {
            using (EventLog eLog = new EventLog("Application"))
            {
                EventLog.Source = "PerceiverService";
                EventLog.WriteEntry("Housekeeping thread started", EventLogEntryType.Information);
            }
            while (bRun)
            {
                MSch _MaintenanceJobs = new MSch() { _AppPath = _AppPath };
                _MaintenanceJobs._AppPath = _AppPath;
                List<MaintSch> _listOfJobs = _MaintenanceJobs.GetAllJobs();
                using (EventLog eLog = new EventLog("Application"))
                {
                    EventLog.Source = "PerceiverService";
                    EventLog.WriteEntry(string.Format("Total Jobs : {0}", _listOfJobs.Count()), EventLogEntryType.Information);
                }

                MaintenanceJobs _JobExcuter = new MaintenanceJobs() { _LogPath = _LogPath };
                _JobExcuter._LogPath = _LogPath; ;
                bool jStatus = false;
                foreach (MaintSch _Job in _listOfJobs)
                {
                    jStatus = false;
                    _JobExcuter._jobToExecute = _Job;
                    jStatus = _JobExcuter.RunJob();
                    if (jStatus == true)
                        continue;
                }
                _MaintenanceJobs = null;
                Thread.Sleep(RunInterval * 60000);
            }
        }

        private void InstanciateAPI(ServiceHost _host, string baseaddr, Type ServiceType)
        {

            if (EnableRestful == 1)
            {
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.None;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                binding.MaxBufferPoolSize = 200000;
                binding.MaxBufferSize = 200000;
                binding.MaxReceivedMessageSize = 200000;
                EndpointAddress address = new EndpointAddress(baseaddr);

                Type[] allIface = ServiceType.GetInterfaces();
                Uri baseAddress = new Uri(baseaddr);
                _host = new ServiceHost(ServiceType);
                foreach (Type tp in allIface)
                {
                    _host.AddServiceEndpoint(tp, binding, baseaddr);
                }
                
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = new Uri(baseaddr);
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
               
                _host.Description.Behaviors.Add(smb);
                
                ServiceDebugBehavior debug = _host.Description.Behaviors.Find<ServiceDebugBehavior>();

                if (debug == null)
                {
                    _host.Description.Behaviors.Add(
                         new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
                }
                else
                {
                    if (!debug.IncludeExceptionDetailInFaults)
                    {
                        debug.IncludeExceptionDetailInFaults = true;
                    }
                }

                _host.Open();
                using (EventLog eLog = new EventLog("Application"))
                {
                    EventLog.Source = "PerceiverService";
                    EventLog.WriteEntry(string.Format("API Listener Type : {0}, state : {1}, address : {2}", ServiceType, _host.State,baseaddr), EventLogEntryType.Information);
                }
            }
        }

    

    }
}
