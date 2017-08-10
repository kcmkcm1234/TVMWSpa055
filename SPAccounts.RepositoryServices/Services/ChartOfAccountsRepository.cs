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

        #region GetAllChartOfAccounts
        public List<ChartOfAccounts> GetAllChartOfAccounts()
        {
            List<ChartOfAccounts> chartOfAccountsList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllChartOfAccounts]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                chartOfAccountsList = new List<ChartOfAccounts>();
                                while (sdr.Read())
                                {
                                    ChartOfAccounts _chartOfAccountsObj = new ChartOfAccounts();
                                    {
                                        _chartOfAccountsObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _chartOfAccountsObj.Code);
                                        _chartOfAccountsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _chartOfAccountsObj.Type);
                                        _chartOfAccountsObj.TypeDesc = (sdr["TypeDesc"].ToString() != "" ? sdr["TypeDesc"].ToString() : _chartOfAccountsObj.TypeDesc);
                                        _chartOfAccountsObj.OpeningPaymentMode = (sdr["OpeningPaymentMode"].ToString() != "" ? sdr["OpeningPaymentMode"].ToString() : _chartOfAccountsObj.OpeningPaymentMode);
                                        _chartOfAccountsObj.OpeningBalance = (sdr["OpeningBalance"].ToString() != "" ? decimal.Parse(sdr["OpeningBalance"].ToString()) : _chartOfAccountsObj.OpeningBalance);
                                        _chartOfAccountsObj.OpeningAsOfDate = (sdr["OpeningAsOfDate"].ToString() != "" ? DateTime.Parse(sdr["OpeningAsOfDate"].ToString()) : _chartOfAccountsObj.OpeningAsOfDate);
                                        _chartOfAccountsObj.ISEmploy = (sdr["ISEmpApplicable"].ToString() != "" ? bool.Parse(sdr["ISEmpApplicable"].ToString()) : _chartOfAccountsObj.ISEmploy);
                                        _chartOfAccountsObj.IsReverse = (sdr["IsReverse"].ToString() != "" ? bool.Parse(sdr["IsReverse"].ToString()) : _chartOfAccountsObj.IsReverse);
                                    }
                                    chartOfAccountsList.Add(_chartOfAccountsObj);
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

            return chartOfAccountsList;
        }
        #endregion GetAllChartOfAccounts

        #region GetChartOfAccountDetails
        public ChartOfAccounts GetChartOfAccountDetails(string Code)
        {
            ChartOfAccounts _chartOfAccountsObj = new ChartOfAccounts();
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
                        cmd.CommandText = "[Accounts].[GetChartOfAccountDetails]";
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar,10).Value = Code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _chartOfAccountsObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _chartOfAccountsObj.Code);
                                    _chartOfAccountsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _chartOfAccountsObj.Type);
                                    _chartOfAccountsObj.TypeDesc = (sdr["TypeDesc"].ToString() != "" ? sdr["TypeDesc"].ToString() : _chartOfAccountsObj.TypeDesc);
                                    _chartOfAccountsObj.OpeningPaymentMode = (sdr["OpeningPaymentMode"].ToString() != "" ? sdr["OpeningPaymentMode"].ToString() : _chartOfAccountsObj.OpeningPaymentMode);
                                    _chartOfAccountsObj.OpeningBalance = (sdr["OpeningBalance"].ToString() != "" ? decimal.Parse(sdr["OpeningBalance"].ToString()) : _chartOfAccountsObj.OpeningBalance);
                                    _chartOfAccountsObj.OpeningAsOfDate = (sdr["OpeningAsOfDate"].ToString() != "" ? DateTime.Parse(sdr["OpeningAsOfDate"].ToString()) : _chartOfAccountsObj.OpeningAsOfDate);
                                    _chartOfAccountsObj.ISEmploy = (sdr["ISEmpApplicable"].ToString() != "" ? bool.Parse(sdr["ISEmpApplicable"].ToString()) : _chartOfAccountsObj.ISEmploy);
                                    _chartOfAccountsObj.IsReverse = (sdr["IsReverse"].ToString() != "" ? bool.Parse(sdr["IsReverse"].ToString()) : _chartOfAccountsObj.IsReverse);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _chartOfAccountsObj;
        }
        #endregion GetChartOfAccountDetails

        #region InsertChartOfAccounts
        public ChartOfAccounts InsertChartOfAccounts(ChartOfAccounts _chartOfAccountsObj)
        {
            try
            {
                SqlParameter outputStatus, outputCode = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[InsertChartOfAccounts]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = _chartOfAccountsObj.Code;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = _chartOfAccountsObj.Type;
                        cmd.Parameters.Add("@TypeDesc", SqlDbType.VarChar, 200).Value = _chartOfAccountsObj.TypeDesc;
                        //cmd.Parameters.Add("@OpeningPaymentMode", SqlDbType.VarChar, 10).Value = _chartOfAccountsObj.OpeningPaymentMode;
                        //cmd.Parameters.Add("@OpeningBalance", SqlDbType.Decimal).Value = _chartOfAccountsObj.OpeningBalance;
                        //cmd.Parameters.Add("@OpeningAsOfDate", SqlDbType.DateTime).Value = _chartOfAccountsObj.OpeningAsOfDate;
                        cmd.Parameters.Add("@ISEmpApplicable", SqlDbType.Bit).Value = _chartOfAccountsObj.ISEmploy;
                        cmd.Parameters.Add("@IsReverse", SqlDbType.Bit).Value = _chartOfAccountsObj.IsReverse;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _chartOfAccountsObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _chartOfAccountsObj.commonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 10);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        _chartOfAccountsObj.Code = outputCode.Value.ToString();
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _chartOfAccountsObj;
        }
        #endregion InsertChartOfAccounts

        #region UpdateChartOfAccounts
        public object UpdateChartOfAccounts(ChartOfAccounts _chartOfAccountsObj)
        {
            SqlParameter outputStatus = null;
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
                        cmd.CommandText = "[Accounts].[UpdateChartOfAccounts]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = _chartOfAccountsObj.Code;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = _chartOfAccountsObj.Type;
                        cmd.Parameters.Add("@TypeDesc", SqlDbType.VarChar, 200).Value = _chartOfAccountsObj.TypeDesc;
                        //cmd.Parameters.Add("@OpeningPaymentMode", SqlDbType.VarChar, 10).Value = _chartOfAccountsObj.OpeningPaymentMode;
                        //cmd.Parameters.Add("@OpeningBalance", SqlDbType.Decimal).Value = _chartOfAccountsObj.OpeningBalance;
                        //cmd.Parameters.Add("@OpeningAsOfDate", SqlDbType.DateTime).Value = _chartOfAccountsObj.OpeningAsOfDate;
                        cmd.Parameters.Add("@ISEmpApplicable", SqlDbType.Bit).Value = _chartOfAccountsObj.ISEmploy;
                        cmd.Parameters.Add("@IsReverse", SqlDbType.Bit).Value = _chartOfAccountsObj.IsReverse;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _chartOfAccountsObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _chartOfAccountsObj.commonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(Cobj.UpdateFailure);
                    case "1":
                        _chartOfAccountsObj.Code = outputStatus.Value.ToString();
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = Cobj.UpdateSuccess
            };
        }
        #endregion UpdateChartOfAccounts

        #region DeleteChartOfAccounts
        public object DeleteChartOfAccounts(string code)
        {
            SqlParameter outputStatus = null;
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
                        cmd.CommandText = "[Accounts].[DeleteChartOfAccounts]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = code;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(Cobj.DeleteFailure);

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = Cobj.DeleteSuccess
            };
        }
        #endregion DeleteChartOfAccounts

    }
}