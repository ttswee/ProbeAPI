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
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.IO;
using System.Diagnostics;
using PerceiverDAL;
using System.Security.Cryptography;
using System.Collections.ObjectModel;
namespace CRESapi
{
    public class ProcessLogs
    {
        private string computerName { get; set; }
        private string queryText { get; set; }
        private bool status { get; set; }
    }

    public class fileSendLogs
    {
        private string logfilename { get; set; }
        private string senddate{get;set;}
        private string sendstatus {get;set;}
        private string message {get;set;}
    }
    
    public class ProcessCode
    {
        public int status { get; set; }
        public string xmlName { get; set; }
        public static List<ProcessCode> pCodes = new List<ProcessCode>();

        public static void setProcessCodes()
        {
            PerceiverDAL.PerceiverDAL.APPDBConnStr = ConfigurationManager.AppSettings["DBConnStr"];
            DataTable CD_PROCESS = new DataTable();
            CD_PROCESS = new ProcessCodeTable().getCD_Process();
            for (int icnt = 0; icnt < CD_PROCESS.Rows.Count - 1; icnt++)
            {
                pCodes.Add(new ProcessCode { status = (int)CD_PROCESS.Rows[icnt]["CD_PSTATUS_CODE"], xmlName = CD_PROCESS.Rows[icnt]["CD_PSTATUS_TYPE"].ToString() });
            }
        }
    }

    public class validUsers
    {
        public string macName { get; set; }
        public string userName { get; set; }
    }

    public static class userList
    {

        public static List<validUsers> usersList { get; set; }

        public static void LoadList()
        {
            usersList = new List<validUsers>();
            usersList.Add(new validUsers() { macName = "MYNPF0K01YF", userName = "1467766" });
            usersList.Add(new validUsers() { macName = "MYNPF0QF95K", userName = "1434247" });
            usersList.Add(new validUsers() { macName = "MYNPF0QEV3P", userName = "1450703" });
            usersList.Add(new validUsers() { macName = "MYNPF0DQFDS", userName = "1498354" });
        }

    }

    public class interfaceFiles
    {
        public string fileName { get; set; }
        public byte[] encFile { get; set; }
    }

    [ServiceContract]
    public interface ICRESapi
    {
        [OperationContract(IsOneWay = false)]
        DataTable GetProcessAudit(string caseNo);

        [OperationContract(IsOneWay = false)]
        DataTable GetGARecords();

        [OperationContract]
        List<interfaceFiles> GetInterFaceFile(string caseNo, string pstatus, string ifPah, string secToken);


        [OperationContract(IsOneWay = false)]
        int UpdateQueues(string sqlStatement, string secToken);

