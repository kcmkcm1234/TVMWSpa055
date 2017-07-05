using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Services
{
    public class BankRepository : IBankRepository
    {
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public BankRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<Bank> GetAllBank()
        {
            List<Bank> bankList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[GetAllBanks]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                bankList = new List<Bank>();
                                while (sdr.Read())
                                {
                                    Bank _bankObj = new Bank();
                                    {
                                        _bankObj.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : _bankObj.Code);
                                        _bankObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _bankObj.Name);
                                        _bankObj.CompanyCode = (sdr["CompanyCode"].ToString() != "" ? sdr["CompanyCode"].ToString() : _bankObj.CompanyCode);
                                       
                                    }
                                    bankList.Add(_bankObj);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return bankList;
        }

    }
}