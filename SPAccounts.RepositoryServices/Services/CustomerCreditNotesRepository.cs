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
    public class CustomerCreditNotesRepository : ICustomerCreditNotesRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public CustomerCreditNotesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllCustomerCreditNotes
        public List<CustomerCreditNotes> GetAllCustomerCreditNotes()
        {
            List<CustomerCreditNotes> customerCreditNotesList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllCustCreditNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerCreditNotesList = new List<CustomerCreditNotes>();
                                while (sdr.Read())
                                {
                                    CustomerCreditNotes _customerCreditNotesObj = new CustomerCreditNotes();
                                    {
                                        _customerCreditNotesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _customerCreditNotesObj.ID);
                                        _customerCreditNotesObj.CustomerName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _customerCreditNotesObj.CustomerName);
                                        _customerCreditNotesObj.CustomerID = (sdr["CustomerID"].ToString() != "" ?Guid.Parse(sdr["CustomerID"].ToString()) : _customerCreditNotesObj.CustomerID);
                                        _customerCreditNotesObj.OriginComanyCode = (sdr["OriginComanyCode"].ToString() != "" ? sdr["OriginComanyCode"].ToString() : _customerCreditNotesObj.OriginComanyCode);
                                        _customerCreditNotesObj.CreditNoteNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : _customerCreditNotesObj.CreditNoteNo);
                                        _customerCreditNotesObj.CreditNoteDate = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()) : _customerCreditNotesObj.CreditNoteDate);
                                        _customerCreditNotesObj.CreditAmount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _customerCreditNotesObj.CreditAmount);
                                        _customerCreditNotesObj.AvailableCredit = (sdr["AvailableCredit"].ToString() != "" ? decimal.Parse(sdr["AvailableCredit"].ToString()) : _customerCreditNotesObj.AvailableCredit);
                                        //  _customerCreditNotesObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _customerCreditNotesObj.Type);
                                        //  _customerCreditNotesObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _customerCreditNotesObj.GeneralNotes);

                                        _customerCreditNotesObj.CreditNoteDateFormatted= (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()).ToString(s.dateformat) : _customerCreditNotesObj.CreditNoteDateFormatted);

                                    }
                                    customerCreditNotesList.Add(_customerCreditNotesObj);
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

            return customerCreditNotesList;
        }
        #endregion GetAllCustomerCreditNotes

        #region GetCustomerCreditNoteDetails
        public CustomerCreditNotes GetCustomerCreditNoteDetails(Guid ID)
        {
            CustomerCreditNotes _customerCreditNoteObj = new CustomerCreditNotes();
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
                        cmd.CommandText = "[Accounts].[GetCustCreditNotesDetailsByID]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _customerCreditNoteObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _customerCreditNoteObj.ID);
                                    _customerCreditNoteObj.CustomerName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _customerCreditNoteObj.CustomerName);
                                    _customerCreditNoteObj.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : _customerCreditNoteObj.CustomerID);
                                    _customerCreditNoteObj.OriginComanyCode = (sdr["OriginComanyCode"].ToString() != "" ? sdr["OriginComanyCode"].ToString() : _customerCreditNoteObj.OriginComanyCode);
                                    _customerCreditNoteObj.CreditNoteNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : _customerCreditNoteObj.CreditNoteNo);
                                    _customerCreditNoteObj.CreditNoteDate = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()) : _customerCreditNoteObj.CreditNoteDate);
                                    _customerCreditNoteObj.CreditAmount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _customerCreditNoteObj.CreditAmount);
                                    _customerCreditNoteObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _customerCreditNoteObj.Type);
                                    _customerCreditNoteObj.adjustedAmount = (sdr["AdjAmount"].ToString() != "" ? decimal.Parse(sdr["AdjAmount"].ToString()) : _customerCreditNoteObj.adjustedAmount);
                                    _customerCreditNoteObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _customerCreditNoteObj.GeneralNotes);
                                    _customerCreditNoteObj.BillingAddress= (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _customerCreditNoteObj.BillingAddress);
                                    _customerCreditNoteObj.CreditNoteDateFormatted = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()).ToString(s.dateformat) : _customerCreditNoteObj.CreditNoteDateFormatted);
                                    _customerCreditNoteObj.commonObj = new Common();
                                    _customerCreditNoteObj.commonObj.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : _customerCreditNoteObj.commonObj.CreatedBy);
                                    _customerCreditNoteObj.commonObj.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(s.dateformat) : _customerCreditNoteObj.commonObj.CreatedDateString);
                                    _customerCreditNoteObj.commonObj.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : _customerCreditNoteObj.commonObj.CreatedDate);
                                    _customerCreditNoteObj.commonObj.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : _customerCreditNoteObj.commonObj.UpdatedBy);
                                    _customerCreditNoteObj.commonObj.UpdatedDate = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()) : _customerCreditNoteObj.commonObj.UpdatedDate);
                                    _customerCreditNoteObj.commonObj.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(s.dateformat) : _customerCreditNoteObj.commonObj.UpdatedDateString);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _customerCreditNoteObj;
        }
        #endregion GetCustomerCreditNoteDetails

        #region InsertCustomerCreditNotes
        public CustomerCreditNotes InsertCustomerCreditNotes(CustomerCreditNotes _customerCreditNotesObj)
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
                        cmd.CommandText = "[Accounts].[InsertCustCreditNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@OriginComanyCode", SqlDbType.NVarChar, 10).Value = _customerCreditNotesObj.OriginComanyCode;
                        cmd.Parameters.Add("@CRNRefNo", SqlDbType.NVarChar, 20).Value = _customerCreditNotesObj.CreditNoteNo;
                        cmd.Parameters.Add("@CRNDate", SqlDbType.DateTime).Value = _customerCreditNotesObj.CreditNoteDateFormatted;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _customerCreditNotesObj.CreditAmount;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 5).Value = _customerCreditNotesObj.Type;                       
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _customerCreditNotesObj.GeneralNotes;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = _customerCreditNotesObj.CustomerID;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _customerCreditNotesObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _customerCreditNotesObj.commonObj.CreatedDate;
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
                        _customerCreditNotesObj.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _customerCreditNotesObj;
        }
        #endregion InsertCustomerCreditNotes

        #region UpdateCustomerCreditNotes
        public object UpdateCustomerCreditNotes(CustomerCreditNotes _customerCreditNotesObj)
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
                        cmd.CommandText = "[Accounts].[UpdateCustCreditNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _customerCreditNotesObj.ID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = _customerCreditNotesObj.CustomerID;
                        cmd.Parameters.Add("@OriginComanyCode", SqlDbType.NVarChar, 10).Value = _customerCreditNotesObj.OriginComanyCode;
                        cmd.Parameters.Add("@CRNRefNo", SqlDbType.NVarChar, 20).Value = _customerCreditNotesObj.CreditNoteNo;
                        cmd.Parameters.Add("@CRNDate", SqlDbType.DateTime).Value = _customerCreditNotesObj.CreditNoteDateFormatted;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _customerCreditNotesObj.CreditAmount;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 5).Value = _customerCreditNotesObj.Type;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _customerCreditNotesObj.GeneralNotes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _customerCreditNotesObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _customerCreditNotesObj.commonObj.UpdatedDate;
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
        #endregion UpdateCustomerCreditNotes

        #region DeleteCustomerCreditNotes
        public object DeleteCustomerCreditNotes(Guid ID, string userName)
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
                        cmd.CommandText = "[Accounts].[DeleteCustCreditNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar, 250).Value = userName;
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
        #endregion DeleteCustomerCreditNotes

        #region GetCreditNoteByCustomer
        public List<CustomerCreditNotes> GetCreditNoteByCustomer(Guid ID)
        {
            List<CustomerCreditNotes> customerCreditNotesList = null;
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
                        cmd.CommandText = "[Accounts].[GetCreditNotesByCustomer]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerCreditNotesList = new List<CustomerCreditNotes>();
                                while (sdr.Read())
                                {
                                    CustomerCreditNotes _customerCreditNotesObj = new CustomerCreditNotes();
                                    {
                                        _customerCreditNotesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _customerCreditNotesObj.ID);
                                        _customerCreditNotesObj.CreditNoteNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : _customerCreditNotesObj.CreditNoteNo);
                                        _customerCreditNotesObj.CreditNoteDate = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()) : _customerCreditNotesObj.CreditNoteDate);
                                        _customerCreditNotesObj.CreditAmount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _customerCreditNotesObj.CreditAmount);
                                        _customerCreditNotesObj.AvailableCredit = (sdr["AvailableCredit"].ToString() != "" ? decimal.Parse(sdr["AvailableCredit"].ToString()) : _customerCreditNotesObj.AvailableCredit);
                                        _customerCreditNotesObj.CreditNoteDateFormatted = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()).ToString(s.dateformat) : _customerCreditNotesObj.CreditNoteDateFormatted);
                                    }
                                    customerCreditNotesList.Add(_customerCreditNotesObj);
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

            return customerCreditNotesList;
        }
        #endregion GetAllCustomerCreditNotes

        #region GetCreditNoteByCustomer
        public List<CustomerCreditNotes> GetCreditNoteByPaymentID(Guid ID, Guid PaymentID)
        {
            List<CustomerCreditNotes> customerCreditNotesList = null;
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
                        cmd.CommandText = "[Accounts].[GetCreditNotesByPaymentID]";
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerCreditNotesList = new List<CustomerCreditNotes>();
                                while (sdr.Read())
                                {
                                    CustomerCreditNotes _customerCreditNotesObj = new CustomerCreditNotes();
                                    {
                                        _customerCreditNotesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _customerCreditNotesObj.ID);
                                        _customerCreditNotesObj.CreditNoteNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : _customerCreditNotesObj.CreditNoteNo);
                                        _customerCreditNotesObj.CreditNoteDate = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()) : _customerCreditNotesObj.CreditNoteDate);
                                        _customerCreditNotesObj.CreditAmount = (sdr["CreditAmount"].ToString() != "" ? decimal.Parse(sdr["CreditAmount"].ToString()) : _customerCreditNotesObj.CreditAmount);
                                        _customerCreditNotesObj.AvailableCredit = (sdr["AvailableCredit"].ToString() != "" ? decimal.Parse(sdr["AvailableCredit"].ToString()) : _customerCreditNotesObj.AvailableCredit);
                                        _customerCreditNotesObj.CreditNoteDateFormatted = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()).ToString(s.dateformat) : _customerCreditNotesObj.CreditNoteDateFormatted);
                                    }
                                    customerCreditNotesList.Add(_customerCreditNotesObj);
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

            return customerCreditNotesList;
        }
        #endregion GetAllCustomerCreditNotes

    }
}