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
        Settings s = new Settings();
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

        public List<SaleDetail> GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<SaleDetail> SaleDetailList = null;
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
                        cmd.CommandText = "[Accounts].[RPT_GetSalesDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SaleDetailList = new List<SaleDetail>();
                                while (sdr.Read())
                                {
                                    SaleDetail saleDetail = new SaleDetail();
                                    {
                                        saleDetail.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : saleDetail.InvoiceNo);
                                        saleDetail.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(s.dateformat) : saleDetail.Date);
                                        saleDetail.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(s.dateformat) : saleDetail.PaymentDueDate);
                                        saleDetail.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : saleDetail.InvoiceAmount);
                                        saleDetail.PaidAmount = (sdr["PaidAmount"].ToString() != "" ? decimal.Parse(sdr["PaidAmount"].ToString()) : saleDetail.PaidAmount);
                                        saleDetail.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? decimal.Parse(sdr["BalanceDue"].ToString()) : saleDetail.BalanceDue);
                                        saleDetail.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : saleDetail.GeneralNotes);
                                        saleDetail.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? sdr["OriginCompany"].ToString() : saleDetail.OriginCompany);
                                        saleDetail.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : saleDetail.CustomerName);
                                       
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

        public List<SaleSummary> GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
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
                                        saleSummary.OriginCompany = (sdr["OriginCompany"].ToString() != "" ? (sdr["OriginCompany"].ToString()) : saleSummary.OriginCompany);
                                      
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
    }
}