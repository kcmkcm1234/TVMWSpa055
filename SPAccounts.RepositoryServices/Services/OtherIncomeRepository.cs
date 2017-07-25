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
    public class OtherIncomeRepository : IOtherIncomeRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        int slno = 1;
        private IDatabaseFactory _databaseFactory;
        public OtherIncomeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllOtherIncome
        public List<OtherIncome> GetAllOtherIncome(string IncomeDate)
        {
            List<OtherIncome> otherIncomeList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllOtherIncome]";
                        if(!string.IsNullOrEmpty(IncomeDate))
                        {
                            cmd.Parameters.Add("@IncomeDate", SqlDbType.DateTime).Value = DateTime.Parse(IncomeDate);
                        }
                       
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherIncomeList = new List<OtherIncome>();
                                while (sdr.Read())
                                {
                                    OtherIncome _otherIncomeObj = new OtherIncome();
                                    {
                                        _otherIncomeObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _otherIncomeObj.ID);
                                        _otherIncomeObj.IncomeDate = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()) : _otherIncomeObj.IncomeDate);
                                        _otherIncomeObj.AccountCode = (sdr["AccountCode"].ToString() != "" ? (sdr["AccountCode"].ToString()) : _otherIncomeObj.AccountCode);
                                        _otherIncomeObj.PaymentRcdComanyCode = (sdr["PaymentRcdComanyCode"].ToString() != "" ? sdr["PaymentRcdComanyCode"].ToString() : _otherIncomeObj.PaymentRcdComanyCode);
                                        _otherIncomeObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : _otherIncomeObj.PaymentMode);
                                        _otherIncomeObj.DepWithdID = (sdr["DepWithdID"].ToString() != "" ? Guid.Parse(sdr["DepWithdID"].ToString()) : _otherIncomeObj.DepWithdID);
                                        _otherIncomeObj.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : _otherIncomeObj.BankCode);
                                        _otherIncomeObj.IncomeRef = (sdr["IncomeRef"].ToString() != "" ? sdr["IncomeRef"].ToString() : _otherIncomeObj.IncomeRef);
                                        _otherIncomeObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _otherIncomeObj.Description);
                                        _otherIncomeObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _otherIncomeObj.Amount);
                                        _otherIncomeObj.AccountDesc= (sdr["TypeDesc"].ToString() != "" ? (sdr["TypeDesc"].ToString()) : _otherIncomeObj.AccountDesc);

                                        _otherIncomeObj.IncomeDateFormatted = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()).ToString(s.dateformat) : _otherIncomeObj.IncomeDateFormatted);

                                        _otherIncomeObj.slNo = slno;
                                    }
                                    otherIncomeList.Add(_otherIncomeObj);
                                    slno++;
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

            return otherIncomeList;
        }
        #endregion GetAllOtherIncome

        #region GetOtherIncomeByID
        public OtherIncome GetOtherIncomeDetails(Guid ID)
        {
            OtherIncome _otherIncomeObj = new OtherIncome();
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
                        cmd.CommandText = "[Accounts].[GetOtherIncomeByID]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _otherIncomeObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _otherIncomeObj.ID);
                                    _otherIncomeObj.IncomeDate = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()) : _otherIncomeObj.IncomeDate);
                                    _otherIncomeObj.AccountCode = (sdr["AccountCode"].ToString() != "" ? (sdr["AccountCode"].ToString()) : _otherIncomeObj.AccountCode);
                                    _otherIncomeObj.PaymentRcdComanyCode = (sdr["PaymentRcdComanyCode"].ToString() != "" ? sdr["PaymentRcdComanyCode"].ToString() : _otherIncomeObj.PaymentRcdComanyCode);
                                    _otherIncomeObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : _otherIncomeObj.PaymentMode);
                                    _otherIncomeObj.DepWithdID = (sdr["DepWithdID"].ToString() != "" ? Guid.Parse(sdr["DepWithdID"].ToString()) : _otherIncomeObj.DepWithdID);
                                    _otherIncomeObj.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : _otherIncomeObj.BankCode);
                                    _otherIncomeObj.IncomeRef = (sdr["IncomeRef"].ToString() != "" ? sdr["IncomeRef"].ToString() : _otherIncomeObj.IncomeRef);
                                    _otherIncomeObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _otherIncomeObj.Description);
                                    _otherIncomeObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _otherIncomeObj.Amount);
                                    _otherIncomeObj.IncomeDateFormatted = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()).ToString(s.dateformat) : _otherIncomeObj.IncomeDateFormatted);
                                                                        
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _otherIncomeObj;
        }
        #endregion GetOtherIncomeByID

        #region InsertOtherIncome
        public OtherIncome InsertOtherIncome(OtherIncome _otherIncomeObj, AppUA ua)
        {
            try
            {
                SqlParameter outputStatus, outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[InsertOtherIncome]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IncomeDate", SqlDbType.DateTime).Value = _otherIncomeObj.IncomeDateFormatted;
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar, 10).Value = _otherIncomeObj.AccountCode;
                        cmd.Parameters.Add("@PaymentRcdComanyCode", SqlDbType.VarChar,10).Value = _otherIncomeObj.PaymentRcdComanyCode;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar,10).Value = _otherIncomeObj.PaymentMode;
                        if(_otherIncomeObj.DepWithdID!=Guid.Empty)
                        {
                            cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = _otherIncomeObj.DepWithdID;
                        }                       
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar,5).Value = _otherIncomeObj.BankCode;
                        cmd.Parameters.Add("@IncomeRef", SqlDbType.VarChar,20).Value = _otherIncomeObj.IncomeRef;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar,-1).Value = _otherIncomeObj.Description;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _otherIncomeObj.Amount;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = "Anija";
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        _otherIncomeObj.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _otherIncomeObj;
        }
        #endregion InsertOtherIncome

        #region UpdateOtherIncome
        public object UpdateOtherIncome(OtherIncome _otherIncomeObj, AppUA ua)
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
                        cmd.CommandText = "[Accounts].[UpdateOtherIncome]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _otherIncomeObj.ID;
                        cmd.Parameters.Add("@IncomeDate", SqlDbType.DateTime).Value = _otherIncomeObj.IncomeDateFormatted;
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar, 10).Value = _otherIncomeObj.AccountCode;
                        cmd.Parameters.Add("@PaymentRcdComanyCode", SqlDbType.VarChar, 10).Value = _otherIncomeObj.PaymentRcdComanyCode;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = _otherIncomeObj.PaymentMode;
                        if(_otherIncomeObj.DepWithdID!=Guid.Empty)
                        {
                            cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = _otherIncomeObj.DepWithdID;
                        }
                        
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 5).Value = _otherIncomeObj.BankCode;
                        cmd.Parameters.Add("@IncomeRef", SqlDbType.VarChar, 20).Value = _otherIncomeObj.IncomeRef;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = _otherIncomeObj.Description;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _otherIncomeObj.Amount;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = "Anija";
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(Cobj.UpdateFailure);

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
        #endregion UpdateOtherIncome

        #region DeleteOtherIncome
        public object DeleteOtherIncome(Guid ID)
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
                        cmd.CommandText = "[Accounts].[DeleteOtherIncome]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
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
        #endregion DeleteOtherIncome

    }
}