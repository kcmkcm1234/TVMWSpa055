using System; 
using SPAccounts.RepositoryServices.Contracts;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SPAccounts.RepositoryServices.Services
{
    public class DatabaseFactory: IDatabaseFactory
    {
        private SqlConnection SQLCon = null;


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
    }
}