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
    public class ReportRepository: IReportRepository
    {
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;
        public ReportRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<SystemReport> GetAllSysReports(AppUA appUA)
        {
            List<SystemReport> Reportlist = null;
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
                        cmd.Parameters.Add("@AppID", SqlDbType.UniqueIdentifier).Value = appUA.AppID;
                        cmd.CommandText = "[Accounts].[GetAllSys_Reports]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Reportlist = new List<SystemReport>();
                                while (sdr.Read())
                                {
                                    SystemReport _ReportObj = new SystemReport();
                                    {
                                        _ReportObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReportObj.ID);
                                        _ReportObj.AppID = (sdr["AppID"].ToString() != "" ? Guid.Parse(sdr["AppID"].ToString()) : _ReportObj.AppID);
                                        _ReportObj.ReportName = (sdr["ReportName"].ToString() != "" ? (sdr["ReportName"].ToString()) : _ReportObj.ReportName);
                                        _ReportObj.ReportDescription = (sdr["ReportDescription"].ToString() != "" ? (sdr["ReportDescription"].ToString()) : _ReportObj.ReportDescription);
                                        _ReportObj.Controller = (sdr["Controller"].ToString() != "" ? sdr["Controller"].ToString() : _ReportObj.Controller);
                                        _ReportObj.Action = (sdr["Action"].ToString() != "" ? sdr["Action"].ToString() : _ReportObj.Action);
                                        _ReportObj.SPName = (sdr["SPName"].ToString() != "" ? sdr["SPName"].ToString() : _ReportObj.SPName);
                                        _ReportObj.SQL = (sdr["SQL"].ToString() != "" ? sdr["SQL"].ToString() : _ReportObj.SQL);
                                        _ReportObj.ReportOrder = (sdr["ReportOrder"].ToString() != "" ? int.Parse(sdr["ReportOrder"].ToString()) : _ReportObj.ReportOrder);
                                        _ReportObj.ReportGroup = (sdr["ReportGroup"].ToString() != "" ? sdr["ReportGroup"].ToString() : _ReportObj.ReportGroup);
                                        _ReportObj.GroupOrder= (sdr["GroupOrder"].ToString() != "" ? int.Parse(sdr["GroupOrder"].ToString()) : _ReportObj.GroupOrder);
                                    }
                                    Reportlist.Add(_ReportObj);
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
            return Reportlist;
        }

        /// <summary>
        /// Get other expense summary
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="CompanyCode"></param>
        /// <returns>List<OtherExpenseSummaryReport></returns>
        public List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string accounthead, string subtype, string employeeorother,string search)
        {
            List<OtherExpenseSummaryReport> otherExpenseSummaryList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@accounthead", SqlDbType.NVarChar, 50).Value = accounthead!=""?accounthead:null;
                        cmd.Parameters.Add("@subtype", SqlDbType.NVarChar, 50).Value = subtype!=""?subtype:null;
                        cmd.Parameters.Add("@employeeorother", SqlDbType.NVarChar, 50).Value = employeeorother!=""?employeeorother:null;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search!=""?search:null;
                        cmd.CommandText = "[Accounts].[RPT_GetOtherExpenseSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherExpenseSummaryList = new List<OtherExpenseSummaryReport>();
                                while (sdr.Read())
                                {
                                    OtherExpenseSummaryReport otherExpenseSummary = new OtherExpenseSummaryReport();
                                    {
                                        otherExpenseSummary.AccountHeadORSubtype = (sdr["AccountHeadORSubtype"].ToString() != "" ? sdr["AccountHeadORSubtype"].ToString() : otherExpenseSummary.AccountHeadORSubtype);
                                        otherExpenseSummary.SubTypeDesc = (sdr["SubTypeDesc"].ToString() != "" ? sdr["SubTypeDesc"].ToString() : otherExpenseSummary.SubTypeDesc);
                                        otherExpenseSummary.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherExpenseSummary.Amount);
                                        otherExpenseSummary.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : otherExpenseSummary.OriginCompany);
                                        //otherExpenseSummary.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherExpenseSummary.Description);
                                    }
                                    otherExpenseSummaryList.Add(otherExpenseSummary);
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
            return otherExpenseSummaryList;
        }


        public List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string search)
        {
            List<OtherExpenseDetailsReport> otherExpenseDetailList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@accounthead", SqlDbType.NVarChar, 50).Value = accounthead != "" ? accounthead : null;
                        cmd.Parameters.Add("@subtype", SqlDbType.NVarChar, 50).Value = subtype != "" ? subtype : null;
                        cmd.Parameters.Add("@employeeorother", SqlDbType.NVarChar, 50).Value = employeeorother != "" ? employeeorother : null;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetOtherExpenseDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherExpenseDetailList = new List<OtherExpenseDetailsReport>();
                                while (sdr.Read())
                                {
                                    OtherExpenseDetailsReport otherExpenseDetails = new OtherExpenseDetailsReport();
                                    {
                                        otherExpenseDetails.AccountHead = (sdr["AccountHead"].ToString() != "" ? sdr["AccountHead"].ToString() : otherExpenseDetails.AccountHead);
                                        otherExpenseDetails.Date= (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : otherExpenseDetails.Date);
                                        otherExpenseDetails.SubType = (sdr["Subtype"].ToString() != "" ? sdr["Subtype"].ToString() : otherExpenseDetails.SubType);
                                        otherExpenseDetails.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : otherExpenseDetails.OriginCompany);
                                        otherExpenseDetails.PaymentMode= (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : otherExpenseDetails.PaymentMode);
                                      
                                        otherExpenseDetails.PaymentReference= (sdr["PaymentReference"].ToString() != "" ? sdr["PaymentReference"].ToString() : otherExpenseDetails.PaymentReference);
                                        otherExpenseDetails.Description= (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherExpenseDetails.Description);
                                        otherExpenseDetails.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherExpenseDetails.Amount);
                                        otherExpenseDetails.Company= (sdr["Company"].ToString() != "" ? sdr["Company"].ToString() : otherExpenseDetails.Company);
                                    }
                                    otherExpenseDetailList.Add(otherExpenseDetails);
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
            return otherExpenseDetailList;
        }

        public List<SaleDetailReport> GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string search)
        {
            List<SaleDetailReport> SaleDetailList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetSalesDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SaleDetailList = new List<SaleDetailReport>();
                                while (sdr.Read())
                                {
                                    SaleDetailReport saleDetail = new SaleDetailReport();
                                    {
                                        saleDetail.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : saleDetail.InvoiceNo);
                                        saleDetail.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : saleDetail.Date);
                                        saleDetail.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : saleDetail.PaymentDueDate);
                                        saleDetail.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : saleDetail.InvoiceAmount);
                                        saleDetail.PaidAmount = (sdr["PaidAmount"].ToString() != "" ? decimal.Parse(sdr["PaidAmount"].ToString()) : saleDetail.PaidAmount);
                                        saleDetail.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? decimal.Parse(sdr["BalanceDue"].ToString()) : saleDetail.BalanceDue);
                                        saleDetail.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : saleDetail.GeneralNotes);
                                        saleDetail.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : saleDetail.OriginCompany);
                                        saleDetail.Origin = (sdr["Origin"].ToString() != "" ? sdr["Origin"].ToString() : saleDetail.Origin);
                                        saleDetail.RowType = (sdr["RowType"].ToString() != "" ? sdr["RowType"].ToString() : saleDetail.RowType);
                                        saleDetail.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : saleDetail.CustomerName);
                                        saleDetail.Credit= (sdr["Credit"].ToString() != "" ? decimal.Parse(sdr["Credit"].ToString()) : saleDetail.Credit);
                                    }
                                    SaleDetailList.Add(saleDetail);
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
            return SaleDetailList;
        }

        public List<SaleSummary> GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<SaleSummary> SaleSummaryList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;

                        cmd.CommandText = "[Accounts].[RPT_GetSalesSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SaleSummaryList = new List<SaleSummary>();
                                while (sdr.Read())
                                {
                                    SaleSummary saleSummary = new SaleSummary();
                                    {
                                        saleSummary.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : saleSummary.CustomerName);
                                        saleSummary.OpeningBalance = (sdr["OpeningBalance"].ToString() != "" ? decimal.Parse(sdr["OpeningBalance"].ToString()) : saleSummary.OpeningBalance);
                                        saleSummary.Invoiced= (sdr["Invoiced"].ToString() != "" ? decimal.Parse(sdr["Invoiced"].ToString()) : saleSummary.Invoiced);
                                        saleSummary.Paid = (sdr["Paid"].ToString() != "" ? decimal.Parse(sdr["Paid"].ToString()) : saleSummary.Paid);
                                        saleSummary.NetDue= (sdr["NetDue"].ToString() != "" ? decimal.Parse(sdr["NetDue"].ToString()) : saleSummary.NetDue);
                                        saleSummary.Credit= (sdr["Credit"].ToString() != "" ? decimal.Parse(sdr["Credit"].ToString()) : saleSummary.Credit);
                                        saleSummary.RowType = (sdr["RowType"].ToString() != "" ? sdr["RowType"].ToString() : saleSummary.RowType);
                                        saleSummary.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? (sdr["OriginCompany"].ToString()) : saleSummary.OriginCompany);
                                        saleSummary.Balance = (sdr["Balance"].ToString() != "" ? decimal.Parse(sdr["Balance"].ToString()) : saleSummary.Balance);
                                    }
                                    SaleSummaryList.Add(saleSummary);
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
            return SaleSummaryList;
        }

        public List<CustomerContactDetailsReport> GetCustomerContactDetailsReport(string search)
        {
            List<CustomerContactDetailsReport> CustomerContactList = null;
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
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetCustomerContactDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CustomerContactList = new List<CustomerContactDetailsReport>();
                                while (sdr.Read())
                                {
                                  CustomerContactDetailsReport CustomerContactDetailsReport = new CustomerContactDetailsReport();
                                    {
                                        CustomerContactDetailsReport.CustomerName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : CustomerContactDetailsReport.CustomerName);
                                        CustomerContactDetailsReport.PhoneNumber = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : CustomerContactDetailsReport.PhoneNumber);
                                        CustomerContactDetailsReport.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : CustomerContactDetailsReport.OtherPhoneNos);
                                        CustomerContactDetailsReport.Email = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : CustomerContactDetailsReport.Email);
                                        CustomerContactDetailsReport.ContactName = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : CustomerContactDetailsReport.ContactName);
                                        CustomerContactDetailsReport.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : CustomerContactDetailsReport.BillingAddress);
                                        CustomerContactDetailsReport.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : CustomerContactDetailsReport.ShippingAddress);
                                      
                                    }
                                    CustomerContactList.Add(CustomerContactDetailsReport);
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
            return CustomerContactList;
        }

        public List<SalesTransactionLogReport> GetSalesTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<SalesTransactionLogReport> salesTransactionLogReportList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetSalesTransactionLog]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                salesTransactionLogReportList = new List<SalesTransactionLogReport>();
                                while (sdr.Read())
                                {
                                    SalesTransactionLogReport salesTransactionLogReport = new SalesTransactionLogReport();
                                    {
                                        salesTransactionLogReport.CompanyCode= (sdr["CompanyCode"].ToString() != "" ? sdr["CompanyCode"].ToString() : salesTransactionLogReport.CompanyCode);
                                        salesTransactionLogReport.OriginatedCompany = (sdr["OriginatedCompany"].ToString() != "" ? sdr["OriginatedCompany"].ToString() : salesTransactionLogReport.OriginatedCompany);
                                        salesTransactionLogReport.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : salesTransactionLogReport.Date);
                                        salesTransactionLogReport.TransactionType = (sdr["TransactionType"].ToString() != "" ? sdr["TransactionType"].ToString() : salesTransactionLogReport.TransactionType);
                                        salesTransactionLogReport.DocNo = (sdr["DocNo"].ToString() != "" ? sdr["DocNo"].ToString() : salesTransactionLogReport.DocNo);
                                        salesTransactionLogReport.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : salesTransactionLogReport.Amount);       
                                    }
                                    salesTransactionLogReportList.Add(salesTransactionLogReport);
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
            return salesTransactionLogReportList;
        }

        public List<PurchaseSummaryReport> GetPurchaseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string search)
        {
            List<PurchaseSummaryReport> purchaseSummaryReportList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetPurchaseSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseSummaryReportList = new List<PurchaseSummaryReport>();
                                while (sdr.Read())
                                {
                                    PurchaseSummaryReport purchaseSummaryReport = new PurchaseSummaryReport();
                                    {
                                        purchaseSummaryReport.SupplierName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : purchaseSummaryReport.SupplierName);
                                        purchaseSummaryReport.OpeningBalance = (sdr["OpeningBalance"].ToString() != "" ? decimal.Parse(sdr["OpeningBalance"].ToString()) : purchaseSummaryReport.OpeningBalance);
                                        purchaseSummaryReport.Invoiced = (sdr["Invoiced"].ToString() != "" ? decimal.Parse(sdr["Invoiced"].ToString()) : purchaseSummaryReport.Invoiced);
                                        purchaseSummaryReport.Paid = (sdr["Paid"].ToString() != "" ? decimal.Parse(sdr["Paid"].ToString()) : purchaseSummaryReport.Paid);
                                        purchaseSummaryReport.NetDue = (sdr["NetDue"].ToString() != "" ? decimal.Parse(sdr["NetDue"].ToString()) : purchaseSummaryReport.NetDue);
                                        purchaseSummaryReport.Credit = (sdr["Credit"].ToString() != "" ? decimal.Parse(sdr["Credit"].ToString()) : purchaseSummaryReport.Credit);
                                        purchaseSummaryReport.RowType = (sdr["RowType"].ToString() != "" ? sdr["RowType"].ToString() : purchaseSummaryReport.RowType);
                                        purchaseSummaryReport.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? (sdr["OriginCompany"].ToString()) : purchaseSummaryReport.OriginCompany);
                                        purchaseSummaryReport.Balance = (sdr["Balance"].ToString() != "" ? decimal.Parse(sdr["Balance"].ToString()) : purchaseSummaryReport.Balance);
                                    }
                                    purchaseSummaryReportList.Add(purchaseSummaryReport);
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
            return purchaseSummaryReportList;
        }

        public List<PurchaseDetailReport> GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string search)
        {
            List<PurchaseDetailReport> purchaseDetailReportList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetPurchaseDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseDetailReportList = new List<PurchaseDetailReport>();
                                while (sdr.Read())
                                {
                                    PurchaseDetailReport purchaseDetailReport = new PurchaseDetailReport();
                                    {
                                        purchaseDetailReport.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : purchaseDetailReport.InvoiceNo);
                                        purchaseDetailReport.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : purchaseDetailReport.Date);
                                        purchaseDetailReport.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : purchaseDetailReport.PaymentDueDate);
                                        purchaseDetailReport.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : purchaseDetailReport.InvoiceAmount);
                                        purchaseDetailReport.PaidAmount = (sdr["PaidAmount"].ToString() != "" ? decimal.Parse(sdr["PaidAmount"].ToString()) : purchaseDetailReport.PaidAmount);
                                        purchaseDetailReport.PaymentProcessed = (sdr["PaymentProcessed"].ToString() != "" ? decimal.Parse(sdr["PaymentProcessed"].ToString()) : purchaseDetailReport.PaymentProcessed);
                                        purchaseDetailReport.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? decimal.Parse(sdr["BalanceDue"].ToString()) : purchaseDetailReport.BalanceDue);
                                        purchaseDetailReport.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : purchaseDetailReport.GeneralNotes);
                                        purchaseDetailReport.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : purchaseDetailReport.OriginCompany);
                                        purchaseDetailReport.Origin = (sdr["Origin"].ToString() != "" ? sdr["Origin"].ToString() : purchaseDetailReport.Origin);
                                        purchaseDetailReport.RowType = (sdr["RowType"].ToString() != "" ? sdr["RowType"].ToString() : purchaseDetailReport.RowType);
                                        purchaseDetailReport.SupplierName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : purchaseDetailReport.SupplierName);
                                        purchaseDetailReport.Credit = (sdr["Credit"].ToString() != "" ? decimal.Parse(sdr["Credit"].ToString()) : purchaseDetailReport.Credit);
                                    }
                                    purchaseDetailReportList.Add(purchaseDetailReport);
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
            return purchaseDetailReportList;
        }

        public List<SupplierContactDetailsReport> GetSupplierContactDetailsReport(string search)
        {
            List<SupplierContactDetailsReport> SupplierContactDetailsReportList = null;
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
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetSupplierContactDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierContactDetailsReportList = new List<SupplierContactDetailsReport>();
                                while (sdr.Read())
                                {
                                    SupplierContactDetailsReport supplierContactDetailsReport = new SupplierContactDetailsReport();
                                    {
                                        supplierContactDetailsReport.SupplierName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : supplierContactDetailsReport.SupplierName);
                                        supplierContactDetailsReport.PhoneNumber = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : supplierContactDetailsReport.PhoneNumber);
                                        supplierContactDetailsReport.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : supplierContactDetailsReport.OtherPhoneNos);
                                        supplierContactDetailsReport.Email = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : supplierContactDetailsReport.Email);
                                        supplierContactDetailsReport.ContactName = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : supplierContactDetailsReport.ContactName);
                                        supplierContactDetailsReport.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : supplierContactDetailsReport.BillingAddress);
                                        supplierContactDetailsReport.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : supplierContactDetailsReport.ShippingAddress);

                                    }
                                    SupplierContactDetailsReportList.Add(supplierContactDetailsReport);
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
            return SupplierContactDetailsReportList;
        }

        public List<PurchaseTransactionLogReport> GetPurchaseTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string search)
        {
            List<PurchaseTransactionLogReport> purchaseTransactionLogReportList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetPurchaseTransactionLog]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseTransactionLogReportList = new List<PurchaseTransactionLogReport>();
                                while (sdr.Read())
                                {
                                    PurchaseTransactionLogReport purchaseTransactionLogReport = new PurchaseTransactionLogReport();
                                    {
                                        purchaseTransactionLogReport.CompanyCode = (sdr["CompanyCode"].ToString() != "" ? sdr["CompanyCode"].ToString() : purchaseTransactionLogReport.CompanyCode);
                                        purchaseTransactionLogReport.OriginatedCompany = (sdr["OriginatedCompany"].ToString() != "" ? sdr["OriginatedCompany"].ToString() : purchaseTransactionLogReport.OriginatedCompany);
                                        purchaseTransactionLogReport.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : purchaseTransactionLogReport.Date);
                                        purchaseTransactionLogReport.TransactionType = (sdr["TransactionType"].ToString() != "" ? sdr["TransactionType"].ToString() : purchaseTransactionLogReport.TransactionType);
                                        purchaseTransactionLogReport.DocNo = (sdr["DocNo"].ToString() != "" ? sdr["DocNo"].ToString() : purchaseTransactionLogReport.DocNo);
                                        purchaseTransactionLogReport.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : purchaseTransactionLogReport.Amount);
                                    }
                                    purchaseTransactionLogReportList.Add(purchaseTransactionLogReport);
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
            return purchaseTransactionLogReportList;
        }

        public List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsReceivableAgeingReport> accountsReceivableAgeingReportList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.CommandText = "[Accounts].[RPT_GetAccountsReceivableAgeingDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                accountsReceivableAgeingReportList = new List<AccountsReceivableAgeingReport>();
                                while (sdr.Read())
                                {
                                    AccountsReceivableAgeingReport accountsReceivableAgeingReport = new AccountsReceivableAgeingReport();
                                    {
                                        accountsReceivableAgeingReport.CompanyCode = (sdr["CompanyCode"].ToString() != "" ? sdr["CompanyCode"].ToString() : accountsReceivableAgeingReport.CompanyCode);
                                        accountsReceivableAgeingReport.CustomerName= (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : accountsReceivableAgeingReport.CustomerName);
                                        accountsReceivableAgeingReport.OriginatedCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : accountsReceivableAgeingReport.OriginatedCompany);
                                        accountsReceivableAgeingReport.DueDate = (sdr["DueDate"].ToString() != "" ? DateTime.Parse(sdr["DueDate"].ToString()).ToString(settings.dateformat) : accountsReceivableAgeingReport.DueDate);
                                        accountsReceivableAgeingReport.TransactionDate = (sdr["TransactionDate"].ToString() != "" ? DateTime.Parse(sdr["TransactionDate"].ToString()).ToString(settings.dateformat) : accountsReceivableAgeingReport.TransactionDate);
                                        accountsReceivableAgeingReport.DaysPastDue= (sdr["DaysPastDue"].ToString() != "" ? sdr["DaysPastDue"].ToString() : accountsReceivableAgeingReport.DaysPastDue);
                                        accountsReceivableAgeingReport.DocNo = (sdr["DocNo"].ToString() != "" ? sdr["DocNo"].ToString() : accountsReceivableAgeingReport.DocNo);
                                        accountsReceivableAgeingReport.Invoiced = (sdr["Invoiced"].ToString() != "" ? decimal.Parse(sdr["Invoiced"].ToString()) : accountsReceivableAgeingReport.Invoiced);
                                        accountsReceivableAgeingReport.Paid = (sdr["Paid"].ToString() != "" ? decimal.Parse(sdr["Paid"].ToString()) : accountsReceivableAgeingReport.Paid);
                                        accountsReceivableAgeingReport.Balance= (sdr["Balance"].ToString() != "" ? decimal.Parse(sdr["Balance"].ToString()) : accountsReceivableAgeingReport.Balance);
                                        accountsReceivableAgeingReport.Group= (sdr["Group"].ToString() != "" ? sdr["Group"].ToString() : accountsReceivableAgeingReport.Group);
                                        accountsReceivableAgeingReport.InvoiceType = (sdr["InvoiceType"].ToString() != "" ? sdr["InvoiceType"].ToString() : accountsReceivableAgeingReport.InvoiceType);
                                    }
                                    accountsReceivableAgeingReportList.Add(accountsReceivableAgeingReport);
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
            return accountsReceivableAgeingReportList;
        }

        public List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsReceivableAgeingSummaryReport> accountsReceivableAgeingSummaryReportList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.CommandText = "[Accounts].[RPT_GetAccountsReceivableAgeingSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                accountsReceivableAgeingSummaryReportList = new List<AccountsReceivableAgeingSummaryReport>();
                                while (sdr.Read())
                                {
                                    AccountsReceivableAgeingSummaryReport accountsReceivableAgeingSummaryReport = new AccountsReceivableAgeingSummaryReport();
                                    {

                                        accountsReceivableAgeingSummaryReport.Customer = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : accountsReceivableAgeingSummaryReport.Customer);
                                        accountsReceivableAgeingSummaryReport.Current = (sdr["Current"].ToString() != "" ? sdr["Current"].ToString() : accountsReceivableAgeingSummaryReport.Current);
                                        accountsReceivableAgeingSummaryReport.OneToThirty = (sdr["1-30"].ToString() != "" ? sdr["1-30"].ToString() : accountsReceivableAgeingSummaryReport.OneToThirty);
                                        accountsReceivableAgeingSummaryReport.ThirtyOneToSixty = (sdr["31-60"].ToString() != "" ? sdr["31-60"].ToString() : accountsReceivableAgeingSummaryReport.ThirtyOneToSixty);
                                        accountsReceivableAgeingSummaryReport.SixtyOneToNinety = (sdr["61-90"].ToString() != "" ? sdr["61-90"].ToString() : accountsReceivableAgeingSummaryReport.SixtyOneToNinety);
                                        accountsReceivableAgeingSummaryReport.NinetyOneAndOver = (sdr["91 And Over"].ToString() != "" ? sdr["91 And Over"].ToString() : accountsReceivableAgeingSummaryReport.NinetyOneAndOver);

                                    }
                                    accountsReceivableAgeingSummaryReportList.Add(accountsReceivableAgeingSummaryReport);
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
            return accountsReceivableAgeingSummaryReportList;
        }
        public List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsReceivableAgeingSummaryReport> accountsReceivableAgeingSummaryReportList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.CommandText = "[Accounts].[RPT_GetAccountsReceivableAgeingSummaryForSA]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                accountsReceivableAgeingSummaryReportList = new List<AccountsReceivableAgeingSummaryReport>();
                                while (sdr.Read())
                                {
                                    AccountsReceivableAgeingSummaryReport accountsReceivableAgeingSummaryReport = new AccountsReceivableAgeingSummaryReport();
                                    {

                                        accountsReceivableAgeingSummaryReport.Customer = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : accountsReceivableAgeingSummaryReport.Customer);
                                        accountsReceivableAgeingSummaryReport.Current = (sdr["Current"].ToString() != "" ? sdr["Current"].ToString() : accountsReceivableAgeingSummaryReport.Current);
                                        accountsReceivableAgeingSummaryReport.OneToThirty = (sdr["1-30"].ToString() != "" ? sdr["1-30"].ToString() : accountsReceivableAgeingSummaryReport.OneToThirty);
                                        accountsReceivableAgeingSummaryReport.ThirtyOneToSixty = (sdr["31-60"].ToString() != "" ? sdr["31-60"].ToString() : accountsReceivableAgeingSummaryReport.ThirtyOneToSixty);
                                        accountsReceivableAgeingSummaryReport.SixtyOneToNinety = (sdr["61-90"].ToString() != "" ? sdr["61-90"].ToString() : accountsReceivableAgeingSummaryReport.SixtyOneToNinety);
                                        accountsReceivableAgeingSummaryReport.NinetyOneAndOver = (sdr["91 And Over"].ToString() != "" ? sdr["91 And Over"].ToString() : accountsReceivableAgeingSummaryReport.NinetyOneAndOver);

                                    }
                                    accountsReceivableAgeingSummaryReportList.Add(accountsReceivableAgeingSummaryReport);
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
            return accountsReceivableAgeingSummaryReportList;
        }

        public List<AccountsPayableAgeingReport> GetAccountsPayableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsPayableAgeingReport> accountsPayableAgeingReportList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.CommandText = "[Accounts].[RPT_GetAccountsPayableAgeingDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                accountsPayableAgeingReportList = new List<AccountsPayableAgeingReport>();
                                while (sdr.Read())
                                {
                                    AccountsPayableAgeingReport accountsPayableAgeingReport = new AccountsPayableAgeingReport();
                                    {
                                        accountsPayableAgeingReport.CompanyCode = (sdr["CompanyCode"].ToString() != "" ? sdr["CompanyCode"].ToString() : accountsPayableAgeingReport.CompanyCode);
                                        accountsPayableAgeingReport.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : accountsPayableAgeingReport.CustomerName);
                                        accountsPayableAgeingReport.OriginatedCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : accountsPayableAgeingReport.OriginatedCompany);
                                        accountsPayableAgeingReport.DueDate = (sdr["DueDate"].ToString() != "" ? DateTime.Parse(sdr["DueDate"].ToString()).ToString(settings.dateformat) : accountsPayableAgeingReport.DueDate);
                                        accountsPayableAgeingReport.TransactionDate = (sdr["TransactionDate"].ToString() != "" ? DateTime.Parse(sdr["TransactionDate"].ToString()).ToString(settings.dateformat) : accountsPayableAgeingReport.TransactionDate);
                                        accountsPayableAgeingReport.DaysPastDue = (sdr["DaysPastDue"].ToString() != "" ? sdr["DaysPastDue"].ToString() : accountsPayableAgeingReport.DaysPastDue);
                                        accountsPayableAgeingReport.DocNo = (sdr["DocNo"].ToString() != "" ? sdr["DocNo"].ToString() : accountsPayableAgeingReport.DocNo);
                                        accountsPayableAgeingReport.Invoiced = (sdr["Invoiced"].ToString() != "" ? decimal.Parse(sdr["Invoiced"].ToString()) : accountsPayableAgeingReport.Invoiced);
                                        accountsPayableAgeingReport.Paid = (sdr["Paid"].ToString() != "" ? decimal.Parse(sdr["Paid"].ToString()) : accountsPayableAgeingReport.Paid);
                                        accountsPayableAgeingReport.Balance = (sdr["Balance"].ToString() != "" ? decimal.Parse(sdr["Balance"].ToString()) : accountsPayableAgeingReport.Balance);
                                        accountsPayableAgeingReport.Group = (sdr["Group"].ToString() != "" ? sdr["Group"].ToString() : accountsPayableAgeingReport.Group);
                                    }
                                    accountsPayableAgeingReportList.Add(accountsPayableAgeingReport);
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
            return accountsPayableAgeingReportList;
        }

        public List<AccountsPayableAgeingSummaryReport> GetAccountsPayableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsPayableAgeingSummaryReport> accountsPayableAgeingSummaryReportList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.CommandText = "[Accounts].[RPT_GetAccountsPayableAgeingSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                accountsPayableAgeingSummaryReportList = new List<AccountsPayableAgeingSummaryReport>();
                                while (sdr.Read())
                                {
                                    AccountsPayableAgeingSummaryReport AccountsPayableAgeingSummaryReport = new AccountsPayableAgeingSummaryReport();
                                    {

                                        AccountsPayableAgeingSummaryReport.Supplier = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : AccountsPayableAgeingSummaryReport.Supplier);
                                        AccountsPayableAgeingSummaryReport.Current = (sdr["Current"].ToString() != "" ? sdr["Current"].ToString() : AccountsPayableAgeingSummaryReport.Current);
                                        AccountsPayableAgeingSummaryReport.OneToThirty = (sdr["1-30"].ToString() != "" ? sdr["1-30"].ToString() : AccountsPayableAgeingSummaryReport.OneToThirty);
                                        AccountsPayableAgeingSummaryReport.ThirtyOneToSixty = (sdr["31-60"].ToString() != "" ? sdr["31-60"].ToString() : AccountsPayableAgeingSummaryReport.ThirtyOneToSixty);
                                        AccountsPayableAgeingSummaryReport.SixtyOneToNinety = (sdr["61-90"].ToString() != "" ? sdr["61-90"].ToString() : AccountsPayableAgeingSummaryReport.SixtyOneToNinety);
                                        AccountsPayableAgeingSummaryReport.NinetyOneAndOver = (sdr["91 And Over"].ToString() != "" ? sdr["91 And Over"].ToString() : AccountsPayableAgeingSummaryReport.NinetyOneAndOver);

                                    }
                                    accountsPayableAgeingSummaryReportList.Add(AccountsPayableAgeingSummaryReport);
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
            return accountsPayableAgeingSummaryReportList;
        }

        public List<EmployeeExpenseSummaryReport> GetEmployeeExpenseSummary(DateTime? FromDate, DateTime? ToDate, string EmployeeCode)
        {
            List<EmployeeExpenseSummaryReport> employeeExpenseSummaryList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@EmployeeCode", SqlDbType.NVarChar, 50).Value = EmployeeCode;
                        cmd.CommandText = " [Accounts].[RPT_GetOtherExpenseSummaryByEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeExpenseSummaryList = new List<EmployeeExpenseSummaryReport>();
                                while (sdr.Read())
                                {
                                    EmployeeExpenseSummaryReport employeeExpenseSummary = new EmployeeExpenseSummaryReport();
                                    {
                                        employeeExpenseSummary.AccountHead = (sdr["AccountHead"].ToString() != "" ? sdr["AccountHead"].ToString() : employeeExpenseSummary.AccountHead);
                                        employeeExpenseSummary.EmployeeCode = (sdr["EmployeeCode"].ToString() != "" ? sdr["EmployeeCode"].ToString() : employeeExpenseSummary.EmployeeCode);
                                        employeeExpenseSummary.EmployeeCompany = (sdr["EmployeeCompany"].ToString() != "" ? sdr["EmployeeCompany"].ToString() : employeeExpenseSummary.EmployeeCompany);
                                        employeeExpenseSummary.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : employeeExpenseSummary.Amount);
                                        employeeExpenseSummary.EmployeeName = (sdr["EmployeeName"].ToString() != "" ? sdr["EmployeeName"].ToString() : employeeExpenseSummary.EmployeeName);
                                    }
                                    employeeExpenseSummaryList.Add(employeeExpenseSummary);
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
            return employeeExpenseSummaryList;
        }


        /// <summary>
        ///To Get Deposit And Withdrawal Details in Report
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="BankCode"></param>
        /// <returns>List</returns>
        public List<DepositsAndWithdrawalsDetailsReport> GetDepositAndWithdrawalDetail(DateTime? FromDate, DateTime? ToDate, string BankCode, string search)
        {
            List<DepositsAndWithdrawalsDetailsReport> depositAndWithdrawalDetailList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@BankCode", SqlDbType.NVarChar, 50).Value = BankCode;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetDepositsAndWithdrawalDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                depositAndWithdrawalDetailList = new List<DepositsAndWithdrawalsDetailsReport>();
                                while (sdr.Read())
                                {
                                    DepositsAndWithdrawalsDetailsReport depositAndWithdrawalDetailReport = new DepositsAndWithdrawalsDetailsReport();
                                    {
                                        depositAndWithdrawalDetailReport.TransactionDate = (sdr["TransactionDate"].ToString() != "" ? DateTime.Parse(sdr["TransactionDate"].ToString()).ToString(settings.dateformat) : depositAndWithdrawalDetailReport.TransactionDate);
                                        depositAndWithdrawalDetailReport.ReferenceNumber = (sdr["ReferenceNumber"].ToString() != "" ? sdr["ReferenceNumber"].ToString() : depositAndWithdrawalDetailReport.ReferenceNumber);
                                        depositAndWithdrawalDetailReport.ReferenceBank = (sdr["ReferenceBank"].ToString() != "" ? sdr["ReferenceBank"].ToString() : depositAndWithdrawalDetailReport.ReferenceBank);
                                        depositAndWithdrawalDetailReport.OurBank = (sdr["OurBank"].ToString() != "" ? sdr["OurBank"].ToString() : depositAndWithdrawalDetailReport.OurBank);
                                        depositAndWithdrawalDetailReport.Mode = (sdr["Mode"].ToString() != "" ? sdr["Mode"].ToString() : depositAndWithdrawalDetailReport.Mode);
                                        depositAndWithdrawalDetailReport.CheckClearDate = (sdr["CheckClearDate"].ToString() != "" ? DateTime.Parse(sdr["CheckClearDate"].ToString()).ToString(settings.dateformat) : depositAndWithdrawalDetailReport.CheckClearDate);
                                        depositAndWithdrawalDetailReport.Withdrawal = (sdr["Withdrawal"].ToString() != "" ? sdr["Withdrawal"].ToString() : depositAndWithdrawalDetailReport.Withdrawal);
                                        depositAndWithdrawalDetailReport.Deposit = (sdr["Deposit"].ToString() != "" ? sdr["Deposit"].ToString() : depositAndWithdrawalDetailReport.Deposit);
                                        depositAndWithdrawalDetailReport.DepositNotCleared = (sdr["DepositNotCleared"].ToString() != "" ? sdr["DepositNotCleared"].ToString() : depositAndWithdrawalDetailReport.DepositNotCleared);
                                    }
                                    depositAndWithdrawalDetailList.Add(depositAndWithdrawalDetailReport);
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
            return depositAndWithdrawalDetailList;
        }


        /// <summary>
        ///To Get Other Income Summary in Report
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="CompanyCode"></param>
        /// <returns>List</returns>
        public List<OtherIncomeSummaryReport> GetOtherIncomeSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string search)// string subtype, string employeeorother,
        {
            List<OtherIncomeSummaryReport> otherIncomeSummaryList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@accounthead", SqlDbType.NVarChar, 50).Value = accounthead != "" ? accounthead : null;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetOtherIncomeSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherIncomeSummaryList = new List<OtherIncomeSummaryReport>();
                                while (sdr.Read())
                                {
                                    OtherIncomeSummaryReport otherIncomeSummaryReport = new OtherIncomeSummaryReport();
                                    {
                                        otherIncomeSummaryReport.AccountHeadORSubtype = (sdr["AccountHeadORSubtype"].ToString() != "" ? sdr["AccountHeadORSubtype"].ToString() : otherIncomeSummaryReport.AccountHeadORSubtype);
                                        otherIncomeSummaryReport.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherIncomeSummaryReport.Amount);
                                        otherIncomeSummaryReport.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : otherIncomeSummaryReport.OriginCompany);
                                        
                                    }
                                    otherIncomeSummaryList.Add(otherIncomeSummaryReport);
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
            return otherIncomeSummaryList;
        }
        /// <summary>
        /// To Get Other Income Details in Report
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="CompanyCode"></param>
        /// <param name="accounthead"></param>
        /// <param name="search"></param>
        /// <returns></returns>

        public List<OtherIncomeDetailsReport> GetOtherIncomeDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string search)
        {
            List<OtherIncomeDetailsReport> otherIncomeDetailList = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = CompanyCode;
                        cmd.Parameters.Add("@accounthead", SqlDbType.NVarChar, 50).Value = accounthead != "" ? accounthead : null;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_GetOtherIncomeDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherIncomeDetailList = new List<OtherIncomeDetailsReport>();
                                while (sdr.Read())
                                {
                                    OtherIncomeDetailsReport otherIncomeDetails = new OtherIncomeDetailsReport();
                                    {
                                        otherIncomeDetails.AccountHead = (sdr["AccountHead"].ToString() != "" ? sdr["AccountHead"].ToString() : otherIncomeDetails.AccountHead);
                                        otherIncomeDetails.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : otherIncomeDetails.Date);

                                        otherIncomeDetails.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : otherIncomeDetails.OriginCompany);
                                        otherIncomeDetails.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : otherIncomeDetails.PaymentMode);
                                        otherIncomeDetails.PaymentReference = (sdr["PaymentReference"].ToString() != "" ? sdr["PaymentReference"].ToString() : otherIncomeDetails.PaymentReference);
                                        otherIncomeDetails.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherIncomeDetails.Description);
                                        otherIncomeDetails.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherIncomeDetails.Amount);
                                        otherIncomeDetails.Company = (sdr["Company"].ToString() != "" ? sdr["Company"].ToString() : otherIncomeDetails.Company);
                                    }
                                    otherIncomeDetailList.Add(otherIncomeDetails);
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
            return otherIncomeDetailList;
        }

    }
}
