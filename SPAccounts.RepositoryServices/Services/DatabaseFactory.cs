using System; 
using SPAccounts.RepositoryServices.Contracts;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;


namespace SPAccounts.RepositoryServices.Services
{
    public class DatabaseFactory: IDatabaseFactory
    {
        private SqlConnection SQLCon = null;

        private OleDbConnection OleDbCon = null;

        public SqlConnection GetDBConnection()
        {
            try
            {
                SQLCon = new SqlConnection(ConfigurationManager.ConnectionStrings["SPAppsConnection"].ConnectionString);                 

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return SQLCon;
        }


        public Boolean DisconectDB()
        {
            try
            {
                if (SQLCon.State == ConnectionState.Open)
                {
                    SQLCon.Close();
                    SQLCon.Dispose();
                    return true;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public OleDbConnection GetOleDBConnection(int flag, string fname)
        {
            try
            {
                string conString = string.Empty;
                switch (flag)
                {
                    case 1: //Excel 97-03
                        conString = string.Format(ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString, fname);
                        break;
                    case 2: //Excel 07 or higher
                        conString = string.Format(ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString, fname);
                        break;
                    case 3: //
                        conString = string.Format(ConfigurationManager.ConnectionStrings["Excel12+ConString"].ConnectionString, fname);
                        break;
                }
                OleDbCon = new OleDbConnection(conString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OleDbCon;
        }
    }
}