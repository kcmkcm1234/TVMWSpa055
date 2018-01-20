using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;

namespace SPAccounts.RepositoryServices.Services
{
    public class SupplierPaymentsRepository: ISupplierPaymentsRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SupplierPaymentsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<SupplierPayments> GetAllSupplierPayments(SupplierPaymentsAdvanceSearch supplierPaymentsAdvanceSearch)
        {
            List<SupplierPayments> SupplierPaylist = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = supplierPaymentsAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = supplierPaymentsAdvanceSearch.ToDate;
                        cmd.Parameters.Add("@SupplierCode", SqlDbType.NVarChar, 50).Value = supplierPaymentsAdvanceSearch.Supplier;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 50).Value = supplierPaymentsAdvanceSearch.PaymentMode;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = supplierPaymentsAdvanceSearch.Company;
                        cmd.Parameters.Add("@ApprovalStatus", SqlDbType.NVarChar, 50).Value = supplierPaymentsAdvanceSearch.ApprovalStatus;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = supplierPaymentsAdvanceSearch.Search;
                        cmd.CommandText = "[Accounts].[GetAllSupplierPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierPaylist = new List<SupplierPayments>();
                                while (sdr.Read())
                                {
                                    SupplierPayments PaymentsObj = new SupplierPayments();
                                    {
                                        PaymentsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : PaymentsObj.ID);
                                        PaymentsObj.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.PaymentDateFormatted);
                                        PaymentsObj.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.ChequeDate);
                                        PaymentsObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : PaymentsObj.PaymentRef);
                                        PaymentsObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : PaymentsObj.EntryNo);
                                        PaymentsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : PaymentsObj.PaymentMode);
                                        PaymentsObj.TotalPaidAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()) : PaymentsObj.TotalPaidAmt);
                                        PaymentsObj.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : PaymentsObj.AdvanceAmount);
                                        PaymentsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : PaymentsObj.Type);
                                        PaymentsObj.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : PaymentsObj.CreditNo);
                                        PaymentsObj.GeneralNotes= (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : PaymentsObj.GeneralNotes);
                                        PaymentsObj.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? Int16.Parse(sdr["ApprovalStatus"].ToString()):PaymentsObj.ApprovalStatus);
                                        PaymentsObj.ApprovalStatusObj = new ApprovalStatus();
                                        PaymentsObj.ApprovalStatusObj.Description = (sdr["StatusDesc"].ToString() != "" ? sdr["StatusDesc"].ToString() : PaymentsObj.ApprovalStatusObj.Description);
                                        PaymentsObj.supplierObj = new Supplier();
                                        PaymentsObj.supplierObj.CompanyName = (sdr["Supplier"].ToString() != "" ? sdr["Supplier"].ToString() : PaymentsObj.supplierObj.CompanyName);
                                        PaymentsObj.supplierObj.ID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : PaymentsObj.supplierObj.ID);
                                        PaymentsObj.supplierObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : PaymentsObj.supplierObj.ContactPerson);
                                        PaymentsObj.CompanyObj = new Companies();
                                        PaymentsObj.CompanyObj.Name = sdr["PaidFrom"].ToString();
                                        PaymentsObj.IsNotificationSuccess = sdr["IsNotificationSuccess"].ToString();
                                    };
                                    SupplierPaylist.Add(PaymentsObj);
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
            return SupplierPaylist;
        }

        public List<SupplierPayments> GetAllPendingSupplierPayments()
        {
            List<SupplierPayments> SupplierPendinglist = null;
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
                        cmd.CommandText = "[Accounts].[GetAllPendingSupplierPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierPendinglist = new List<SupplierPayments>();
                                while (sdr.Read())
                                {
                                    SupplierPayments PaymentsObj = new SupplierPayments();
                                    {
                                        PaymentsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : PaymentsObj.ID);
                                        PaymentsObj.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.PaymentDateFormatted);
                                        PaymentsObj.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.ChequeDate);
                                        PaymentsObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : PaymentsObj.PaymentRef);
                                        PaymentsObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : PaymentsObj.EntryNo);
                                        PaymentsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : PaymentsObj.PaymentMode);
                                        PaymentsObj.TotalPaidAmt = (sdr["Amount"].ToString() != "" ? Decimal.Parse(sdr["Amount"].ToString()) : PaymentsObj.TotalPaidAmt);
                                        PaymentsObj.Amount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : PaymentsObj.Amount);
                                        PaymentsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : PaymentsObj.Type);
                                        PaymentsObj.CreditID = (sdr["CreditID"].ToString() != "" ?Guid.Parse(sdr["CreditID"].ToString()) : PaymentsObj.CreditID);
                                        PaymentsObj.supplierObj = new Supplier();
                                        PaymentsObj.supplierObj.CompanyName = (sdr["Supplier"].ToString() != "" ? sdr["Supplier"].ToString() : PaymentsObj.supplierObj.CompanyName);
                                        PaymentsObj.supplierObj.ID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : PaymentsObj.supplierObj.ID);
                                        PaymentsObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : PaymentsObj.GeneralNotes);
                                        PaymentsObj.PaidFromComanyCode = (sdr["PaidFromComanyCode"].ToString() != "" ? sdr["PaidFromComanyCode"].ToString() : PaymentsObj.PaidFromComanyCode);
                                        PaymentsObj.DepWithdID = (sdr["DepWithdID"].ToString() != "" ? Guid.Parse(sdr["DepWithdID"].ToString()) : PaymentsObj.DepWithdID);
                                        PaymentsObj.BankCode=(sdr["BankCode"].ToString()!=""?sdr["BankCode"].ToString() : PaymentsObj.BankCode);
                                        PaymentsObj.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? int.Parse(sdr["ApprovalStatus"].ToString()) : PaymentsObj.ApprovalStatus);
                                        PaymentsObj.ApprovalDate = (sdr["ApprovalDate"].ToString() != "" ? sdr["ApprovalDate"].ToString() : PaymentsObj.ApprovalDate);
                                       

                                    };
                                    SupplierPendinglist.Add(PaymentsObj);
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
            return SupplierPendinglist;
        }

        public SupplierPayments GetSupplierPaymentsByID(string ID)
        {
            SupplierPayments PaymentsObj = new SupplierPayments();
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
                        cmd.CommandText = "[Accounts].[GetSupplierPaymentsDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    PaymentsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : PaymentsObj.ID);
                                    PaymentsObj.PaidFromComanyCode = (sdr["PaidFromComanyCode"].ToString() != "" ? sdr["PaidFromComanyCode"].ToString() : PaymentsObj.PaidFromComanyCode);
                                    PaymentsObj.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.PaymentDateFormatted);
                                    PaymentsObj.ChequeClearDate = (sdr["ChequeClearDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeClearDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.ChequeClearDate);
                                    PaymentsObj.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.ChequeDate);
                                    PaymentsObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : PaymentsObj.PaymentRef);
                                    PaymentsObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : PaymentsObj.EntryNo);
                                    PaymentsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : PaymentsObj.PaymentMode);
                                    PaymentsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : PaymentsObj.Type);
                                    PaymentsObj.ReferenceBank = (sdr["ReferenceBank"].ToString() != "" ? sdr["ReferenceBank"].ToString() : PaymentsObj.ReferenceBank);
                                    PaymentsObj.CreditID = (sdr["CreditID"].ToString() != "" ? Guid.Parse(sdr["CreditID"].ToString()) : PaymentsObj.CreditID);
                                    PaymentsObj.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : PaymentsObj.CreditNo);
                                    PaymentsObj.TotalPaidAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()) : PaymentsObj.TotalPaidAmt);
                                    PaymentsObj.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : PaymentsObj.AdvanceAmount);
                                    PaymentsObj.BankCode = (sdr["BankCode"].ToString() != "" ? sdr["BankCode"].ToString() : PaymentsObj.BankCode);
                                    PaymentsObj.DepWithdID = (sdr["DepWithdID"].ToString() != "" ? Guid.Parse(sdr["DepWithdID"].ToString()) : PaymentsObj.DepWithdID);
                                    PaymentsObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : PaymentsObj.GeneralNotes);
                                    PaymentsObj.supplierObj = new Supplier();
                                    PaymentsObj.supplierObj.ID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : PaymentsObj.supplierObj.ID);
                                    PaymentsObj.ApprovalStatus=(sdr["ApprovalStatus"].ToString() != "" ? Int32.Parse(sdr["ApprovalStatus"].ToString()) : PaymentsObj.ApprovalStatus);
                                    PaymentsObj.ApprovalDate = (sdr["ApprovalDate"].ToString() != "" ? DateTime.Parse(sdr["ApprovalDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.ApprovalDate);
                                    PaymentsObj.IsNotificationSuccess = (sdr["IsNotificationSuccess"].ToString() != "" ? sdr["IsNotificationSuccess"].ToString() : PaymentsObj.IsNotificationSuccess);

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
            return PaymentsObj;

        }
        public List<SupplierPayments> GetSupplierInvoiceAdjustedByPaymentID(SupplierPayments SupplierObj)
        {
            List<SupplierPayments> PaymentsList = null;
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = SupplierObj.ID;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = SupplierObj.Date;
                        cmd.CommandText = "[Accounts].[GetAllSupplierInvoiceAdjustedByPaymentID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            PaymentsList = new List<SupplierPayments>();
                            while (sdr.Read())
                            {
                                SupplierPayments PaymentsObj = new SupplierPayments();
                                {
                                    PaymentsObj.ID = (sdr["InvoiceID"].ToString() != "" ? Guid.Parse(sdr["InvoiceID"].ToString()) : PaymentsObj.ID);
                                    PaymentsObj.supplierPaymentsDetailObj = new SupplierPaymentsDetail();
                                    PaymentsObj.supplierPaymentsDetailObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : PaymentsObj.supplierPaymentsDetailObj.InvoiceNo);
                                    PaymentsObj.supplierPaymentsDetailObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.supplierPaymentsDetailObj.InvoiceDate);
                                    PaymentsObj.supplierPaymentsDetailObj.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : PaymentsObj.supplierPaymentsDetailObj.InvoiceAmount);
                                    PaymentsObj.supplierPaymentsDetailObj.DueDays = (sdr["DueDays"].ToString() != "" ? sdr["DueDays"].ToString() : PaymentsObj.supplierPaymentsDetailObj.DueDays);
                                    PaymentsObj.supplierPaymentsDetailObj.PrevPayment = (sdr["PrevPayment"].ToString() != "" ? decimal.Parse(sdr["PrevPayment"].ToString()) : PaymentsObj.supplierPaymentsDetailObj.PrevPayment);
                                    PaymentsObj.supplierPaymentsDetailObj.CurrPayment = (sdr["CurrPayment"].ToString() != "" ?decimal.Parse( sdr["CurrPayment"].ToString()) : PaymentsObj.supplierPaymentsDetailObj.CurrPayment);
                                    PaymentsObj.supplierPaymentsDetailObj.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.supplierPaymentsDetailObj.PaymentDueDate);
                                    PaymentsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : PaymentsObj.Type);
                                    PaymentsObj.supplierPaymentsDetailObj.BalancePayment = (sdr["BalancePayment"].ToString() != "" ? decimal.Parse(sdr["BalancePayment"].ToString()) : PaymentsObj.supplierPaymentsDetailObj.BalancePayment);
                                    PaymentsObj.CompanyObj = new Companies();
                                    PaymentsObj.CompanyObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : PaymentsObj.CompanyObj.Name);
                                }
                                PaymentsList.Add(PaymentsObj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PaymentsList;

        }

        public SupplierPayments ApprovedSupplierPayment(SupplierPayments SupplierObj)
        {
            SupplierPayments PaymentsObj = new SupplierPayments();
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = SupplierObj.ID;
                        cmd.Parameters.Add("@ApprovalDate", SqlDbType.DateTime).Value = SupplierObj.ApprovalDate;
                        cmd.CommandText = "[Accounts].[ApproveSupplierPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        Object count = cmd.ExecuteScalar();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PaymentsObj;
        }
        public SupplierPayments InsertSupplierPayments(SupplierPayments _supplierPayObj)
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
                        cmd.CommandText = "[Accounts].[InsertSupplierPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.supplierObj.ID;
                        cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = _supplierPayObj.PaymentDate;
                        cmd.Parameters.Add("@ChequeClearDate", SqlDbType.DateTime).Value = _supplierPayObj.ChequeClearDate;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = _supplierPayObj.ChequeDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaymentMode;
                        cmd.Parameters.Add("@Refbank", SqlDbType.NVarChar, 50).Value = _supplierPayObj.ReferenceBank;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = _supplierPayObj.BankCode;
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.VarChar, 20).Value = _supplierPayObj.PaymentRef;
                        cmd.Parameters.Add("@PaidFromComanyCode", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaidFromComanyCode;
                        cmd.Parameters.Add("@TotalPaidAmt", SqlDbType.Decimal).Value = _supplierPayObj.TotalPaidAmt;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = _supplierPayObj.AdvanceAmount;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 1).Value = _supplierPayObj.Type;
                        cmd.Parameters.Add("@CreditID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.CreditID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierPayObj.GeneralNotes;
                        cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = _supplierPayObj.DetailXml;
                        cmd.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = _supplierPayObj.ApprovalStatus;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _supplierPayObj.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _supplierPayObj.CommonObj.CreatedDate;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.hdnFileID;
                        //cmd.Parameters.Add("@IsNotioficationSuccess", SqlDbType.Int).Value = _supplierPayObj.IsNotificationSuccess;
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
                        _supplierPayObj.ID = new Guid(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierPayObj;  
        }

        public SupplierPayments UpdateSupplierPayments(SupplierPayments _supplierPayObj)
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
                        cmd.CommandText = "[Accounts].[UpdateSupplierPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.ID;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(_supplierPayObj.hdfSupplierID);
                        cmd.Parameters.Add("@ChequeClearDate", SqlDbType.DateTime).Value = _supplierPayObj.ChequeClearDate;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = _supplierPayObj.ChequeDate;
                        cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = _supplierPayObj.PaymentDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaymentMode;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 1).Value = _supplierPayObj.hdfType;
                        cmd.Parameters.Add("@Refbank", SqlDbType.NVarChar, 50).Value = _supplierPayObj.ReferenceBank;
                        cmd.Parameters.Add("@CreditID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.hdfCreditID;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = _supplierPayObj.BankCode;
                        cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.DepWithdID;
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.VarChar, 20).Value = _supplierPayObj.PaymentRef;
                        cmd.Parameters.Add("@PaidFromComanyCode", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaidFromComanyCode;
                        cmd.Parameters.Add("@TotalPaidAmt", SqlDbType.Decimal).Value = _supplierPayObj.TotalPaidAmt;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = _supplierPayObj.AdvanceAmount;
                        cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = _supplierPayObj.DetailXml;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierPayObj.GeneralNotes;
                        cmd.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = _supplierPayObj.ApprovalStatus;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _supplierPayObj.CommonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _supplierPayObj.CommonObj.UpdatedDate;
                        cmd.Parameters.Add("@IsNotificationSuccess", SqlDbType.Int).Value = _supplierPayObj.IsNotificationSuccess;
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
            return _supplierPayObj;

        }

        public SupplierPayments InsertPaymentAdjustment(SupplierPayments _supplierPayObj)
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
                        cmd.CommandText = "[Accounts].[SupplierAdvanceAdjustment]";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.supplierObj.ID;
                        cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = _supplierPayObj.DetailXml;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _supplierPayObj.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _supplierPayObj.CommonObj.CreatedDate;
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
            return _supplierPayObj;

        }

        public object DeletePayments(Guid PaymentID, string UserName)
        {
            AppConst Cobj = new AppConst();
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
                        cmd.CommandText = "[Accounts].[DeleteSupplierPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentID;
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20).Value = UserName;
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
            return new { Message = Cobj.DeleteSuccess };

        }

        public SupplierPayments GetOutstandingAmountBySupplier(string SupplierID)
        {
            SupplierPayments PaymentsObj = new SupplierPayments();
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
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(SupplierID);
                        cmd.CommandText = "[Accounts].[GetOutstandingAmountBySupplier]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    PaymentsObj.OutstandingAmount = (sdr["OutstandingAmount"].ToString() != "" ? sdr["OutstandingAmount"].ToString() : PaymentsObj.OutstandingAmount);
                                    PaymentsObj.PaymentOutstanding = (sdr["PaymentOutstanding"].ToString() != "" ? sdr["PaymentOutstanding"].ToString() : PaymentsObj.PaymentOutstanding);
                                    PaymentsObj.InvoiceOutstanding = (sdr["InvoiceOutstanding"].ToString() != "" ? sdr["InvoiceOutstanding"].ToString() : PaymentsObj.InvoiceOutstanding);
                                    PaymentsObj.CreditOutstanding = (sdr["CreditOutstanding"].ToString() != "" ? sdr["CreditOutstanding"].ToString() : PaymentsObj.CreditOutstanding);
                                    PaymentsObj.AdvOutstanding = (sdr["AdvOutstanding"].ToString() != "" ? sdr["AdvOutstanding"].ToString() : PaymentsObj.AdvOutstanding);
                                    PaymentsObj.PaymentBooked= (sdr["PaymentBooked"].ToString() != "" ? sdr["PaymentBooked"].ToString() : PaymentsObj.PaymentBooked);
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
            return PaymentsObj;

        }

        public object ApprovedPayment(Guid PaymentID, string UserName, DateTime date)
        {
            AppConst Cobj = new AppConst();
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
                        cmd.CommandText = "[Accounts].[SupplierApprovedPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentID;
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20).Value = UserName;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = date;
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
            return new { Message = Cobj.InsertSuccess };
        }

        public object Validate(SupplierPayments _supplierpayObj)
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
                        cmd.CommandText = "[Accounts].[ValidateSupplierPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = _supplierpayObj.PaymentRef;
                        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = _supplierpayObj.ID;
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

        public SupplierPayments UpdateSupplierPaymentGeneralNotes(SupplierPayments supobj)
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
                        cmd.CommandText = "[Accounts].[UpdateSupplierPaymentGeneralNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = supobj.ID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = supobj.GeneralNotes;
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
            return supobj;

        }


        public object UpdateNotification(SupplierPayments _supplierpayObj)
        {
           
            SqlParameter outputStatus = null;
            try
            {
               // _supplierpayObj = new SupplierPayments();
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }                       
                       
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[UpdateNotificationStatus]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _supplierpayObj.ID;                       
                        cmd.Parameters.Add("@IsNotificationSuccess", SqlDbType.Int).Value = 1;                       
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                AppConst Cobj = new AppConst();
                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(Cobj.UpdateFailure);

                    case "1":

                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = Cobj.UpdateSuccess
                        };
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
            };
        }




    }
}