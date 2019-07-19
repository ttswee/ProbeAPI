using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
namespace DataAccess
{
    public class QueryParam
    {
         public string ParamName { get; set; }
         public object ParamValue { get; set; }
    }
    public class MSSQLDB
    {
         public string ConnString { get; set; }
         public static string APPDBConnStr { get; set; }
         public string QueryText { get; set; } 
         public List<QueryParam> QueryParams {get;set;}
         public MSSQLDB(string sConnectionString="")
         {
             QueryParams = new List<QueryParam>();
             if (sConnectionString != "")
                ConnString = sConnectionString;
             else
                 ConnString = APPDBConnStr;
         }
       
        public int ExecNonQuery()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnString))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = PrepareCommand(sqlConn);
                    return sqlCmd.ExecuteNonQuery();
                    
                }
            }
            catch { throw; }
        }

        public int ExecuteScalar()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnString))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = PrepareCommand(sqlConn);
                    return (int)sqlCmd.ExecuteScalar();
                }
            }
            catch { throw; }
        }

        private SqlCommand PrepareCommand(SqlConnection _sqlConn)
        {
            using (SqlCommand sqlCmd = new SqlCommand(QueryText, _sqlConn))
            {
                //case of no parameters is passed
                if (QueryParams != null)
                {
                    foreach (QueryParam qParam in QueryParams)
                    {
                        var _ParaValue = qParam.ParamValue;
                        sqlCmd.Parameters.AddWithValue(qParam.ParamName, qParam.ParamValue);
                    }
                }
                return sqlCmd;
            }
        }
        public DataTable GetData()
        {
            DataTable dtResults = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnString))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = PrepareCommand(sqlConn);
                    using (SqlDataAdapter sqlAdapt = GetAdapter(sqlCmd))
                    {
                        sqlAdapt.Fill(dtResults);
                        return dtResults;
                    }
                }
            }
            catch { throw; }

        }

         public DataSet GetData(string dtName)
        {
            DataSet dsMain = new DataSet();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnString))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = PrepareCommand(sqlConn);
                    using (SqlDataAdapter sqlAdapt = GetAdapter(sqlCmd))
                    {
                        sqlAdapt.Fill(dsMain, dtName);
                        sqlAdapt.FillSchema(dsMain.Tables[dtName], SchemaType.Mapped);
                        return dsMain;
                    }
                }
            }
            catch { throw; }
        }

         static SqlDataAdapter GetAdapter(SqlCommand _SqlCommand)
        {
            SqlDataAdapter sAdapter = new SqlDataAdapter();
            sAdapter.SelectCommand = _SqlCommand;
            return sAdapter;
        }

         public IDataReader GetReader()
        {
            SqlConnection sqlConn = new SqlConnection(ConnString);
            sqlConn.Open();
            SqlCommand sqlCmd = PrepareCommand(sqlConn);
            SqlDataReader sReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            return sReader;
        }


         public bool InsertToDB(DataTable _SourceData)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnString))
                {
                    DataSet _dsForUpdate = new DataSet();
                    sqlConn.Open();
                    SqlCommand sqlCmd = PrepareCommand(sqlConn);
                    using (SqlDataAdapter sqlAdapt = GetAdapter(sqlCmd))
                    {
                        sqlAdapt.Fill(_dsForUpdate,"Update");
                        sqlAdapt.FillSchema(_dsForUpdate.Tables[0], SchemaType.Mapped);
                        for (int rw = 0; rw < _SourceData.Rows.Count; rw++)
                        {
                            DataRow dr = _dsForUpdate.Tables["Update"].NewRow();
                            for (int cl = 0; cl < dr.Table.Columns.Count; cl++)
                            {
                                dr[dr.Table.Columns[cl].ColumnName] = _SourceData.Rows[rw][dr.Table.Columns[cl].ColumnName];
                            }
                            _dsForUpdate.Tables["Update"].Rows.Add(dr);
                        }
                        SqlCommandBuilder  _builder = new SqlCommandBuilder(sqlAdapt);
                        _builder.GetUpdateCommand();
                        sqlAdapt.Update(_dsForUpdate,"Update");
                        _dsForUpdate.Tables["Update"].AcceptChanges();
                    }
                }
                return true;
            }
            catch { throw; }
        }
  
    }
}
