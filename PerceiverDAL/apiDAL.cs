using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using System.Data;
using System.Diagnostics;
namespace PerceiverDAL
{
    public class PerceiverDAL
    {
        public static string APPDBConnStr { get { return APPDBConnStr; } set { DataAccess.MSSQLDB.APPDBConnStr = value; } }
        public string ConnStr { get; set; }
        public PerceiverDAL(string ConnStr = "")
        {
            if (ConnStr != "")
                MSSQLDB.APPDBConnStr = ConnStr;
            else
                MSSQLDB.APPDBConnStr = APPDBConnStr;
        }
    }

    public class ProcessAudit
    {
        public DataTable GetProcessAudit(string caseno)
        {
            var _getProcessAudit =  new MSSQLDB();
            _getProcessAudit.QueryText = "SELECT * FROM PROCESS_AUDIT WHERE PROC_CASE_NO = @CASENO order by PROC_START_TIME";
            _getProcessAudit.QueryParams.Add(new QueryParam{ParamName= "@caseno",ParamValue=caseno});
            return _getProcessAudit.GetData();
        }
    }


}
