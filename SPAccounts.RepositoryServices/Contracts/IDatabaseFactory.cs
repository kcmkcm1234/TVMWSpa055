using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface IDatabaseFactory
    {
        SqlConnection GetDBConnection();

        Boolean DisconectDB();

        OleDbConnection GetOleDBConnection(int flag, string fname);
    }
}
