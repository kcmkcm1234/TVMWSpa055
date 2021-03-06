﻿using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;

namespace SPAccounts.RepositoryServices.Services
{
    public class CustomerPaymentsRepository:ICustomerPaymentsRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CustomerPaymentsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<CustomerPayments> GetAllCustomerPayments(CustomerPaymentsSearch customerPaymentsSearch)
        {
            List<CustomerPayments> ExpenseTypelist = null;
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
                        cmd.CommandText = "[Accounts].[GetAllCustomerPayments]";
                        //DateTime? FromDate = string.IsNullOrEmpty(customerPaymentsSearch.FromDate) ? (DateTime?)null : DateTime.Parse();
                        //DateTime? ToDate = string.IsNullOrEmpty(customerPaymentsSearch.ToDate) ? (DateTime?)null : DateTime.Parse();
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = customerPaymentsSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = customerPaymentsSearch.ToDate;
                        cmd.Parameters.Add("@CustomerCode", SqlDbType.NVarChar, 50).Value = customerPaymentsSearch.Customer;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 50).Value = customerPaymentsSearch.PaymentMode;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = customerPaymentsSearch.Company;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = customerPaymentsSearch.Search;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ExpenseTypelist = new List<CustomerPayments>();
                                while (sdr.Read())
                                {
                                    CustomerPayments CustPaymentsObj = new CustomerPayments();

                                    {
                                        CustPaymentsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : CustPaymentsObj.ID);
                                        CustPaymentsObj.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : CustPaymentsObj.PaymentDateFormatted);
                                        CustPaymentsObj.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : CustPaymentsObj.ChequeDate);
                                        CustPaymentsObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : CustPaymentsObj.PaymentRef);
                                        CustPaymentsObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : CustPaymentsObj.EntryNo);
                                        CustPaymentsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : CustPaymentsObj.PaymentMode);
                                        CustPaymentsObj.TotalRecdAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()): CustPaymentsObj.TotalRecdAmt);
                                        CustPaymentsObj.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : CustPaymentsObj.AdvanceAmount);
                                        CustPaymentsObj.CompanyObj = new Companies();
                                        CustPaymentsObj.CompanyObj.Name = sdr["ReceivedTo"].ToString();
                                        CustPaymentsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : CustPaymentsObj.Type);
                                        CustPaymentsObj.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : CustPaymentsObj.CreditNo);
                                        CustPaymentsObj.customerObj = new Customer();
                                        CustPaymentsObj.customerObj.CompanyName = (sdr["Customer"].ToString() != "" ? sdr["Customer"].ToString() : CustPaymentsObj.customerObj.CompanyName);
                                        CustPaymentsObj.customerObj.ID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : CustPaymentsObj.customerObj.ID);
                                        CustPaymentsObj.customerObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : CustPaymentsObj.customerObj.ContactPerson);

                                    };

                                    ExpenseTypelist.Add(CustPaymentsObj);
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
            return ExpenseTypelist;
        }

        public CustomerPayments GetCustomerPaymentsByID(string ID)
        {
            CustomerPayments CustPaymentsObj = new CustomerPayments();
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.CommandText = "[Accounts].[GetCustomerPaymentsDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    CustPaymentsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : CustPaymentsObj.ID);
                                    CustPaymentsObj.RecdToComanyCode = (sdr["RecdToComanyCode"].ToString() != "" ? sdr["RecdToComanyCode"].ToString() : CustPaymentsObj.RecdToComanyCode);
                                    CustPaymentsObj.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : CustPaymentsObj.PaymentDateFormatted);
                                    CustPaymentsObj.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : CustPaymentsObj.ChequeDate);
                                    CustPaymentsObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : CustPaymentsObj.PaymentRef);
                                    CustPaymentsObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : CustPaymentsObj.EntryNo);
                                    CustPaymentsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : CustPaymentsObj.PaymentMode);
                                    CustPaymentsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : CustPaymentsObj.Type);
                                    CustPaymentsObj.ReferenceBank = (sdr["ReferenceBank"].ToString() != "" ? sdr["ReferenceBank"].ToString() : CustPaymentsObj.ReferenceBank);
                                    CustPaymentsObj.CreditID = (sdr["CreditID"].ToString() != "" ? Guid.Parse(sdr["CreditID"].ToString()) : CustPaymentsObj.CreditID);
                                    CustPaymentsObj.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : CustPaymentsObj.CreditNo);
                                    CustPaymentsObj.TotalRecdAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()) : CustPaymentsObj.TotalRecdAmt);
                                    CustPaymentsObj.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : CustPaymentsObj.AdvanceAmount);
                                    CustPaymentsObj.BankCode = (sdr["BankCode"].ToString() != "" ? sdr["BankCode"].ToString() : CustPaymentsObj.BankCode);
                                    CustPaymentsObj.DepWithdID = (sdr["DepWithdID"].ToString() != "" ? Guid.Parse(sdr["DepWithdID"].ToString()) : CustPaymentsObj.DepWithdID);
                                    CustPaymentsObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : CustPaymentsObj.GeneralNotes);
                                    CustPaymentsObj.customerObj = new Customer();
                                    CustPaymentsObj.customerObj.ID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : CustPaymentsObj.customerObj.ID);
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
            return CustPaymentsObj;

        }

        public CustomerPayments InsertCustomerPayments(CustomerPayments _custPayObj)
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
                        cmd.CommandText = "[Accounts].[InsertCustomerPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = _custPayObj.customerObj.ID;
                        cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = _custPayObj.PaymentDate;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = _custPayObj.ChequeDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = _custPayObj.PaymentMode;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = _custPayObj.BankCode;
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.VarChar, 10).Value = _custPayObj.PaymentRef;
                        cmd.Parameters.Add("@Refbank", SqlDbType.NVarChar, 50).Value = _custPayObj.ReferenceBank;
                        cmd.Parameters.Add("@RecdToComanyCode", SqlDbType.VarChar, 10).Value = _custPayObj.RecdToComanyCode;
                        cmd.Parameters.Add("@TotalRecdAmt", SqlDbType.Decimal).Value = _custPayObj.TotalRecdAmt;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = _custPayObj.AdvanceAmount;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 1).Value = _custPayObj.Type;
                        cmd.Parameters.Add("@CreditID", SqlDbType.UniqueIdentifier).Value = _custPayObj.CreditID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _custPayObj.GeneralNotes;
                        cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = _custPayObj.DetailXml;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _custPayObj.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _custPayObj.CommonObj.CreatedDate;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = _custPayObj.hdnFileID;
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
                        _custPayObj.ID = new Guid(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _custPayObj;

        }

        public CustomerPayments UpdateCustomerPayments(CustomerPayments _custPayObj)
        {
            try
            {
                SqlParameter outputStatus = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[UpdateCustomerPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _custPayObj.ID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(_custPayObj.hdfCustomerID);
                        cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = _custPayObj.PaymentDate;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = _custPayObj.ChequeDate; 
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = _custPayObj.PaymentMode;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 1).Value = _custPayObj.hdfType;
                        cmd.Parameters.Add("@CreditID", SqlDbType.UniqueIdentifier).Value = _custPayObj.hdfCreditID;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = _custPayObj.BankCode;
                        cmd.Parameters.Add("@Refbank", SqlDbType.NVarChar, 50).Value = _custPayObj.ReferenceBank;
                        cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = _custPayObj.DepWithdID;
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.VarChar, 10).Value = _custPayObj.PaymentRef;
                        cmd.Parameters.Add("@RecdToComanyCode", SqlDbType.VarChar, 10).Value = _custPayObj.RecdToComanyCode;
                        cmd.Parameters.Add("@TotalRecdAmt", SqlDbType.Decimal).Value = _custPayObj.TotalRecdAmt;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = _custPayObj.AdvanceAmount;
                        cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = _custPayObj.DetailXml;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _custPayObj.GeneralNotes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _custPayObj.CommonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _custPayObj.CommonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.UpdateFailure);
                    case "1":
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _custPayObj;
        }

        public object DeletePayments(Guid PaymentId,string UserName)
        {

            AppConst Cobj = new AppConst();
            try
            {
                SqlParameter outputStatus= null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[DeleteCustomerPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentId;
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar,20).Value =UserName;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(Cobj.InsertFailure);
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new   {  Message = Cobj.DeleteSuccess   };
        }

        public object Validate(CustomerPayments _customerpayObj)
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
                        cmd.CommandText = "[Accounts].[ValidateCustomerPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = _customerpayObj.PaymentRef;
                        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = _customerpayObj.ID;
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


        public CustomerPayments InsertPaymentAdjustment(CustomerPayments _custPayObj)
        {
            try
            {
                SqlParameter outputStatus = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[AdvanceAdjustment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = _custPayObj.customerObj.ID;
                        cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = _custPayObj.DetailXml;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _custPayObj.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _custPayObj.CommonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _custPayObj;

        }

        public CustomerPayments GetOutstandingAmountByCustomer(string CustomerID)
        {
            CustomerPayments CustPaymentsObj = new CustomerPayments();
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
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(CustomerID);
                        cmd.CommandText = "[Accounts].[GetOutstandingAmountByCustomer]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    CustPaymentsObj.OutstandingAmount = (sdr["OutstandingAmount"].ToString() != "" ?sdr["OutstandingAmount"].ToString() : CustPaymentsObj.OutstandingAmount);
                                    CustPaymentsObj.PaymentOutstanding = (sdr["PaymentOutstanding"].ToString() != "" ? sdr["PaymentOutstanding"].ToString() : CustPaymentsObj.PaymentOutstanding);
                                    CustPaymentsObj.InvoiceOutstanding = (sdr["InvoiceOutstanding"].ToString() != "" ? sdr["InvoiceOutstanding"].ToString() : CustPaymentsObj.InvoiceOutstanding);
                                    CustPaymentsObj.CreditOutstanding = (sdr["CreditOutstanding"].ToString() != "" ? sdr["CreditOutstanding"].ToString() : CustPaymentsObj.CreditOutstanding);
                                    CustPaymentsObj.AdvOutstanding = (sdr["AdvOutstanding"].ToString() != "" ? sdr["AdvOutstanding"].ToString() : CustPaymentsObj.AdvOutstanding);

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
            return CustPaymentsObj;
        }
    }
}