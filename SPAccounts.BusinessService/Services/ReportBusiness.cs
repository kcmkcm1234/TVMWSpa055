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

        public List<CustomerContactDetailsReport> GetCustomerContactDetailsReport(string search)
        {
            List<CustomerContactDetailsReport> CustomerContactDetailsList = null;
            try
            {
                CustomerContactDetailsList = _reportRepository.GetCustomerContactDetailsReport(search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerContactDetailsList;
        }

        public List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string OrderBy, string accounthead, string subtype, string employeeorother, string employeecompany, string search)
        {
            List<OtherExpenseDetailsReport> otherExpenseDetailsList = null;
            try
            {
                otherExpenseDetailsList = _reportRepository.GetOtherExpenseDetails(FromDate, ToDate, CompanyCode, accounthead,  subtype, employeeorother, employeecompany, search);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseDetailsList;
        }

        public List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string ReportType, string OrderBy, string accounthead, string subtype, string employeeorother,string employeecompany, string search)
        {
            List<OtherExpenseSummaryReport> otherExpenseSummaryList = null;
           
            try
            {
                otherExpenseSummaryList = _reportRepository.GetOtherExpenseSummary(FromDate, ToDate, CompanyCode,ReportType, accounthead, subtype, employeeorother, employeecompany, search);
                
                }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseSummaryList;
        }

        public List<PurchaseDetailReport> GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<PurchaseDetailReport> purchaseDetailReportList = null;
            try
            {
                purchaseDetailReportList = _reportRepository.GetPurchaseDetails(FromDate, ToDate, CompanyCode,search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return purchaseDetailReportList;
        }

        public List<PurchaseSummaryReport> GetPurchaseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<PurchaseSummaryReport> purchaseSummaryReportList = null;
            try
            {
                purchaseSummaryReportList = _reportRepository.GetPurchaseSummary(FromDate, ToDate, CompanyCode,search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return purchaseSummaryReportList;
        }

        public List<PurchaseTransactionLogReport> GetPurchaseTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<PurchaseTransactionLogReport> purchaseTransactionLogReportList = null;
            try
            {
                purchaseTransactionLogReportList = _reportRepository.GetPurchaseTransactionLogDetails(FromDate, ToDate, CompanyCode,search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return purchaseTransactionLogReportList;
        }

        public List<SaleDetailReport> GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<SaleDetailReport> saleDetailList = null;
            try
            {
                saleDetailList = _reportRepository.GetSaleDetail(FromDate,ToDate, CompanyCode,search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saleDetailList;
        }

        public List<SalesTransactionLogReport> GetSalesTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<SalesTransactionLogReport> salesTransactionLogReportList = null;
            try
            {
                salesTransactionLogReportList = _reportRepository.GetSalesTransactionLogDetails(FromDate, ToDate, CompanyCode,search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return salesTransactionLogReportList;
        }

        public List<SaleSummary> GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<SaleSummary> saleSummaryList = null;
            try
            {
                saleSummaryList = _reportRepository.GetSaleSummary(FromDate, ToDate, CompanyCode,search);

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saleSummaryList;
        }

        public List<SupplierContactDetailsReport> GetSupplierContactDetailsReport(string search)
        {
            List<SupplierContactDetailsReport> supplierContactDetailsReportList = null;
            try
            {
                supplierContactDetailsReportList = _reportRepository.GetSupplierContactDetailsReport(search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return supplierContactDetailsReportList;
        }
        public List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsReceivableAgeingReport> accountsReceivableAgeingReportList = null;
            try
            {
                accountsReceivableAgeingReportList = _reportRepository.GetAccountsReceivableAgeingReport(FromDate, ToDate, CompanyCode);
                accountsReceivableAgeingReportList=accountsReceivableAgeingReportList.Where(C => C.InvoiceType == "RB").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsReceivableAgeingReportList;
        }
        public List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsReceivableAgeingReport> accountsReceivableAgeingReportList = null;
            try
            {
                accountsReceivableAgeingReportList = _reportRepository.GetAccountsReceivableAgeingReport(FromDate, ToDate, CompanyCode);
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
                accountsReceivableAgeingSummaryReportList = _reportRepository.GetAccountsReceivableAgeingSummaryReport(FromDate, ToDate, CompanyCode);
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
                accountsReceivableAgeingSummaryReportList = _reportRepository.GetAccountsReceivableAgeingSummaryReportForSA(FromDate, ToDate, CompanyCode);
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
                accountsPayableAgeingReportList = _reportRepository.GetAccountsPayableAgeingReport(FromDate, ToDate, CompanyCode);
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
                accountsPayableAgeingSummaryReportList = _reportRepository.GetAccountsPayableAgeingSummaryReport(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsPayableAgeingSummaryReportList;
        }



        public List<EmployeeExpenseSummaryReport> GetEmployeeExpenseSummary(DateTime? FromDate, DateTime? ToDate, string EmployeeCode, string OrderBy)
        {
            List<EmployeeExpenseSummaryReport> employeeExpenseSummaryList = null;
            try
            {
                employeeExpenseSummaryList = _reportRepository.GetEmployeeExpenseSummary(FromDate, ToDate, EmployeeCode);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeExpenseSummaryList;
        }


        public List<DepositsAndWithdrawalsDetailsReport> GetDepositAndWithdrawalDetail(DateTime? FromDate, DateTime? ToDate, string BankCode, string search)
        {
            List<DepositsAndWithdrawalsDetailsReport> depositAndWithdrawalDetailList = null;
            try
            {
                depositAndWithdrawalDetailList = _reportRepository.GetDepositAndWithdrawalDetail(FromDate, ToDate, BankCode, search);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return depositAndWithdrawalDetailList;
        }
        public List<OtherIncomeSummaryReport> GetOtherIncomeSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string search)
        {
            List<OtherIncomeSummaryReport> otherIncomeSummaryList = null;

            try
            {
                otherIncomeSummaryList = _reportRepository.GetOtherIncomeSummary(FromDate, ToDate, CompanyCode, accounthead, search);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherIncomeSummaryList;
        }

        public List<OtherIncomeDetailsReport> GetOtherIncomeDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string search)
        {
            List<OtherIncomeDetailsReport> otherIncomeDetailsList = null;
            try
            {
                otherIncomeDetailsList = _reportRepository.GetOtherIncomeDetails(FromDate, ToDate, CompanyCode, accounthead, search);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherIncomeDetailsList;
        }

         public List<CustomerPaymentLedger> GetCustomerPaymentLedger(DateTime? FromDate, DateTime? ToDate, string CustomerIDs)
        {
            List<CustomerPaymentLedger> CustomerPaymentsDetailsList = null;
            try
            {
                CustomerPaymentsDetailsList = _reportRepository.GetCustomerPaymentLedger(FromDate,ToDate, CustomerIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerPaymentsDetailsList;
        }
        public List<SupplierPaymentLedger> GetSupplierPaymentLedger(DateTime? FromDate, DateTime? ToDate, string Suppliercode)
        {
            List<SupplierPaymentLedger> SupplierPaymentsDetailsList = null;
            try
            {
                SupplierPaymentsDetailsList = _reportRepository.GetSupplierPaymentLedger(FromDate, ToDate, Suppliercode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SupplierPaymentsDetailsList;
        }
    }
}
