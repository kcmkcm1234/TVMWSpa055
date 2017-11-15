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
    public class SupplierInvoicesRepository: ISupplierInvoicesRepository
    {
        AppConst Cobj = new AppConst();
        private IDatabaseFactory _databaseFactory;
        
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>

        public SupplierInvoicesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<SupplierInvoices> GetAllSupplierInvoices(DateTime? FromDate, DateTime? ToDate, string Supplier, string InvoiceType, string Company, string Status, string Search)
        {
            List<SupplierInvoices> SupplierInvoicesList = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[Accounts].[GetAllSupplierInvoices]";
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@SupplierCode", SqlDbType.NVarChar, 50).Value = Supplier;
                        cmd.Parameters.Add("@InvoiceType", SqlDbType.NVarChar, 50).Value = InvoiceType;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = Company;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = Search;
                        cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = Status;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoicesList = new List<SupplierInvoices>();
                                while (sdr.Read())
                                {
                                    SupplierInvoices SIList = new SupplierInvoices();
                                    {
                                        SIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : SIList.ID);
                                        SIList.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : SIList.InvoiceDate);
                                        SIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        SIList.InvoiceType = sdr["InvoiceType"].ToString();
                                        SIList.companiesObj = new Companies();
                                        SIList.companiesObj.Name= sdr["OrginCompany"].ToString();
                                        SIList.suppliersObj = new Supplier();
                                        SIList.suppliersObj.ID = Guid.Parse(sdr["SupplierID"].ToString());
                                        SIList.suppliersObj.CompanyName = sdr["CompanyName"].ToString();
                                        SIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : SIList.PaymentDueDate);
                                        SIList.TotalInvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : SIList.TotalInvoiceAmount);
                                        SIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : SIList.BalanceDue);
                                        SIList.PaidAmount = (sdr["PaidAmount"].ToString() != "" ? Decimal.Parse(sdr["PaidAmount"].ToString()) : SIList.PaidAmount);
                                        
                                        SIList.PaymentProcessed = (sdr["PaymentProcessed"].ToString() != "" ? Decimal.Parse(sdr["PaymentProcessed"].ToString()) : SIList.PaymentProcessed);
                                        SIList.LastPaymentDate = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()) : SIList.LastPaymentDate);
                                        SIList.Status = sdr["Status"].ToString();

                                        //------------date formatting-----------------//
                                        SIList.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.dateformat) : SIList.InvoiceDateFormatted);
                                        SIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : SIList.PaymentDueDateFormatted);
                                        SIList.LastPaymentDateFormatted = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()).ToString(settings.dateformat) : SIList.LastPaymentDateFormatted);


                                    }
                                    SupplierInvoicesList.Add(SIList);
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

            return SupplierInvoicesList;
        }

        public SupplierInvoiceSummary GetSupplierInvoicesSummary(bool IsInternal)
        {
            SupplierInvoiceSummary SupplierInvoiceSummaryObj = null;

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
                        cmd.CommandText = "[Accounts].[GetSupplierInvoiceSummary]";
                        cmd.Parameters.Add("@IsInternal", SqlDbType.Bit).Value = IsInternal;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoiceSummaryObj = new SupplierInvoiceSummary();
                                if (sdr.Read())
                                {

                                    {
                                        SupplierInvoiceSummaryObj.OverdueAmount = (sdr["Overdueamt"].ToString() != "" ? Decimal.Parse(sdr["Overdueamt"].ToString()) : SupplierInvoiceSummaryObj.OverdueAmount);
                                        SupplierInvoiceSummaryObj.OverdueInvoices = (sdr["OverdueInvoices"].ToString() != "" ? int.Parse(sdr["OverdueInvoices"].ToString()) : SupplierInvoiceSummaryObj.OverdueInvoices);

                                        SupplierInvoiceSummaryObj.OpenAmount = (sdr["Openamt"].ToString() != "" ? Decimal.Parse(sdr["Openamt"].ToString()) : SupplierInvoiceSummaryObj.OpenAmount);
                                        SupplierInvoiceSummaryObj.OpenInvoices = (sdr["OpenInvoices"].ToString() != "" ? int.Parse(sdr["OpenInvoices"].ToString()) : SupplierInvoiceSummaryObj.OpenInvoices);

                                        SupplierInvoiceSummaryObj.PaidAmount = (sdr["Paidamt"].ToString() != "" ? Decimal.Parse(sdr["Paidamt"].ToString()) : SupplierInvoiceSummaryObj.PaidAmount);
                                        SupplierInvoiceSummaryObj.PaidInvoices = (sdr["PaidInvoices"].ToString() != "" ? int.Parse(sdr["PaidInvoices"].ToString()) : SupplierInvoiceSummaryObj.PaidInvoices);

                                        SupplierInvoiceSummaryObj.Approved = (sdr["Approved"].ToString() != "" ? sdr["Approved"].ToString() : SupplierInvoiceSummaryObj.Approved);
                                        SupplierInvoiceSummaryObj.NotApproved = (sdr["NotApproved"].ToString() != "" ? sdr["NotApproved"].ToString() : SupplierInvoiceSummaryObj.NotApproved);
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

            return SupplierInvoiceSummaryObj;
        }

        public SupplierInvoices GetSupplierInvoiceDetails(Guid ID)
        {
            SupplierInvoices SIList = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[Accounts].[GetSupplierInvoiceDetails]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    SIList = new SupplierInvoices();
                                    SIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : SIList.ID);
                                    SIList.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : SIList.InvoiceDate);
                                    SIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                    SIList.InvoiceType = sdr["InvoiceType"].ToString();

                                    SIList.companiesObj = new Companies();
                                    SIList.companiesObj.Code = (sdr["InvoiceToComanyCode"].ToString());
                                    SIList.paymentTermsObj = new PaymentTerms();
                                    SIList.paymentTermsObj.Code = (sdr["PaymentTerm"].ToString());
                                    SIList.suppliersObj = new Supplier();
                                    SIList.suppliersObj.ID = Guid.Parse(sdr["SupplierID"].ToString());
                                    SIList.suppliersObj.CompanyName = sdr["CompanyName"].ToString();
                                    SIList.BillingAddress = (sdr["BillingAddress"].ToString());
                                    SIList.GrossAmount = (sdr["GrossAmount"].ToString() != "" ? Decimal.Parse(sdr["GrossAmount"].ToString()) : SIList.GrossAmount);
                                    SIList.Discount = (sdr["Discount"].ToString() != "" ? Decimal.Parse(sdr["Discount"].ToString()) : SIList.Discount);
                                    SIList.TaxAmount = (sdr["TaxAmount"].ToString() != "" ? Decimal.Parse(sdr["TaxAmount"].ToString()) : SIList.TaxAmount);
                                    SIList.ShippingCharge = (sdr["ShippingCharge"].ToString() != "" ? Decimal.Parse(sdr["ShippingCharge"].ToString()) : SIList.ShippingCharge);
                                    SIList.TaxPercApplied = (sdr["TaxPercApplied"].ToString() != "" ? Decimal.Parse(sdr["TaxPercApplied"].ToString()) : SIList.TaxPercApplied);
                                    SIList.Notes = (sdr["GeneralNotes"].ToString() != "" ? (sdr["GeneralNotes"].ToString()) : SIList.Notes);
                                    SIList.TaxTypeObj = new TaxTypes();
                                    SIList.TaxTypeObj.Code = sdr["TaxTypeCode"].ToString();
                                    SIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : SIList.PaymentDueDate);
                                    SIList.TotalInvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : SIList.TotalInvoiceAmount);
                                    SIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : SIList.BalanceDue);
                                    SIList.LastPaymentDate = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()) : SIList.LastPaymentDate);
                                    SIList.PaymentProcessed = (sdr["PaymentProcessed"].ToString() != "" ? Decimal.Parse(sdr["PaymentProcessed"].ToString()) : SIList.PaymentProcessed);
                                    
                                    //------------date formatting-----------------//
                                    SIList.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.dateformat) : SIList.InvoiceDateFormatted);
                                    SIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : SIList.PaymentDueDateFormatted);
                                    SIList.LastPaymentDateFormatted = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()).ToString(settings.dateformat) : SIList.LastPaymentDateFormatted);

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

            return SIList;
        }

        public SupplierInvoices InsertInvoice(SupplierInvoices _supplierInvoicesObj)
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
                        cmd.CommandText = "[Accounts].[InsertSupplierInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.CompanyCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 20).Value = _supplierInvoicesObj.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceType", SqlDbType.VarChar, 2).Value = _supplierInvoicesObj.InvoiceType;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierInvoicesObj.SupplierID;
                        cmd.Parameters.Add("@PaymentTerm", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.PayCode;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.InvoiceDateFormatted;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = _supplierInvoicesObj.BillingAddress;
                        cmd.Parameters.Add("@GrossAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.GrossAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = _supplierInvoicesObj.Discount;
                        cmd.Parameters.Add("@TaxTypeCode", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.TaxCode != "" ? _supplierInvoicesObj.TaxCode : null;
                        cmd.Parameters.Add("@TaxPreApplied", SqlDbType.Decimal).Value = _supplierInvoicesObj.TaxPercApplied;
                        cmd.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.TaxAmount;                       
                        cmd.Parameters.Add("@ShippingCharge", SqlDbType.Decimal).Value = _supplierInvoicesObj.ShippingCharge;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierInvoicesObj.Notes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _supplierInvoicesObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.commonObj.CreatedDate;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = _supplierInvoicesObj.hdnFileID;
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
                        _supplierInvoicesObj.ID = new Guid(outputID.Value.ToString());
                        break;
                    case "2":
                        AppConst Cobj1 = new AppConst();
                        throw new Exception(Cobj1.Duplicate);
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierInvoicesObj;
        }

        public SupplierInvoices UpdateInvoice(SupplierInvoices _supplierInvoicesObj)
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
                        cmd.CommandText = "[Accounts].[UpdateSupplierInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _supplierInvoicesObj.ID;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.CompanyCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 20).Value = _supplierInvoicesObj.InvoiceNo;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierInvoicesObj.SupplierID;
                        cmd.Parameters.Add("@PaymentTerm", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.PayCode;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.InvoiceDateFormatted;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = _supplierInvoicesObj.BillingAddress;
                        cmd.Parameters.Add("@GrossAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.GrossAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = _supplierInvoicesObj.Discount;
                        cmd.Parameters.Add("@TaxTypeCode", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.TaxCode != "" ? _supplierInvoicesObj.TaxCode : null;
                        cmd.Parameters.Add("@TaxPreApplied", SqlDbType.Decimal).Value = _supplierInvoicesObj.TaxPercApplied;
                        cmd.Parameters.Add("@ShippingCharge", SqlDbType.Decimal).Value = _supplierInvoicesObj.ShippingCharge;
                        cmd.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.TaxAmount;                       
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierInvoicesObj.Notes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _supplierInvoicesObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.commonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        //outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        //outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        // _customerInvoicesObj.ID = new Guid(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierInvoicesObj;
        }

        public List<SupplierInvoices> GetOutstandingSupplierInvoices(SupplierInvoices SupplierInvoiceObj)
        {
            List<SupplierInvoices> SupplierInvoicesList = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[Accounts].[GetAllSupplierOutStandingInvoices]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (SupplierInvoiceObj != null)
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = SupplierInvoiceObj.suppliersObj.ID;
                            cmd.Parameters.Add("@includeinternal", SqlDbType.Bit).Value = SupplierInvoiceObj.suppliersObj.IsInternalComp;
                        }                      
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoicesList = new List<SupplierInvoices>();
                                while (sdr.Read())
                                {
                                    SupplierInvoices CIList = new SupplierInvoices();
                                    {
                                        CIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : CIList.ID);
                                        CIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        CIList.suppliersObj = new Supplier();
                                        CIList.suppliersObj.ID = Guid.Parse(sdr["ID"].ToString());
                                        CIList.suppliersObj.ContactPerson = sdr["ContactPerson"].ToString();
                                        CIList.suppliersObj.CompanyName = sdr["CompanyName"].ToString();
                                        CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                        CIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : CIList.BalanceDue);
                                        CIList.PaidAmount = (sdr["PaidAmount"].ToString() != "" ? Decimal.Parse(sdr["PaidAmount"].ToString()) : CIList.PaidAmount);
                                        CIList.DueDays = (sdr["DueDays"].ToString() != "" ? int.Parse(sdr["DueDays"].ToString()) : CIList.DueDays);
                                        CIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : CIList.PaymentDueDateFormatted);

                                    }
                                    SupplierInvoicesList.Add(CIList);
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

            return SupplierInvoicesList;
        }

        public List<SupplierInvoices> GetOpeningSupplierInvoices(SupplierInvoices SupObj)
        {
            List<SupplierInvoices> SupplierInvoicesList = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[Accounts].[GetAllSupplierOpeningInvoices]";
                        if (SupObj!=null)
                        cmd.Parameters.Add("@includeinternal", SqlDbType.Bit).Value = SupObj.suppliersObj.IsInternalComp;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoicesList = new List<SupplierInvoices>();
                                while (sdr.Read())
                                {
                                    SupplierInvoices CIList = new SupplierInvoices();
                                    {
                                        CIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : CIList.ID);
                                        CIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        CIList.suppliersObj = new Supplier();
                                        CIList.suppliersObj.ID = Guid.Parse(sdr["ID"].ToString());
                                        CIList.suppliersObj.ContactPerson = sdr["ContactPerson"].ToString();
                                        CIList.suppliersObj.CompanyName = sdr["CompanyName"].ToString();
                                        CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                        CIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : CIList.BalanceDue);
                                        CIList.PaidAmount = (sdr["PaidAmount"].ToString() != "" ? Decimal.Parse(sdr["PaidAmount"].ToString()) : CIList.PaidAmount);
                                        CIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : CIList.PaymentDueDateFormatted);

                                    }
                                    SupplierInvoicesList.Add(CIList);
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

            return SupplierInvoicesList;
        }

        public List<SupplierInvoices> GetSupplierPurchasesByDateWise(SupplierInvoices SupplierInvoiceObj)
        {
            List<SupplierInvoices> SupplierInvoicesList = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[Accounts].[GetAllPurchaseListByDate]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (SupplierInvoiceObj != null)
                        {
                            cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value = SupplierInvoiceObj.FromDate;
                            cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = SupplierInvoiceObj.ToDate;
                            if(SupplierInvoiceObj.suppliersObj!=null)
                            cmd.Parameters.Add("@includeinternal", SqlDbType.Bit).Value = SupplierInvoiceObj.suppliersObj.IsInternalComp;
                        }
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoicesList = new List<SupplierInvoices>();
                                while (sdr.Read())
                                {
                                    SupplierInvoices CIList = new SupplierInvoices();
                                    {
                                        CIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : CIList.ID);
                                        CIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        CIList.suppliersObj = new Supplier();
                                        CIList.suppliersObj.ID = Guid.Parse(sdr["ID"].ToString());
                                        CIList.suppliersObj.ContactPerson = sdr["ContactPerson"].ToString();
                                        CIList.suppliersObj.CompanyName = sdr["CompanyName"].ToString();
                                        CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                        CIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : CIList.BalanceDue);
                                        CIList.PaidAmount = (sdr["PaidAmount"].ToString() != "" ? Decimal.Parse(sdr["PaidAmount"].ToString()) : CIList.PaidAmount);
                                        CIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : CIList.PaymentDueDateFormatted);

                                    }
                                    SupplierInvoicesList.Add(CIList);
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

            return SupplierInvoicesList;
        }


        #region DeleteSupplierInvoice
        public object DeleteSupplierInvoice(Guid ID, string userName)
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
                        cmd.CommandText = "[Accounts].[DeleteSupplierInvoice]";
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
        #endregion DeleteSupplierInvoice

        public List<SupplierInvoices> GetOutStandingInvoicesBySupplier(Guid PaymentID, Guid supplierID)
        {
            List<SupplierInvoices> SupplierInvoicesList = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[Accounts].[GetSupplierOutStandingInvoices]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = supplierID;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoicesList = new List<SupplierInvoices>();
                                while (sdr.Read())
                                {
                                    SupplierInvoices SIList = new SupplierInvoices();
                                    {
                                        SIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : SIList.ID);
                                        SIList.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : SIList.InvoiceDate);
                                        SIList.InvoiceNo = sdr["InvoiceNo"].ToString();

                                        SIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : SIList.PaymentDueDate);
                                        SIList.TotalInvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : SIList.TotalInvoiceAmount);
                                        SIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : SIList.BalanceDue);
                                        SIList.LastPaymentDate = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()) : SIList.LastPaymentDate);
                                        SIList.OtherPayments = (sdr["OtherPayments"].ToString() != "" ? Decimal.Parse(sdr["OtherPayments"].ToString()) : SIList.OtherPayments);
                                        SIList.SuppPaymentObj = new SupplierPayments();
                                        SIList.SuppPaymentObj.supplierPaymentsDetailObj = new SupplierPaymentsDetail();
                                        SIList.SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = (sdr["PaidAmountEdit"].ToString() != "" ? Decimal.Parse(sdr["PaidAmountEdit"].ToString()) : SIList.SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount);
                                        SIList.SuppPaymentObj.supplierPaymentsDetailObj.ID = (sdr["PaymentDetailID"].ToString() != "" ? Guid.Parse(sdr["PaymentDetailID"].ToString()) : SIList.SuppPaymentObj.supplierPaymentsDetailObj.ID);
                                        //------------date formatting-----------------//
                                        SIList.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.dateformat) : SIList.InvoiceDateFormatted);
                                        SIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : SIList.PaymentDueDateFormatted);
                                        SIList.LastPaymentDateFormatted = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()).ToString(settings.dateformat) : SIList.LastPaymentDateFormatted);
                                    }
                                    SupplierInvoicesList.Add(SIList);
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

            return SupplierInvoicesList;
        }

        public SupplierInvoices GetSupplierAdvances(string ID)
        {
            SupplierInvoices SIList = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[Accounts].[GetSupplierAdvances]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    SIList = new SupplierInvoices();
                                    SIList.suppliersObj = new Supplier();
                                    SIList.suppliersObj.AdvanceAmount = decimal.Parse(sdr["AdvanceAmount"].ToString());
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

            return SIList;
        }

        public  SupplierInvoiceAgeingSummary GetSupplierInvoicesAgeingSummary()
        {
            SupplierInvoiceAgeingSummary SupplierInvoiceSummaryObj = null;
            Common C = new Common();
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
                        cmd.CommandText = "[Accounts].[GetSupplierInvAgeingSummary]";
                        cmd.Parameters.Add("@OnDate", SqlDbType.Date).Value = C.GetCurrentDateTime().Date; ;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoiceSummaryObj = new SupplierInvoiceAgeingSummary();
                                if (sdr.Read())
                                {

                                    {                                       
                                        SupplierInvoiceSummaryObj.Todays = (sdr["Today"].ToString() != "" ? int.Parse(sdr["Today"].ToString()) : SupplierInvoiceSummaryObj.total);
                                        SupplierInvoiceSummaryObj.Count1To30 = (sdr["Count1to30"].ToString() != "" ? int.Parse(sdr["Count1to30"].ToString()) : SupplierInvoiceSummaryObj.Count1To30);
                                        SupplierInvoiceSummaryObj.Count31To60 = (sdr["Count31to60"].ToString() != "" ? int.Parse(sdr["Count31to60"].ToString()) : SupplierInvoiceSummaryObj.Count31To60);
                                        SupplierInvoiceSummaryObj.Count61To90= (sdr["Count61to90"].ToString() != "" ? int.Parse(sdr["Count61to90"].ToString()) : SupplierInvoiceSummaryObj.Count61To90);
                                        SupplierInvoiceSummaryObj.Count91Above = (sdr["Count90Above"].ToString() != "" ? int.Parse(sdr["Count90Above"].ToString()) : SupplierInvoiceSummaryObj.Count91Above);
                                        SupplierInvoiceSummaryObj.ThisWeek = (sdr["ThisWeek"].ToString() != "" ? int.Parse(sdr["ThisWeek"].ToString()) : SupplierInvoiceSummaryObj.ThisWeek);
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

            return SupplierInvoiceSummaryObj;
        }
    }
}