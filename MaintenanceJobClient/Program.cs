using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileMaintenance;
using System.IO;
namespace MaintenanceJobClient
{
    class Program
    {
        static void Main(string[] args)
        {

                MSch _MaintenanceJobs = new MSch();
                _MaintenanceJobs._AppPath = Directory.GetCurrentDirectory();
                List<MaintSch> _listOfJobs = _MaintenanceJobs.GetAllJobs();
                MaintenanceJobs _JobExcuter = new MaintenanceJobs();
                _JobExcuter._LogPath = Directory.GetCurrentDirectory();
                bool jStatus = false;
                foreach (MaintSch _Job in _listOfJobs)
                {
                    _JobExcuter._jobToExectute = _Job;
                    jStatus = _JobExcuter.RunJob();
                }
                _MaintenanceJobs = null;

        }
    }
}
