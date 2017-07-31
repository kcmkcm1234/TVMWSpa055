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
    public class ChartOfAccountsRepository : IChartOfAccountsRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public ChartOfAccountsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetChartOfAccountsByType
        public List<ChartOfAccounts> GetChartOfAccountsByType(string type)
        {
            List<ChartOfAccounts> chartofAccountsList = null;
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
                        cmd.CommandText = "[Accounts].[GetAccountCode]";
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = type;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                chartofAccountsList = new List<ChartOfAccounts>();
                                while (sdr.Read())
                                {
                                    ChartOfAccounts _chartofAccountsObj = new ChartOfAccounts();
                                    {
                                        _chartofAccountsObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _chartofAccountsObj.Code);
                                        _chartofAccountsObj.TypeDesc = (sdr["TypeDesc"].ToString() != "" ? (sdr["TypeDesc"].ToString()) : _chartofAccountsObj.TypeDesc);
                                        _chartofAccountsObj.ISEmploy = (sdr["ISEmpApplicable"].ToString() != "" ? bool.Parse(sdr["ISEmpApplicable"].ToString()) : _chartofAccountsObj.ISEmploy);

                                    }
                                    chartofAccountsList.Add(_chartofAccountsObj);
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

            return chartofAccountsList;
        }
        #endregion GetChartOfAccountsByType


        #region GetExpenseDetailsByValue
        public List<ChartOfAccounts> GetExpenseTypeDetails(ChartOfAccounts accObj)
        {
            List<ChartOfAccounts> chartofAccountsList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllExpenseType]";
                        cmd.Parameters.Add("@account", SqlDbType.NVarChar, 20).Value = accObj.account;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                chartofAccountsList = new List<ChartOfAccounts>();
                                while (sdr.Read())
                                {
                                    ChartOfAccounts _chartofAccountsObj = new ChartOfAccounts();
                                    {
                                        _chartofAccountsObj.Type = (sdr["Type"].ToString() != "" ? (sdr["Type"].ToString()) : _chartofAccountsObj.Type);
                                        _chartofAccountsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _chartofAccountsObj.Amount);


                                        chartofAccountsList.Add(_chartofAccountsObj);
                                    }
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

            return chartofAccountsList;
        }
        #endregion GetExpenseDetailsByValue
    }

}
