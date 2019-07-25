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

    sealed class CaseNo
    {
        public string nCaseNo { get; set; }
        public string sDestFolder { get; set; }
    }

    sealed class sqlStatmentHandler
    {
        public string sSqlStatement{get;set;}
    }

    sealed class enqXML
    {
        public List<string> sysList { get; set; }
    }
}
