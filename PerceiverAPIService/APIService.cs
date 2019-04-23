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

        private static int RunInterval = Convert.ToInt32(ConfigurationManager.AppSettings["SleepInterval"]);
        private static int EnableRestful = Convert.ToInt32(ConfigurationManager.AppSettings["EnableRestful"]);
        private static string hostURI = ConfigurationManager.AppSettings["APIUri"];
        private ServiceHost host = null;
        protected override void OnStart(string[] args)
        {
            //using (EventLog eLog = new EventLog("Application"))
            //{
            //    EventLog.Source = "PerceiverService";
            //    EventLog.WriteEntry(string.Format("Configuration : {0}",hostURI), EventLogEntryType.Information);
            //}

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
            //Start RESTFul listening
            if (EnableRestful == 1)
            {
                //Uri baseAddress = new Uri("http://10.112.179.196:8080/PerceiverAPI");
                Uri baseAddress = new Uri(hostURI);
                host = new ServiceHost(typeof(GlobalAPI.PerceiverAPIs), baseAddress);
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

                host.Description.Behaviors.Add(smb);

                ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();

                if (debug == null)
                {
                    host.Description.Behaviors.Add(
                         new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
                }
                else
                {
                    if (!debug.IncludeExceptionDetailInFaults)
                    {
                        debug.IncludeExceptionDetailInFaults = true;
                    }
                }

                host.Open();
                using (EventLog eLog = new EventLog("Application"))
                {
                    EventLog.Source = "PerceiverService";
                    EventLog.WriteEntry(string.Format("API Listener state {0}", host.State), EventLogEntryType.Information);
                }
            }
        }

        protected override void OnStop()
        {
            bRun = false;
            if (EnableRestful == 1)
                host.Close();
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
                MSch _MaintenanceJobs = new MSch() { _AppPath = _AppPath};
                _MaintenanceJobs._AppPath = _AppPath;
                List<MaintSch> _listOfJobs = _MaintenanceJobs.GetAllJobs();
                using (EventLog eLog = new EventLog("Application"))
                {
                    EventLog.Source = "PerceiverService";
                    EventLog.WriteEntry(string.Format("Total Jobs : {0}", _listOfJobs.Count()), EventLogEntryType.Information);
                }

                MaintenanceJobs _JobExcuter = new MaintenanceJobs() { _LogPath = _LogPath };
                _JobExcuter._LogPath = _LogPath;;
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
                Thread.Sleep(RunInterval*60000);
            }
        }
    }
}
