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
                                        _depositAndWithdrawalsObj.TransferID = (sdr["TransferId"].ToString() != "" ? Guid.Parse(sdr["TransferId"].ToString()) : _depositAndWithdrawalsObj.TransferID);
                                        _depositAndWithdrawalsObj.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : _depositAndWithdrawalsObj.CustomerID);
                                        _depositAndWithdrawalsObj.CustomerName = (sdr["CompanyName"].ToString() != "" ? (sdr["CompanyName"].ToString()) : _depositAndWithdrawalsObj.CustomerName);
                                        _depositAndWithdrawalsObj.TransactionType = (sdr["TransactionType"].ToString() != "" ? (sdr["TransactionType"].ToString()) : _depositAndWithdrawalsObj.TransactionType);
                                        _depositAndWithdrawalsObj.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? (sdr["ReferenceNo"].ToString()) : _depositAndWithdrawalsObj.ReferenceNo);
                                        _depositAndWithdrawalsObj.GeneralNotes = (sdr["GeneralNote"].ToString() != "" ? sdr["GeneralNote"].ToString() : _depositAndWithdrawalsObj.GeneralNotes);                                       
                                        _depositAndWithdrawalsObj.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : _depositAndWithdrawalsObj.BankCode);                                      
                                        _depositAndWithdrawalsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _depositAndWithdrawalsObj.Amount);
                                        _depositAndWithdrawalsObj.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(s.dateformat) : _depositAndWithdrawalsObj.DateFormatted);
                                        _depositAndWithdrawalsObj.ChequeFormatted = (sdr["ChequeClearDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeClearDate"].ToString()).ToString(s.dateformat) : _depositAndWithdrawalsObj.ChequeFormatted);
                                        _depositAndWithdrawalsObj.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString(s.dateformat) : _depositAndWithdrawalsObj.ChequeDate);
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
                                        if (sdr["DepositMode"].ToString() == "ONLINE")
                                        {
                                            _depositAndWithdrawalsObj.ChequeStatus = "";
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
            DepositAndWithdrawals depositAndWithdrawalsObj = new DepositAndWithdrawals();
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
                                    depositAndWithdrawalsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : depositAndWithdrawalsObj.ID);
                                    depositAndWithdrawalsObj.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()):Guid.Empty);
                                    depositAndWithdrawalsObj.TransactionType = (sdr["TransactionType"].ToString() != "" ? (sdr["TransactionType"].ToString()) : depositAndWithdrawalsObj.TransactionType);
                                    depositAndWithdrawalsObj.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? (sdr["ReferenceNo"].ToString()) : depositAndWithdrawalsObj.ReferenceNo);
                                    depositAndWithdrawalsObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : depositAndWithdrawalsObj.GeneralNotes);
                                    depositAndWithdrawalsObj.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : depositAndWithdrawalsObj.BankCode);
                                    depositAndWithdrawalsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : depositAndWithdrawalsObj.Amount);
                                    depositAndWithdrawalsObj.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(s.dateformat) : depositAndWithdrawalsObj.DateFormatted);
                                    depositAndWithdrawalsObj.ChequeFormatted = (sdr["ChequeClearDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeClearDate"].ToString()).ToString(s.dateformat) : depositAndWithdrawalsObj.ChequeFormatted);
                                    depositAndWithdrawalsObj.ChequeStatus = (sdr["ChequeStatus"].ToString() != "" ? (sdr["ChequeStatus"].ToString()) : depositAndWithdrawalsObj.ChequeStatus);
                                    depositAndWithdrawalsObj.PaymentMode = (sdr["DepositMode"].ToString() != "" ? (sdr["DepositMode"].ToString()) : depositAndWithdrawalsObj.PaymentMode);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return depositAndWithdrawalsObj;
        }
        #endregion GetDepositAndWithdrawalDetails
        
        #region GetTransferCashDetailsById
        public DepositAndWithdrawals GetTransferCashById(Guid TransferId)
        {
            DepositAndWithdrawals depositAndWithdrawalsObj = new DepositAndWithdrawals();
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
                        cmd.CommandText = "[Accounts].[GetCashTransferById]";
                        cmd.Parameters.Add("@TransferId", SqlDbType.UniqueIdentifier).Value = TransferId;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    depositAndWithdrawalsObj.TransferID = (sdr["TransferId"].ToString() != "" ? Guid.Parse(sdr["TransferId"].ToString()) : depositAndWithdrawalsObj.TransferID);
                                    depositAndWithdrawalsObj.FromBank = (sdr["FromBank"].ToString() != "" ? (sdr["FromBank"].ToString()) : depositAndWithdrawalsObj.FromBank);
                                    depositAndWithdrawalsObj.ToBank = (sdr["ToBank"].ToString() != "" ? (sdr["ToBank"].ToString()) : depositAndWithdrawalsObj.ToBank);
                                    depositAndWithdrawalsObj.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? (sdr["ReferenceNo"].ToString()) : depositAndWithdrawalsObj.ReferenceNo);
                                    depositAndWithdrawalsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : depositAndWithdrawalsObj.Amount);
                                    depositAndWithdrawalsObj.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(s.dateformat) : depositAndWithdrawalsObj.DateFormatted);
                                    //_depositAndWithdrawalsObj.ChequeFormatted = (sdr["ChequeClearDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeClearDate"].ToString()).ToString(s.dateformat) : _depositAndWithdrawalsObj.ChequeFormatted);
                                    //_depositAndWithdrawalsObj.ChequeStatus = (sdr["ChequeStatus"].ToString() != "" ? (sdr["ChequeStatus"].ToString()) : _depositAndWithdrawalsObj.ChequeStatus);
                                    //_depositAndWithdrawalsObj.PaymentMode = (sdr["DepositMode"].ToString() != "" ? (sdr["DepositMode"].ToString()) : _depositAndWithdrawalsObj.PaymentMode);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return depositAndWithdrawalsObj;
        }
        #endregion GetTransferCashDetailsById
        
        #region InsertDepositAndWithdrawals
        public DepositAndWithdrawals InsertDepositAndWithdrawals(DepositAndWithdrawals depositAndWithdrawalsObj)
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
                        if(depositAndWithdrawalsObj.Date!=default(DateTime))
                        {
                            cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = depositAndWithdrawalsObj.Date;
                        }
                        else
                        {
                            cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = null;
                        }                       
                         cmd.Parameters.Add("@TransactionType", SqlDbType.Char, 1).Value = depositAndWithdrawalsObj.TransactionType;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = depositAndWithdrawalsObj.ReferenceNo;                                     
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 5).Value = depositAndWithdrawalsObj.BankCode;                       
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = depositAndWithdrawalsObj.Amount;
                        cmd.Parameters.Add("@ChequeStatus", SqlDbType.NVarChar, 10).Value = depositAndWithdrawalsObj.ChequeStatus;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = depositAndWithdrawalsObj.CustomerID;
                        cmd.Parameters.Add("@DepositMode", SqlDbType.NVarChar, 10).Value = depositAndWithdrawalsObj.PaymentMode;
                        if (depositAndWithdrawalsObj.ChequeClearDate != default(DateTime))
                        {
                            cmd.Parameters.Add("@ChequeClearDate", SqlDbType.DateTime).Value = depositAndWithdrawalsObj.ChequeClearDate;
                        } 
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = depositAndWithdrawalsObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = depositAndWithdrawalsObj.commonObj.CreatedDate;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, -1).Value = depositAndWithdrawalsObj.GeneralNotes;
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
                        depositAndWithdrawalsObj.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return depositAndWithdrawalsObj;
        }
        #endregion InsertDepositAndWithdrawals

        #region UpdateDepositAndWithdrawals
        public object UpdateDepositAndWithdrawals(DepositAndWithdrawals depositAndWithdrawalsObj)
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = depositAndWithdrawalsObj.ID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = depositAndWithdrawalsObj.CustomerID;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = depositAndWithdrawalsObj.Date;
                        cmd.Parameters.Add("@TransactionType", SqlDbType.Char, 1).Value = depositAndWithdrawalsObj.TransactionType;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = depositAndWithdrawalsObj.ReferenceNo;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 5).Value = depositAndWithdrawalsObj.BankCode;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = depositAndWithdrawalsObj.Amount;
                        cmd.Parameters.Add("@ChequeStatus", SqlDbType.NVarChar, 10).Value = depositAndWithdrawalsObj.ChequeStatus;
                        if (depositAndWithdrawalsObj.ChequeClearDate != default(DateTime))
                        {
                            cmd.Parameters.Add("@ChequeClearDate", SqlDbType.DateTime).Value = depositAndWithdrawalsObj.ChequeClearDate;
                        }
                        //cmd.Parameters.Add("@ChequeClearDate", SqlDbType.DateTime).Value = _depositAndWithdrawalsObj.ChequeClearDate;
                        cmd.Parameters.Add("@DepositMode", SqlDbType.NVarChar, 10).Value = depositAndWithdrawalsObj.PaymentMode;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = depositAndWithdrawalsObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = depositAndWithdrawalsObj.commonObj.UpdatedDate;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, -1).Value = depositAndWithdrawalsObj.GeneralNotes;
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
        public object ClearCheque(Guid ID,string date)
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
                        cmd.Parameters.Add("@ChequeClearDate", SqlDbType.Date).Value = date;
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

        #region GetUndepositedCheque
        /// <summary>
        /// To get Undeposited cheques
        /// </summary>
        /// <param name="undepositedChequeAdvanceSearchObject"></param>
        /// <returns>unDepositedChequeList</returns>
        public List<DepositAndWithdrawals> GetUndepositedCheque(UndepositedChequeAdvanceSearch undepositedChequeAdvanceSearchObject)
        {
            List<DepositAndWithdrawals> unDepositedChequeList = null;
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
                        cmd.CommandText = "[Accounts].[GetUndepositedCheques]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = undepositedChequeAdvanceSearchObject.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = undepositedChequeAdvanceSearchObject.ToDate;
                        cmd.Parameters.Add("@Company", SqlDbType.NVarChar).Value = undepositedChequeAdvanceSearchObject.Company;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = undepositedChequeAdvanceSearchObject.Customer;
                        cmd.Parameters.Add("@BankCode", SqlDbType.NVarChar).Value = undepositedChequeAdvanceSearchObject.BankCode;
                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 250).Value = undepositedChequeAdvanceSearchObject.Search;                     

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                unDepositedChequeList = new List<DepositAndWithdrawals>();
                                while (sdr.Read())
                                {
                                    DepositAndWithdrawals depositAndWithdrawalsObj = new DepositAndWithdrawals();
                                    {
                                        depositAndWithdrawalsObj.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(s.dateformat) : depositAndWithdrawalsObj.DateFormatted);
                                        //_depositAndWithdrawalsObj.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : _depositAndWithdrawalsObj.Date);
                                        depositAndWithdrawalsObj.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? (sdr["ReferenceNo"].ToString()) : depositAndWithdrawalsObj.ReferenceNo);
                                        depositAndWithdrawalsObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : depositAndWithdrawalsObj.CustomerName);
                                        depositAndWithdrawalsObj.CompanyObj = new Companies();
                                        {
                                            depositAndWithdrawalsObj.CompanyObj.Name = (sdr["Company"].ToString() != "" ? sdr["Company"].ToString() : depositAndWithdrawalsObj.CompanyObj.Name);
                                        }                                       
                                        depositAndWithdrawalsObj.ReferenceBank = (sdr["ReferenceBank"].ToString() != "" ? (sdr["ReferenceBank"].ToString()) : depositAndWithdrawalsObj.ReferenceBank);
                                        depositAndWithdrawalsObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? (sdr["GeneralNotes"].ToString()) : depositAndWithdrawalsObj.GeneralNotes);
                                        depositAndWithdrawalsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : depositAndWithdrawalsObj.Amount);                                    
                                    }
                                    unDepositedChequeList.Add(depositAndWithdrawalsObj);
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
            return unDepositedChequeList;
        }
        #endregion GetUndepositedCheque



        public List<OutGoingCheques> GetOutGoingCheques(OutgoingChequeAdvanceSearch advanceSearchObject)
        {
            List<OutGoingCheques> outgoingcheque = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = advanceSearchObject.FromDate == "" ? null : advanceSearchObject.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = advanceSearchObject.ToDate;
                        cmd.Parameters.Add("@Company", SqlDbType.NVarChar,20).Value = advanceSearchObject.Company;
                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = advanceSearchObject.Status;
                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 20).Value = advanceSearchObject.Search;
                        cmd.CommandText = "[Accounts].[GetAllOutGoingCheques]";
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                outgoingcheque = new List<OutGoingCheques>();
                                while (sdr.Read())
                                {
                                    OutGoingCheques depositAndWithdrawalsObj = new OutGoingCheques();
                                    {
                                        depositAndWithdrawalsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : depositAndWithdrawalsObj.ID);
                                        depositAndWithdrawalsObj.ChequeNo= (sdr["ChequeNo"].ToString() != "" ? (sdr["ChequeNo"].ToString()) : depositAndWithdrawalsObj.ChequeNo);
                                        depositAndWithdrawalsObj.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString(s.dateformat) : depositAndWithdrawalsObj.ChequeDate);
                                        depositAndWithdrawalsObj.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(s.dateformat) : depositAndWithdrawalsObj.CreatedDate);
                                        depositAndWithdrawalsObj.Bank = (sdr["BankName"].ToString() != "" ? (sdr["BankName"].ToString()) : depositAndWithdrawalsObj.Bank);
                                        depositAndWithdrawalsObj.Party = (sdr["Party"].ToString() != "" ? (sdr["Party"].ToString()) : depositAndWithdrawalsObj.Party);
                                        depositAndWithdrawalsObj.Status = (sdr["Status"].ToString() != "" ? (sdr["Status"].ToString()) : depositAndWithdrawalsObj.Status);
                                        depositAndWithdrawalsObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : depositAndWithdrawalsObj.Remarks);
                                        depositAndWithdrawalsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : depositAndWithdrawalsObj.Amount);
                                        depositAndWithdrawalsObj.Company = (sdr["Company"].ToString() != "" ?(sdr["Company"].ToString()) : depositAndWithdrawalsObj.Company);

                                    }

                                    outgoingcheque.Add(depositAndWithdrawalsObj);
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

            return outgoingcheque;
        }
        #region GetUndepositedChequeCount
        public string GetUndepositedChequeCount(string Date)
        {
            DepositAndWithdrawals depositAndWithdrawalsObj = new DepositAndWithdrawals();
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
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;
                        cmd.CommandText = "[Accounts].[GetUndepositedChequesCount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        depositAndWithdrawalsObj.UndepositedChequeCount=cmd.ExecuteScalar().ToString();
                      
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return depositAndWithdrawalsObj.UndepositedChequeCount;
        }
        #endregion GetUndepositedChequeCount
        
        #region CashTransferBetweenBanks

        public DepositAndWithdrawals TransferAmount(DepositAndWithdrawals _depositAndWithdrwalObj)
        {
            DepositAndWithdrawals WithdrawalsAndObjdeposit = new DepositAndWithdrawals();
            try
            {
                SqlParameter outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[InsertTransferAmount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.NVarChar,20).Value = _depositAndWithdrwalObj.ReferenceNo;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = _depositAndWithdrwalObj.Date;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _depositAndWithdrwalObj.Amount;
                        cmd.Parameters.Add("@FromBankCode", SqlDbType.NVarChar,20).Value = _depositAndWithdrwalObj.FromBank;
                        cmd.Parameters.Add("@ToBankCode", SqlDbType.NVarChar,20).Value = _depositAndWithdrwalObj.ToBank;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _depositAndWithdrwalObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _depositAndWithdrwalObj.commonObj.CreatedDate;
                        outputID = cmd.Parameters.Add("@TransferID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                if (outputID.Value != null)
                {
                    WithdrawalsAndObjdeposit.TransferID = Guid.Parse(outputID.Value.ToString());
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return WithdrawalsAndObjdeposit;
        }


        #endregion CashTransferBetweenBanks

        #region UpdateTransferredAmount
        public object UpdateTransferAmount(DepositAndWithdrawals depositAndWithdrwalObj)
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
                        cmd.CommandText = "[Accounts].[UpdateTransferAmount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TransferId", SqlDbType.UniqueIdentifier).Value = depositAndWithdrwalObj.TransferID;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = depositAndWithdrwalObj.Date;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = depositAndWithdrwalObj.ReferenceNo;
                        cmd.Parameters.Add("@FromBankCode", SqlDbType.VarChar, 20).Value = depositAndWithdrwalObj.FromBank;
                        cmd.Parameters.Add("@ToBankCode", SqlDbType.VarChar, 20).Value = depositAndWithdrwalObj.ToBank;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = depositAndWithdrwalObj.Amount;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = depositAndWithdrwalObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = depositAndWithdrwalObj.commonObj.UpdatedDate;
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
 
        #endregion UpdateTransferredAmount

        #region DeleteDepositandwithdrawal
        public object DeleteDepositandwithdrawal(Guid ID, string UserName)
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
                        cmd.CommandText = "[Accounts].[DeleteDepositandwithdrawal]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar, 250).Value = UserName;
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
        #endregion DeleteDepositandwithdrawal

        #region ClearChequeOut
        public object ClearChequeOut(Guid ID, string Date)
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
                        cmd.CommandText = "[Accounts].[ClearChequeOut]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@ChequeClearDate", SqlDbType.DateTime).Value = Date;
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

        #region GetAllWithdrawals
        public List<DepositAndWithdrawals> GetAllWithdrawals()
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
                        cmd.CommandText = "[Accounts].[GetAllWithdrawals]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                depositAndWithdrawalsList = new List<DepositAndWithdrawals>();
                                while (sdr.Read())
                                {
                                    DepositAndWithdrawals depositAndWithdrawalsObj = new DepositAndWithdrawals();
                                    {
                                        depositAndWithdrawalsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : depositAndWithdrawalsObj.ID);
                                        
                                        depositAndWithdrawalsObj.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : depositAndWithdrawalsObj.CustomerID);
                                        depositAndWithdrawalsObj.CustomerName = (sdr["CompanyName"].ToString() != "" ? (sdr["CompanyName"].ToString()) : depositAndWithdrawalsObj.CustomerName);
                                        depositAndWithdrawalsObj.TransactionType = (sdr["TransactionType"].ToString() != "" ? (sdr["TransactionType"].ToString()) : depositAndWithdrawalsObj.TransactionType);
                                        depositAndWithdrawalsObj.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? (sdr["ReferenceNo"].ToString()) : depositAndWithdrawalsObj.ReferenceNo);
                                        depositAndWithdrawalsObj.GeneralNotes = (sdr["GeneralNote"].ToString() != "" ? sdr["GeneralNote"].ToString() : depositAndWithdrawalsObj.GeneralNotes);
                                        depositAndWithdrawalsObj.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : depositAndWithdrawalsObj.BankCode);
                                        depositAndWithdrawalsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : depositAndWithdrawalsObj.Amount);
                                        depositAndWithdrawalsObj.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(s.dateformat) : depositAndWithdrawalsObj.DateFormatted);
                                        depositAndWithdrawalsObj.ChequeFormatted = (sdr["ChequeClearDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeClearDate"].ToString()).ToString(s.dateformat) : depositAndWithdrawalsObj.ChequeFormatted);
                                        depositAndWithdrawalsObj.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString(s.dateformat) : depositAndWithdrawalsObj.ChequeDate);
                                        depositAndWithdrawalsObj.BankName = (sdr["BankName"].ToString() != "" ? (sdr["BankName"].ToString()) : depositAndWithdrawalsObj.BankName);

                                        depositAndWithdrawalsObj.PaymentMode = (sdr["DepositMode"].ToString() != "" ? (sdr["DepositMode"].ToString()) : depositAndWithdrawalsObj.PaymentMode);
                                        
                                    }
                                    depositAndWithdrawalsList.Add(depositAndWithdrawalsObj);

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
        #endregion GetAllWithdrawals

        #region DeleteTransferAmountBetweenBanks
        public object DeleteTransferAmount(Guid TransferID, string UserName)
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
                        cmd.CommandText = "[Accounts].[DeleteTransferAmount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TransferID", SqlDbType.UniqueIdentifier).Value = TransferID;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar, 250).Value = UserName;
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
        #endregion DeleteTransferAmountBetweenBanks




        public OutGoingCheques InsertOutgoingCheques(OutGoingCheques outGoingChequeObj)
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
                        cmd.CommandText = "[Accounts].[InsertOutgoingCheques]";
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        cmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar,50).Value = outGoingChequeObj.ChequeNo;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = outGoingChequeObj.ChequeDate;
                        cmd.Parameters.Add("@Bank", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.Bank;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = outGoingChequeObj.Amount;
                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.Status;
                        cmd.Parameters.Add("@Party", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.Party;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = outGoingChequeObj.Remarks;
                        cmd.Parameters.Add("@FromCompany", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.Company;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = outGoingChequeObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = outGoingChequeObj.commonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Outstatus", SqlDbType.SmallInt);
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
                        outGoingChequeObj.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return outGoingChequeObj;
        }



        public object UpdateOutgoingCheques(OutGoingCheques outGoingChequeObj)
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
                        cmd.CommandText = "[Accounts].[UpdateOutgoingCheques]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = outGoingChequeObj.ID;
                        cmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.ChequeNo;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = outGoingChequeObj.ChequeDate;
                        cmd.Parameters.Add("@Bank", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.Bank;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = outGoingChequeObj.Amount;
                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.Status;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = outGoingChequeObj.Remarks;
                        cmd.Parameters.Add("@Party", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.Party;
                        cmd.Parameters.Add("@Company", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.Company;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = outGoingChequeObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = outGoingChequeObj.commonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@OutStatus", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
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



        public object DeleteOutgoingCheque(Guid ID)
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
                        cmd.CommandText = "[Accounts].[DeleteOutgoingCheque]";
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



        public OutGoingCheques GetOutgoingChequeById(Guid ID)
        {
            OutGoingCheques outGoingChequesObj = new OutGoingCheques();
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
                        cmd.CommandText = "[Accounts].[GetOutgoingChequesByID]";
                        cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    outGoingChequesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : outGoingChequesObj.ID);
                                    outGoingChequesObj.ChequeNo = (sdr["ChequeNo"].ToString() != "" ? (sdr["ChequeNo"].ToString()) : outGoingChequesObj.ChequeNo);
                                    outGoingChequesObj.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString(s.dateformat) : outGoingChequesObj.ChequeDate);
                                    outGoingChequesObj.Bank = (sdr["Bank"].ToString() != "" ? (sdr["Bank"].ToString()) : outGoingChequesObj.Bank);
                                    outGoingChequesObj.Party = (sdr["Party"].ToString() != "" ? (sdr["Party"].ToString()) : outGoingChequesObj.Party);
                                    outGoingChequesObj.Status = (sdr["Status"].ToString() != "" ? (sdr["Status"].ToString()) : outGoingChequesObj.Status);
                                    outGoingChequesObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : outGoingChequesObj.Remarks);
                                    outGoingChequesObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : outGoingChequesObj.Amount);
                                    outGoingChequesObj.Company = (sdr["FromCompany"].ToString() != "" ? (sdr["FromCompany"].ToString()) : outGoingChequesObj.Company);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return outGoingChequesObj;
        }


        public object ValidateChequeNo(OutGoingCheques outGoingChequeObj)
        {
            AppConst appcust = new AppConst();
            SqlParameter outputStatus = null;
            SqlParameter outputStatus1 = null;
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
                        cmd.CommandText = "[Accounts].[ValidateChequeNo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ChequeNo", SqlDbType.VarChar, 20).Value = outGoingChequeObj.ChequeNo;
                        cmd.Parameters.Add("@Bank", SqlDbType.NVarChar, 50).Value = outGoingChequeObj.Bank;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = outGoingChequeObj.ID;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus1 = cmd.Parameters.Add("@message", SqlDbType.VarChar, 100);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputStatus1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }

            }


            catch (Exception ex)
            {
                return new { Message = ex.ToString(), Status = -1 };
            }

            return new { Message = outputStatus1.Value.ToString(), Status = outputStatus.Value };

        }


    }
}
