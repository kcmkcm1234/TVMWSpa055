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
    public class DepositAndWithdrawalsRepository : IDepositAndWithdrawalsRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();

        private IDatabaseFactory _databaseFactory;
        public DepositAndWithdrawalsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllDepositAndWithdrawals
        public List<DepositAndWithdrawals> GetAllDepositAndWithdrawals(string FromDate, string ToDate, string DepositOrWithdrawal,string chqclr)
        {
            List<DepositAndWithdrawals> depositAndWithdrawalsList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllDepositsAndWithdrawals]";
                        if(!string.IsNullOrEmpty(FromDate))
                        {
                            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DateTime.Parse(FromDate);
                        }
                        if(!string.IsNullOrEmpty(ToDate))
                        {
                            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = DateTime.Parse(ToDate);
                        } 
                        if(!string.IsNullOrEmpty(chqclr))
                        {
                            cmd.Parameters.Add("@chqclr", SqlDbType.Bit).Value = bool.Parse(chqclr);
                        }                                           
                        cmd.Parameters.Add("@TransactionType", SqlDbType.Char, 1).Value = DepositOrWithdrawal;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                depositAndWithdrawalsList = new List<DepositAndWithdrawals>();
                                while (sdr.Read())
                                {
                                    DepositAndWithdrawals _depositAndWithdrawalsObj = new DepositAndWithdrawals();
                                    {
                                        _depositAndWithdrawalsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _depositAndWithdrawalsObj.ID);
                                        _depositAndWithdrawalsObj.TransactionType = (sdr["TransactionType"].ToString() != "" ? (sdr["TransactionType"].ToString()) : _depositAndWithdrawalsObj.TransactionType);
                                        _depositAndWithdrawalsObj.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? (sdr["ReferenceNo"].ToString()) : _depositAndWithdrawalsObj.ReferenceNo);
                                        _depositAndWithdrawalsObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _depositAndWithdrawalsObj.GeneralNotes);                                       
                                        _depositAndWithdrawalsObj.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : _depositAndWithdrawalsObj.BankCode);                                      
                                        _depositAndWithdrawalsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _depositAndWithdrawalsObj.Amount);
                                        _depositAndWithdrawalsObj.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(s.dateformat) : _depositAndWithdrawalsObj.DateFormatted);
                                        _depositAndWithdrawalsObj.BankName = (sdr["BankName"].ToString() != "" ? (sdr["BankName"].ToString()) : _depositAndWithdrawalsObj.BankName);

                                        _depositAndWithdrawalsObj.PaymentMode = (sdr["DepositMode"].ToString() != "" ? (sdr["DepositMode"].ToString()) : _depositAndWithdrawalsObj.PaymentMode);
                                        if(sdr["TransactionType"].ToString()=="W")
                                        {
                                            _depositAndWithdrawalsObj.ChequeStatus = "";
                                        }
                                        else
                                        {
                                            _depositAndWithdrawalsObj.ChequeStatus = (sdr["ChequeStatus"].ToString() != "" ? (sdr["ChequeStatus"].ToString()) : _depositAndWithdrawalsObj.ChequeStatus);
                                        }
                                        if(sdr["TransactionType"].ToString() == "D" && sdr["DepositMode"].ToString()=="CHEQUE" && sdr["ChequeStatus"].ToString()=="")
                                        {
                                            _depositAndWithdrawalsObj.ChequeStatus = "NotCleared";
                                        }
                                        
                                    }
                                    depositAndWithdrawalsList.Add(_depositAndWithdrawalsObj);
                                   
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

            return depositAndWithdrawalsList;
        }
        #endregion GetAllDepositAndWithdrawals

        #region GetDepositAndWithdrawalDetails
        public DepositAndWithdrawals GetDepositAndWithdrawalDetails(Guid ID)
        {
            DepositAndWithdrawals _depositAndWithdrawalsObj = new DepositAndWithdrawals();
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
                        cmd.CommandText = "[Accounts].[GetDepositAndWithdrawalDetails]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _depositAndWithdrawalsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _depositAndWithdrawalsObj.ID);
                                    _depositAndWithdrawalsObj.TransactionType = (sdr["TransactionType"].ToString() != "" ? (sdr["TransactionType"].ToString()) : _depositAndWithdrawalsObj.TransactionType);
                                    _depositAndWithdrawalsObj.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? (sdr["ReferenceNo"].ToString()) : _depositAndWithdrawalsObj.ReferenceNo);
                                    _depositAndWithdrawalsObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _depositAndWithdrawalsObj.GeneralNotes);
                                    _depositAndWithdrawalsObj.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : _depositAndWithdrawalsObj.BankCode);
                                    _depositAndWithdrawalsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _depositAndWithdrawalsObj.Amount);
                                    _depositAndWithdrawalsObj.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(s.dateformat) : _depositAndWithdrawalsObj.DateFormatted);
                                    _depositAndWithdrawalsObj.ChequeStatus = (sdr["ChequeStatus"].ToString() != "" ? (sdr["ChequeStatus"].ToString()) : _depositAndWithdrawalsObj.ChequeStatus);
                                    _depositAndWithdrawalsObj.PaymentMode = (sdr["DepositMode"].ToString() != "" ? (sdr["DepositMode"].ToString()) : _depositAndWithdrawalsObj.PaymentMode);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _depositAndWithdrawalsObj;
        }
        #endregion GetDepositAndWithdrawalDetails

        #region InsertDepositAndWithdrawals
        public DepositAndWithdrawals InsertDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj)
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
                        cmd.CommandText = "[Accounts].[InsertDepositAndWithdrawals]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if(_depositAndWithdrawalsObj.Date!=default(DateTime))
                        {
                            cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = _depositAndWithdrawalsObj.Date;
                        }
                        else
                        {
                            cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = null;
                        }                       
                        cmd.Parameters.Add("@TransactionType", SqlDbType.Char, 1).Value = _depositAndWithdrawalsObj.TransactionType;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = _depositAndWithdrawalsObj.ReferenceNo;                                     
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 5).Value = _depositAndWithdrawalsObj.BankCode;                       
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _depositAndWithdrawalsObj.Amount;
                        cmd.Parameters.Add("@ChequeStatus", SqlDbType.NVarChar, 10).Value = _depositAndWithdrawalsObj.ChequeStatus;
                        cmd.Parameters.Add("@DepositMode", SqlDbType.NVarChar, 10).Value = _depositAndWithdrawalsObj.PaymentMode;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _depositAndWithdrawalsObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _depositAndWithdrawalsObj.commonObj.CreatedDate;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, -1).Value = _depositAndWithdrawalsObj.GeneralNotes;
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
                        _depositAndWithdrawalsObj.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _depositAndWithdrawalsObj;
        }
        #endregion InsertDepositAndWithdrawals

        #region UpdateDepositAndWithdrawals
        public object UpdateDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj)
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
                        cmd.CommandText = "[Accounts].[UpdateDepositAndWithdrawals]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _depositAndWithdrawalsObj.ID;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = _depositAndWithdrawalsObj.Date;
                        cmd.Parameters.Add("@TransactionType", SqlDbType.Char, 1).Value = _depositAndWithdrawalsObj.TransactionType;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = _depositAndWithdrawalsObj.ReferenceNo;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 5).Value = _depositAndWithdrawalsObj.BankCode;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _depositAndWithdrawalsObj.Amount;
                        cmd.Parameters.Add("@ChequeStatus", SqlDbType.NVarChar, 10).Value = _depositAndWithdrawalsObj.ChequeStatus;
                        cmd.Parameters.Add("@DepositMode", SqlDbType.NVarChar, 10).Value = _depositAndWithdrawalsObj.PaymentMode;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _depositAndWithdrawalsObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _depositAndWithdrawalsObj.commonObj.UpdatedDate;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, -1).Value = _depositAndWithdrawalsObj.GeneralNotes;
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
        #endregion UpdateDepositAndWithdrawals

        #region ClearCheque
        public object ClearCheque(Guid ID)
        {
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
                        cmd.CommandText = "[Accounts].[ClearCheque]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value =ID;
                        cmd.ExecuteNonQuery();


                    }
                }

             

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {                
                Message = Cobj.UpdateSuccess
            };
        }
        #endregion ClearCheque
    }
}