        [OperationContract(IsOneWay = false)]
        List<fileSendLogs> GetSendLogs(DateTime StartDate, int NoOfDays);


    }


    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class CRESapi : ICRESapi
    {

        public DataTable GetProcessAudit(string CaseNo)
        {
            try
            {
                PerceiverDAL.PerceiverDAL.APPDBConnStr = ConfigurationManager.AppSettings["DBConnStr"];
                var dalapi = new PerceiverDAL.ProcessAudit();
                DataTable dt = new DataTable();

                dt = dalapi.GetProcessAudit(CaseNo);
                dt.TableName = "ProcessAudit";
                Console.WriteLine(dt.Rows.Count);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ex.ToString();
                throw;
            }
        }

        public DataTable GetGARecords()
        {

            PerceiverDAL.PerceiverDAL.APPDBConnStr = ConfigurationManager.AppSettings["DBConnStr"];
            var dalapi = new PerceiverDAL.GATables();
            DataTable dt = new DataTable();

            dt = dalapi.GetAllRecords();

            return dt;
        }

        public bool execStatement(string sqlStatement, string MachineName)
        {
            PerceiverDAL.PerceiverDAL.APPDBConnStr = ConfigurationManager.AppSettings["DBConnStr"];
            return false;
        }


        public List<interfaceFiles> GetInterFaceFile(string caseNo, string pstatus, string ifPath, string secToken)
        {
            try
            {
                PerceiverDAL.PerceiverDAL.APPDBConnStr = ConfigurationManager.AppSettings["DBConnStr"];
                List<interfaceFiles> iFiles = new List<interfaceFiles>();
                List<string> fileNames = new List<string>();

                DirectoryInfo dirInfo;
                
                if (validateToken(secToken))
                {
                    dirInfo = new DirectoryInfo(ifPath);
                        List<FileInfo> ListToRet = dirInfo.EnumerateFiles().Take(500000).ToList();
                        List<FileInfo> found = ListToRet.FindAll(x => x.Name.Contains(caseNo));
                        string[] allstates = pstatus.Split(',');
                        foreach (string istatus in allstates)
                        {
                            ProcessCode pcode = ProcessCode.pCodes.Find(x => x.status == int.Parse(istatus));
                            if (pcode != null)
                            {
                                List<FileInfo> foundstatus = found.FindAll(x => x.Name.Contains(pcode.xmlName));
                                if (foundstatus.Count > 0)
                                {
                                    foreach (FileInfo fi in foundstatus)
                                    {
                                        iFiles.Add(new interfaceFiles { fileName = fi.Name, encFile = encFile(File.ReadAllBytes(fi.FullName),secToken)});
                                    }
                                }
                            }
                        }
                    return iFiles;
                }
                else
                {
                    return iFiles;
                }
            }
            catch (Exception ex)
            {
                using (EventLog eLog = new EventLog("Application"))
                {
                    eLog.Source = "PerceiverService";
                    eLog.WriteEntry(string.Format("Exception {0}", ex.ToString()));
                }
                return new List<interfaceFiles>();
            }
        }


        private static bool validateToken(string secToken)
        {

            string dectoken = DecryptToken(secToken);
            string[] tokenval = dectoken.Split('!');
            DateTime tokentime = DateTime.Now;
            userList.LoadList();

            var result = userList.usersList.FindAll(x => x.macName == tokenval[0] && x.userName == tokenval[1]);
            if (result.Count > 0)
            {
                DateTime.TryParse(tokenval[2], out tokentime);
                TimeSpan totalSecs = DateTime.Now - tokentime;
                if (totalSecs.TotalSeconds > 5)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }



        public static string DecryptToken(string cipherText)
        {

            string EncryptionKey = "cresrocks";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        private static byte[] encFile(byte[] uncText, string EncryptKey = "")
        {
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pik = new Rfc2898DeriveBytes(EncryptKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pik.GetBytes(32);
                encryptor.IV = pik.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(uncText, 0, uncText.Length);
                        cs.Close();
                    }
                    return ms.ToArray();
                }
            }
        }


        public int UpdateQueues(string sqlStatement, string secToken)
        {
            if (validateToken(secToken) == false)
            {
                return -1;
            }

            return PerceiverDAL.UpdateQueues.UpdateEBBSQueue(sqlStatement);

        }




        public List<fileSendLogs> GetSendLogs(DateTime StartDate, int NoOfDays)
        {
            string pdwpath = ConfigurationManager.AppSettings["PDWLogPath"];
            string ifrs = ConfigurationManager.AppSettings["IFRSLogPath"];

            return new List<fileSendLogs>();
        }


        private static void CheckPDWFiles(string filename, DateTime checkdate)
        {
            if (!File.Exists(filename))
                return;
            bool bErr = false;
            int logstart = 0;
            var fileContent = File.ReadLines(filename, Encoding.UTF8).ToArray();
            DateTime foundate = DateTime.Today;
            bool isdate = false;
            string dateline = "";
            for (int ln = 0; ln <= fileContent.Count() - 1; ln++)
            {
                dateline = "";
                if (fileContent[ln].ToString() == "\"*** Start ***\" ")
                {
                    ++ln;
                    if (fileContent[ln].Length > 8)
                        dateline = new string(fileContent[ln].Reverse().Take(10).Reverse().ToArray());
                    isdate = DateTime.TryParse(dateline, out foundate);
                    if (isdate && checkdate == foundate)
                    {
                        logstart = ln;
                        bErr = false;
                        for (int finderror = logstart; logstart < fileContent.Count(); logstart++)
                        {
                            if (fileContent[logstart].ToString() == "\"*** Start ***\" ")
                                break;
                            if (fileContent[logstart].ToString() == "All channels closed. Disconnecting")
                                break;

                            if (fileContent[logstart].Contains("error"))
                            {
                                bErr = true;
                                break;
                            }
                        }
                        if (bErr)
                        {
                            Console.WriteLine(filename);
                            Console.WriteLine("Send error detected for date : {0}", foundate.ToString("dd/MM/yyyy"));
                            Console.WriteLine("in file :{0} ", fileContent[logstart]);

                        }
                    }
                }
            }
        }


        private static void CheckIFRS(string ifrs)
        {
            string filedatepart = "EDM_IFRS_LOG_" + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Today.Year.ToString() + ".txt";
            //string filedatepart = "EDM_IFRS_LOG_04072019.txt";
            string filename = Path.Combine(ifrs, filedatepart);
            if (File.Exists(filename))
            {
                var fileContent = File.ReadLines(filename).ToArray();
                for (int i = 0; i < fileContent.Count(); i++)
                {
                    if (fileContent[i].Contains("Reason Code"))
                    {
                        Console.WriteLine("IFRS Failed");
                        break;
                    }
                }
            }
        }


    }

}

