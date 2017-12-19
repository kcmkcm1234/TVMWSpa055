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
        public List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string ReportType,string accounthead, string subtype, string employeeorother, string employeecompany,string search)
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
                        cmd.Parameters.Add("@ReportType", SqlDbType.NVarChar, 50).Value = ReportType;
                        cmd.Parameters.Add("@AccountHead", SqlDbType.NVarChar, 50).Value = accounthead!=""?accounthead:null;
                        cmd.Parameters.Add("@SubType", SqlDbType.NVarChar, 50).Value = subtype!=""?subtype:null;
                        cmd.Parameters.Add("@EmployeeOrOther", SqlDbType.NVarChar, 50).Value = employeeorother!=""?employeeorother:null;
                        cmd.Parameters.Add("@EmployeeCompany", SqlDbType.NVarChar, 50).Value = employeecompany != "" ? employeecompany : null;
                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 250).Value = search!=""?search:null;
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
                                        otherExpenseSummary.AccountHead = (sdr["AccountHeadCode"].ToString() != "" ? sdr["AccountHeadCode"].ToString() : otherExpenseSummary.AccountHead);
                                        otherExpenseSummary.EmployeeID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : otherExpenseSummary.EmployeeID);
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

        public List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string employeecompany, string search,string ExpenseType)
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
                        cmd.Parameters.Add("@AccountHead", SqlDbType.NVarChar, 50).Value = accounthead != "" ? accounthead : null;
                        cmd.Parameters.Add("@SubType", SqlDbType.NVarChar, 50).Value = subtype != "" ? subtype : null;
                        cmd.Parameters.Add("@EmployeeOrOther", SqlDbType.NVarChar, 50).Value = employeeorother != "" ? employeeorother : null;
                        cmd.Parameters.Add("@EmployeeCompany", SqlDbType.NVarChar, 50).Value = employeecompany != "" ? employeecompany : null;
                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 250).Value = search != "" ? search : null;
                        cmd.Parameters.Add("@ExpenseType", SqlDbType.NVarChar, 50).Value = ExpenseType != "" ? ExpenseType : "ALL";
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
                                        otherExpenseDetails.EmpCompany = (sdr["EmpCompany"].ToString() != "" ? sdr["EmpCompany"].ToString() : otherExpenseDetails.EmpCompany);
                                        otherExpenseDetails.PaymentMode= (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : otherExpenseDetails.PaymentMode);

                                        otherExpenseDetails.ExpenseType = (sdr["ExpenseType"].ToString() != "" ? sdr["ExpenseType"].ToString() : otherExpenseDetails.ExpenseType);
                                        otherExpenseDetails.PaymentReference= (sdr["PaymentReference"].ToString() != "" ? sdr["PaymentReference"].ToString() : otherExpenseDetails.PaymentReference);
                                        otherExpenseDetails.Description= (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherExpenseDetails.Description);
                                        otherExpenseDetails.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherExpenseDetails.Amount);
                                        otherExpenseDetails.RowType = (sdr["RowType"].ToString() != "" ? sdr["RowType"].ToString() : otherExpenseDetails.RowType);
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

        public List<SaleDetailReport> GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string search, Boolean IsInternal,Boolean IsTax,Guid Customer,string InvoiceType)
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
                        cmd.Parameters.Add("@IsInternal", SqlDbType.Bit).Value = IsInternal ;
                        cmd.Parameters.Add("@IsTax", SqlDbType.Bit).Value = IsTax;
                        cmd.Parameters.Add("@Customercode", SqlDbType.UniqueIdentifier).Value = Customer;
                        cmd.Parameters.Add("@InvoiceType", SqlDbType.NVarChar, 50).Value = InvoiceType != "" ?InvoiceType:null;
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
                                        saleDetail.TaxAmount = (sdr["Tax"].ToString() != "" ? decimal.Parse(sdr["Tax"].ToString()) : saleDetail.TaxAmount);
                                        saleDetail.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : saleDetail.Total);
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

        public List<SaleDetailReport> GetRPTViewCustomerDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode,Guid Customer)
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
                        cmd.Parameters.Add("@Customerid", SqlDbType.UniqueIdentifier).Value = Customer;
                        cmd.CommandText = "[Accounts].[RPT_GetCustomerSalesDetail]";
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
                                        saleDetail.TaxAmount = (sdr["Tax"].ToString() != "" ? decimal.Parse(sdr["Tax"].ToString()) : saleDetail.TaxAmount);
                                        saleDetail.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : saleDetail.Total);
                                        saleDetail.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : saleDetail.GeneralNotes);
                                        saleDetail.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : saleDetail.OriginCompany);
                                        saleDetail.Origin = (sdr["Origin"].ToString() != "" ? sdr["Origin"].ToString() : saleDetail.Origin);
                                        saleDetail.RowType = (sdr["RowType"].ToString() != "" ? sdr["RowType"].ToString() : saleDetail.RowType);
                                        saleDetail.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : saleDetail.CustomerName);
                                        saleDetail.Credit = (sdr["Credit"].ToString() != "" ? decimal.Parse(sdr["Credit"].ToString()) : saleDetail.Credit);
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

        public List<SaleSummary> GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Boolean IsTax)
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
                        cmd.Parameters.Add("@IsInternal", SqlDbType.Bit).Value = IsInternal;
                        cmd.Parameters.Add("@IsTax", SqlDbType.Bit).Value = IsTax;
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
                                        saleSummary.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : saleSummary.CustomerID);
                                        saleSummary.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : saleSummary.Total);
                                        saleSummary.TaxAmount = (sdr["TaxAmount"].ToString() != "" ? decimal.Parse(sdr["TaxAmount"].ToString()) : saleSummary.TaxAmount);
                                        saleSummary.Invoiced= (sdr["Invoice"].ToString() != "" ? decimal.Parse(sdr["Invoice"].ToString()) : saleSummary.Invoiced);
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

        public List<PurchaseSummaryReport> GetPurchaseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string search,Boolean IsInternal)
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
                        cmd.Parameters.Add("@IsInternal", SqlDbType.Bit).Value = IsInternal;
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
                                        purchaseSummaryReport.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : purchaseSummaryReport.SupplierID);
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

        public List<PurchaseDetailReport> GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string search,Boolean IsInternal, Guid Supplier,string InvoiceType, Guid SubType, string AccountCode)
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
                        cmd.Parameters.Add("@IsInternal", SqlDbType.Bit).Value = IsInternal;
                          cmd.Parameters.Add("@Suppliercode", SqlDbType.UniqueIdentifier).Value = Supplier;
                        cmd.Parameters.Add("@InvoiceType", SqlDbType.NVarChar, 50).Value = InvoiceType != "" ? InvoiceType : null;
                        if (AccountCode != null && AccountCode != "")
                        {
                            string AccountHeadCode = AccountCode.Split(':')[0];
                        cmd.Parameters.Add("@AccountCode", SqlDbType.NVarChar, 50).Value = AccountHeadCode;

                        }
                        if(SubType!=Guid.Empty)
                        cmd.Parameters.Add("@SubType", SqlDbType.UniqueIdentifier).Value = SubType; 
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
                                        purchaseDetailReport.Tax = (sdr["Tax"].ToString() != "" ? decimal.Parse(sdr["Tax"].ToString()) : purchaseDetailReport.Tax);
                                        purchaseDetailReport.TotalInvoice = (sdr["TotalInvoice"].ToString() != "" ? decimal.Parse(sdr["TotalInvoice"].ToString()) : purchaseDetailReport.TotalInvoice);
                                        purchaseDetailReport.AccountCode = (sdr["TypeDesc"].ToString() != "" ? sdr["TypeDesc"].ToString() : purchaseDetailReport.AccountCode);
                                        purchaseDetailReport.SubType = (sdr["EmpName"].ToString() != "" ?sdr["EmpName"].ToString() : purchaseDetailReport.SubType);
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

        public List<PurchaseDetailReport> GetRPTViewPurchaseDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, Guid SupplierID)
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
                        cmd.Parameters.Add("@Supplierid", SqlDbType.UniqueIdentifier).Value = SupplierID;
                        cmd.CommandText = "[Accounts].[RPT_GetSupplierPurchaseDetail]";
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
                                        purchaseDetailReport.AccountHead = (sdr["TypeDesc"].ToString() != "" ? sdr["TypeDesc"].ToString() : purchaseDetailReport.AccountHead);
                                        purchaseDetailReport.EmpName = (sdr["EmpName"].ToString() != "" ? sdr["EmpName"].ToString() : purchaseDetailReport.EmpName);
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

        public List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string Customerids)
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
                        cmd.Parameters.Add("@Customerids", SqlDbType.NVarChar, -1).Value = Customerids;

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

        public List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string Customerids)
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
                        cmd.Parameters.Add("@Customerids", SqlDbType.NVarChar, -1).Value = Customerids;

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

        public List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string Customerids)
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
                        cmd.Parameters.Add("@Customerids", SqlDbType.NVarChar, -1).Value = Customerids;
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

        public List<AccountsPayableAgeingReport> GetAccountsPayableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string SupplierIDs)
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
                        cmd.Parameters.Add("@SupplierIDs", SqlDbType.NVarChar, -1).Value = SupplierIDs;
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

        public List<AccountsPayableAgeingSummaryReport> GetAccountsPayableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string SupplierIDs)
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
                        cmd.Parameters.Add("@SupplierIDs", SqlDbType.NVarChar, -1).Value = SupplierIDs;
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
        public List<OtherIncomeSummaryReport> GetOtherIncomeSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead,string subtype, string employeeorother, string search)
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
                        cmd.Parameters.Add("@SubType", SqlDbType.NVarChar, 50).Value = subtype != "" ? subtype : null;
                        cmd.Parameters.Add("@EmployeeOrOther", SqlDbType.NVarChar, 50).Value = employeeorother != "" ? employeeorother : null;
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
                                        otherIncomeSummaryReport.AccountHead= (sdr["AccountCode"].ToString() != "" ? sdr["AccountCode"].ToString() : otherIncomeSummaryReport.AccountHead);
                                        otherIncomeSummaryReport.SubTypeDesc = (sdr["SubTypeDesc"].ToString() != "" ? sdr["SubTypeDesc"].ToString() : otherIncomeSummaryReport.SubTypeDesc);                                       
                                        otherIncomeSummaryReport.EmployeeID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : otherIncomeSummaryReport.EmployeeID);
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

        public List<OtherIncomeDetailsReport> GetOtherIncomeDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead,string subtype,string employeeorother, string search)
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
                        cmd.Parameters.Add("@SubType", SqlDbType.NVarChar, 50).Value = subtype != "" ? subtype : null;
                        cmd.Parameters.Add("@EmployeeOrOther", SqlDbType.NVarChar, 50).Value = employeeorother != "" ? employeeorother : null;
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
                                        otherIncomeDetails.SubType = (sdr["SubType"].ToString() != "" ? sdr["SubType"].ToString() : otherIncomeDetails.SubType);
                                        otherIncomeDetails.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : otherIncomeDetails.OriginCompany);
                                        otherIncomeDetails.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : otherIncomeDetails.PaymentMode);
                                        otherIncomeDetails.PaymentReference = (sdr["PaymentReference"].ToString() != "" ? sdr["PaymentReference"].ToString() : otherIncomeDetails.PaymentReference);
                                        otherIncomeDetails.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherIncomeDetails.Description);
                                        otherIncomeDetails.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherIncomeDetails.Amount);
                                        otherIncomeDetails.Company = (sdr["Company"].ToString() != "" ? sdr["Company"].ToString() : otherIncomeDetails.Company);
                                        otherIncomeDetails.RowType = (sdr["RowType"].ToString() != "" ? sdr["RowType"].ToString() : otherIncomeDetails.RowType);
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

        public List<CustomerPaymentLedger> GetCustomerPaymentLedger(DateTime? FromDate, DateTime? ToDate, string CustomerIDs, string Company)
        {
            List<CustomerPaymentLedger> customerpaymentList = null;
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
                        cmd.Parameters.Add("@CustomerIDs", SqlDbType.NVarChar,-1).Value = CustomerIDs;
                        cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar,50).Value = Company;
                        cmd.CommandText = "[Accounts].[RPT_GetCustomerPaymentLedger]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerpaymentList = new List<CustomerPaymentLedger>();
                                while (sdr.Read())
                                {
                                    CustomerPaymentLedger customerpayment = new CustomerPaymentLedger();
                                    {
                                        customerpayment.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : customerpayment.Date);
                                        customerpayment.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : customerpayment.Type);
                                        customerpayment.Credit = (sdr["Credit"].ToString() != "" ? decimal.Parse(sdr["Credit"].ToString()) : customerpayment.Credit);
                                        customerpayment.Debit = (sdr["Debit"].ToString() != "" ? decimal.Parse(sdr["Debit"].ToString()) : customerpayment.Debit);
                                        customerpayment.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : customerpayment.CustomerID);
                                        customerpayment.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : customerpayment.CustomerName);
                                        customerpayment.Balance = (sdr["Balance"].ToString() != "" ? decimal.Parse(sdr["Balance"].ToString()) : customerpayment.Balance);
                                        customerpayment.Ref = (sdr["REFNO"].ToString() != "" ? sdr["REFNO"].ToString() : customerpayment.Ref);
                                        customerpayment.Company = (sdr["Company"].ToString() != "" ? sdr["Company"].ToString() : customerpayment.Company);
                                    }
                                    customerpaymentList.Add(customerpayment);
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
            return customerpaymentList;
        }

        public List<SupplierPaymentLedger> GetSupplierPaymentLedger(DateTime? FromDate, DateTime? ToDate, string Suppliercode, string Company)
        {
            List<SupplierPaymentLedger> supplierpaymentList = null;
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
                        cmd.Parameters.Add("@SupplierIDs", SqlDbType.NVarChar, -1).Value = Suppliercode;
                        cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, -1).Value = Company;
                        cmd.CommandText = "[Accounts].[RPT_GetSupplierPaymentLedger]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                supplierpaymentList = new List<SupplierPaymentLedger>();
                                while (sdr.Read())
                                {
                                    SupplierPaymentLedger supplierpayment = new SupplierPaymentLedger();
                                    {
                                        supplierpayment.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : supplierpayment.Date);
                                        supplierpayment.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : supplierpayment.Type);
                                        supplierpayment.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : supplierpayment.SupplierID);
                                        supplierpayment.SupplierName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : supplierpayment.SupplierName);
                                        supplierpayment.Credit = (sdr["Credit"].ToString() != "" ? decimal.Parse(sdr["Credit"].ToString()) : supplierpayment.Credit);
                                        supplierpayment.Debit = (sdr["Debit"].ToString() != "" ? decimal.Parse(sdr["Debit"].ToString()) : supplierpayment.Debit);
                                        supplierpayment.Balance = (sdr["Balance"].ToString() != "" ? decimal.Parse(sdr["Balance"].ToString()) : supplierpayment.Balance);
                                        supplierpayment.Ref = (sdr["REFNO"].ToString() != "" ? sdr["REFNO"].ToString() : supplierpayment.Ref);
                                        supplierpayment.Company = (sdr["Company"].ToString() != "" ? sdr["Company"].ToString() : supplierpayment.Company);
                                    }
                                    supplierpaymentList.Add(supplierpayment);
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
            return supplierpaymentList;
        }

        /// <summary>
  /// To Get Daily Ledger Details in Report
  /// </summary>
  /// <param name="FromDate"></param>
  /// <param name="ToDate"></param>
  /// <param name="Date"></param>
  /// <param name="MainHead"></param>
  /// <param name="search"></param>
  /// <returns></returns>
        public List<DailyLedgerReport> GetDailyLedgerDetails(DateTime? FromDate, DateTime? ToDate, DateTime? Date, string MainHead, string search,string Bank)
        {
            List<DailyLedgerReport> dailyLedgerList = null;
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
                        cmd.Parameters.Add("@OnDate", SqlDbType.DateTime).Value = Date;
                        cmd.Parameters.Add("@MainHead", SqlDbType.NVarChar, 50).Value = MainHead != "" ? MainHead : null;
                        cmd.Parameters.Add("@bank", SqlDbType.NVarChar, 50).Value = Bank != "" ? Bank : null;
                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 500).Value = search != "" ? search : null;
                        cmd.CommandText = "[Accounts].[RPT_DailyPaymentLedger]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                dailyLedgerList = new List<DailyLedgerReport>();
                                while (sdr.Read())
                                {
                                    DailyLedgerReport dailyLedgerDetails = new DailyLedgerReport();
                                    {
                                        dailyLedgerDetails.TransactionDate = (sdr["TransactionDate"].ToString() != "" ? DateTime.Parse(sdr["TransactionDate"].ToString()).ToString(settings.dateformat) : dailyLedgerDetails.TransactionDate);
                                        dailyLedgerDetails.EntryType = (sdr["EntryType"].ToString() != "" ? sdr["EntryType"].ToString() : dailyLedgerDetails.EntryType);
                                        dailyLedgerDetails.Particulars = (sdr["Particulars"].ToString() != "" ? sdr["Particulars"].ToString() : dailyLedgerDetails.Particulars);
                                        dailyLedgerDetails.MainHead = (sdr["mainHead"].ToString() != "" ? sdr["mainHead"].ToString() : dailyLedgerDetails.MainHead);
                                        dailyLedgerDetails.AccountHead = (sdr["AccountHead"].ToString() != "" ? sdr["AccountHead"].ToString() : dailyLedgerDetails.AccountHead);
                                        dailyLedgerDetails.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? sdr["ReferenceNo"].ToString() : dailyLedgerDetails.ReferenceNo);
                                        dailyLedgerDetails.CustomerORemployee = (sdr["CustomerORemployee"].ToString() != "" ? sdr["CustomerORemployee"].ToString() : dailyLedgerDetails.CustomerORemployee);
                                        dailyLedgerDetails.Debit = (sdr["Debit"].ToString() != "" ? sdr["Debit"].ToString() : dailyLedgerDetails.Debit);
                                        dailyLedgerDetails.Credit = (sdr["Credit"].ToString() != "" ? sdr["Credit"].ToString() : dailyLedgerDetails.Credit);
                                        dailyLedgerDetails.PayMode = (sdr["PayMode"].ToString() != "" ? sdr["PayMode"].ToString() : dailyLedgerDetails.PayMode);
                                        dailyLedgerDetails.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : dailyLedgerDetails.Remarks);
                                    }
                                    dailyLedgerList.Add(dailyLedgerDetails);
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
            return dailyLedgerList;
        }

        public List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? ToDate,string Filter)
        {
            List<CustomerExpeditingReport> CustomerExpeditingList = null;
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
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar,50).Value = Filter;
                        cmd.CommandText = "[Accounts].[RPT_CustomerPaymentExpeditingDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CustomerExpeditingList = new List<CustomerExpeditingReport>();
                                while (sdr.Read())
                                {
                                    CustomerExpeditingReport customerexpeditingdetail = new CustomerExpeditingReport();
                                    {
                                        customerexpeditingdetail.CustomerName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customerexpeditingdetail.CustomerName);
                                        customerexpeditingdetail.CustomerName1 = (sdr["CompanyName1"].ToString() != "" ? sdr["CompanyName1"].ToString() : customerexpeditingdetail.CustomerName1);
                                        customerexpeditingdetail.ContactNo = (sdr["Mobile"].ToString() != "" ? ("☎"+sdr["Mobile"].ToString()) : "") +
                                            (sdr["LandLine"].ToString() != "" ? (", ☎"+sdr["LandLine"].ToString()) : "" )+
                                            (sdr["OtherPhoneNos"].ToString() != "" ? (", ☎"+sdr["OtherPhoneNos"].ToString()) : "");
                                        customerexpeditingdetail.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : customerexpeditingdetail.InvoiceNo);
                                        customerexpeditingdetail.InvoiceDate = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : customerexpeditingdetail.InvoiceDate);
                                        customerexpeditingdetail.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : customerexpeditingdetail.Amount);
                                        customerexpeditingdetail.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? sdr["NoOfDays"].ToString() : customerexpeditingdetail.NoOfDays);
                                       

                                    }
                                    CustomerExpeditingList.Add(customerexpeditingdetail);
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
            return CustomerExpeditingList;
        }

        public List<SupplierExpeditingReport> GetSupplierExpeditingDetail(DateTime? ToDate, string Filter)
        {
            List<SupplierExpeditingReport> SupplierExpeditingList = null;
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
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = ToDate;
                        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 50).Value = Filter;
                        cmd.CommandText = "[Accounts].[RPT_SupplierPaymentExpeditingDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierExpeditingList = new List<SupplierExpeditingReport>();
                                while (sdr.Read())
                                {
                                    SupplierExpeditingReport supplierexpeditingdetail = new SupplierExpeditingReport();
                                    {
                                        supplierexpeditingdetail.SupplierName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : supplierexpeditingdetail.SupplierName);
                                        supplierexpeditingdetail.SupplierName1 = (sdr["CompanyName1"].ToString() != "" ? sdr["CompanyName1"].ToString() : supplierexpeditingdetail.SupplierName1);
                                        supplierexpeditingdetail.ContactNo = (sdr["Mobile"].ToString() != "" ? ("☎" + sdr["Mobile"].ToString()) : "") +
                                            (sdr["LandLine"].ToString() != "" ? (", ☎" + sdr["LandLine"].ToString()) : "") +
                                            (sdr["OtherPhoneNos"].ToString() != "" ? (", ☎" + sdr["OtherPhoneNos"].ToString()) : "");
                                        supplierexpeditingdetail.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : supplierexpeditingdetail.InvoiceNo);
                                        supplierexpeditingdetail.InvoiceDate = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : supplierexpeditingdetail.InvoiceDate);
                                        supplierexpeditingdetail.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : supplierexpeditingdetail.Amount);
                                        supplierexpeditingdetail.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? sdr["NoOfDays"].ToString() : supplierexpeditingdetail.NoOfDays);


                                    }
                                    SupplierExpeditingList.Add(supplierexpeditingdetail);
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
            return SupplierExpeditingList;
        }

        public List<TrialBalance> GetTrialBalanceReport(DateTime? Date)
        {
            List<TrialBalance> TBlist = null;
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
                        cmd.Parameters.Add("@OnDate", SqlDbType.DateTime).Value = Date;
                        cmd.CommandText = "[Accounts].[RPT_TrialBalance]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                TBlist = new List<TrialBalance>();
                                while (sdr.Read())
                                {
                                    TrialBalance tbdetail = new TrialBalance();
                                    {
                                        tbdetail.Account = (sdr["Account"].ToString() != "" ? sdr["Account"].ToString() : tbdetail.Account);
                                        tbdetail.Credit = (sdr["Cr"].ToString() != "" ? decimal.Parse(sdr["Cr"].ToString()) : tbdetail.Credit);
                                        tbdetail.Debit = (sdr["Dr"].ToString() != "" ? decimal.Parse(sdr["Dr"].ToString()) : tbdetail.Debit);
                                    }
                                    TBlist.Add(tbdetail);
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
            return TBlist;

        }
    }
}
