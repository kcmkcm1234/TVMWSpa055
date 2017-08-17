using SPAccounts.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;

namespace SPAccounts.BusinessService.Services
{
    public class ReportBusiness : IReportBusiness
    {
        IReportRepository _reportRepository;
        public ReportBusiness(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public List<SystemReport> GetAllSysReports(AppUA appUA)
        {
            List<SystemReport> systemReportList = null;
            try
            {
                systemReportList = _reportRepository.GetAllSysReports(appUA);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return systemReportList;
        }

        public List<CustomerContactDetailsReport> GetCustomerContactDetailsReport()
        {
            List<CustomerContactDetailsReport> CustomerContactDetailsList = null;
            try
            {
                CustomerContactDetailsList = _reportRepository.GetCustomerContactDetailsReport();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerContactDetailsList;
        }

        public List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string OrderBy)
        {
            List<OtherExpenseDetailsReport> otherExpenseDetailsList = null;
            try
            {
                otherExpenseDetailsList = _reportRepository.GetOtherExpenseDetails(FromDate, ToDate, CompanyCode);
                if (otherExpenseDetailsList != null)
                {
                    switch (OrderBy)
                    {
                        case "AH":
                            otherExpenseDetailsList = otherExpenseDetailsList.OrderBy(OE => OE.Company).ThenBy(OE=>OE.AccountHead).ToList();
                            break;

                        case "ST":
                            otherExpenseDetailsList = otherExpenseDetailsList.OrderBy(OE => OE.Company).ThenBy(OE => OE.SubType).ToList();
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseDetailsList;
        }

        public List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string OrderBy)
        {
            List<OtherExpenseSummaryReport> otherExpenseSummaryList = null;
            try
            {
                otherExpenseSummaryList = _reportRepository.GetOtherExpenseSummary(FromDate, ToDate, CompanyCode);
                //if (otherExpenseSummaryList != null)
                //{
                //    switch (OrderBy)
                //    {
                //        case "AH":
                //          //  otherExpenseSummaryList = otherExpenseSummaryList.OrderBy(OE => OE.AccountHeadORSubtype).ToList();
                //            break;

                //        case "ST":
                //          //  otherExpenseSummaryList = otherExpenseSummaryList.OrderByDescending(OE => OE.SubTypeDesc).ToList();
                //            break;
                //    }

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseSummaryList;
        }

        public List<PurchaseDetailReport> GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<PurchaseDetailReport> purchaseDetailReportList = null;
            try
            {
                purchaseDetailReportList = _reportRepository.GetPurchaseDetails(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return purchaseDetailReportList;
        }

        public List<PurchaseSummaryReport> GetPurchaseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<PurchaseSummaryReport> purchaseSummaryReportList = null;
            try
            {
                purchaseSummaryReportList = _reportRepository.GetPurchaseSummary(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return purchaseSummaryReportList;
        }

        public List<PurchaseTransactionLogReport> GetPurchaseTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<PurchaseTransactionLogReport> purchaseTransactionLogReportList = null;
            try
            {
                purchaseTransactionLogReportList = _reportRepository.GetPurchaseTransactionLogDetails(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return purchaseTransactionLogReportList;
        }

        public List<SaleDetailReport> GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<SaleDetailReport> saleDetailList = null;
            try
            {
                saleDetailList = _reportRepository.GetSaleDetail(FromDate,ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saleDetailList;
        }

        public List<SalesTransactionLogReport> GetSalesTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<SalesTransactionLogReport> salesTransactionLogReportList = null;
            try
            {
                salesTransactionLogReportList = _reportRepository.GetSalesTransactionLogDetails(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return salesTransactionLogReportList;
        }

        public List<SaleSummary> GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<SaleSummary> saleSummaryList = null;
            try
            {
                saleSummaryList = _reportRepository.GetSaleSummary(FromDate, ToDate, CompanyCode);

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saleSummaryList;
        }

        public List<SupplierContactDetailsReport> GetSupplierContactDetailsReport()
        {
            List<SupplierContactDetailsReport> supplierContactDetailsReportList = null;
            try
            {
                supplierContactDetailsReportList = _reportRepository.GetSupplierContactDetailsReport();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return supplierContactDetailsReportList;
        }
    }
}