using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.ServiceModel;
using System.Data;
using System.Configuration;
namespace CRESapi
{
    [ServiceContract]
    public interface ICRESapi
    {
        [OperationContract]
        DataSet GetProcessAudit(string caseNo);
    }
    public class CRESapi:ICRESapi
    {

        public string macAddr { get; set; }
        public DataSet GetProcessAudit(string CaseNo)
        {
            PerceiverDAL.PerceiverDAL.APPDBConnStr = ConfigurationManager.AppSettings["DBConnStr"];
            var dalapi = new PerceiverDAL.ProcessAudit();
            DataSet dsreturn = new DataSet();
            DataTable dt = new DataTable();
            dt = dalapi.GetProcessAudit(CaseNo);
            dsreturn.Tables.Add(dt);
            return dsreturn;
        }
    }
}

