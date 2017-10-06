using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IReportBusiness
    {
        List<SystemReport> GetAllSysReports(AppUA appUA);
        List<CustomerPaymentLedger> GetCustomerPaymentLedger(DateTime? FromDate, DateTime? ToDate, string CustomerIDs);
        List<SupplierPaymentLedger>GetSupplierPaymentLedger(DateTime? FromDate, DateTime? ToDate, string Suppliercode);
        List<SaleSummary> GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<SaleDetailReport> GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string ReportType, string OrderBy,string accounthead, string subtype, string employeeorother,string employeecompany,string search);
        List<EmployeeExpenseSummaryReport> GetEmployeeExpenseSummary(DateTime? FDate, DateTime? TDate, string EmployeeCode, string OrderBy);
        List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string OrderBy, string accounthead, string subtype, string employeeorother, string employeecompany, string search);
        List<CustomerContactDetailsReport> GetCustomerContactDetailsReport(string search);
        List<SalesTransactionLogReport> GetSalesTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<PurchaseSummaryReport> GetPurchaseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<PurchaseDetailReport> GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<SupplierContactDetailsReport> GetSupplierContactDetailsReport(string search);
        List<PurchaseTransactionLogReport> GetPurchaseTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode);
        List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode);
        List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode);
        List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode);
        List<AccountsPayableAgeingReport> GetAccountsPayableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode);
        List<AccountsPayableAgeingSummaryReport> GetAccountsPayableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode);
        List<DepositsAndWithdrawalsDetailsReport> GetDepositAndWithdrawalDetail(DateTime? FromDate, DateTime? ToDate, string BankCode, string search);
        List<OtherIncomeSummaryReport> GetOtherIncomeSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string accounthead, string search);
        List<OtherIncomeDetailsReport> GetOtherIncomeDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string search);
        List<DailyLedgerReport> GetDailyLedgerDetails(DateTime? FromDate, DateTime? ToDate, DateTime? Date, string MainHead, string search);
    }
}
