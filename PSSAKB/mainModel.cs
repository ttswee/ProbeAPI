using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSSAKB
{
    sealed  class winProcesses
    {
        public List<GlobalAPI.WindowsProcesses> allProcesses { get; set; }
    }

    sealed class winServices
    {
        public List<GlobalAPI.WindowsServices> allServices { get; set; }
    }
}
