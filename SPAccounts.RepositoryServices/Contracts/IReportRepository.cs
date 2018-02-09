using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public  interface IReportRepository
    {
        List<SystemReport> GetAllSysReports(AppUA appUA);
        List<CustomerPaymentLedger> GetCustomerPaymentLedger(DateTime? fromDate, DateTime? toDate, string customerIDs, string company,string invoiceType);
        List<SupplierPaymentLedger> GetSupplierPaymentLedger(DateTime? fromDate, DateTime? toDate, string supplierCode, string company, string invoiceType);
        List<SaleSummary> GetSaleSummary(DateTime? FromDate,DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Boolean IsTax);
        List<SaleDetailReport> GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Boolean IsTax, Guid Customer,string InvoiceType);
        List<SaleDetailReport> GetRPTViewCustomerDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, Guid Customer);
        List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string ReportType, string accounthead, string subtype, string employeeorother, string employeecompany,string search,string ExpenseType);
        List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string employeecompany, string search,string ExpenseType);
        List<OtherExpenseDetailsReport> GetOtherExpenseLimitedDetailReport(OtherExpenseLimitedExpenseAdvanceSearch limitedExpenseadvanceSearchObject);
        List<EmployeeExpenseSummaryReport> GetEmployeeExpenseSummary(DateTime? FromDate, DateTime? ToDate, string EmployeeCode);
        List<CustomerContactDetailsReport> GetCustomerContactDetailsReport(string search);
        List<SalesTransactionLogReport> GetSalesTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<PurchaseSummaryReport> GetPurchaseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal);
        List<PurchaseDetailReport> GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Guid Supplier,string InvoiceType, Guid EmpID, string AccountCode);
        List<PurchaseDetailReport> GetRPTViewPurchaseDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, Guid SupplierID);
        List<SupplierContactDetailsReport> GetSupplierContactDetailsReport(string search);
        List<PurchaseTransactionLogReport> GetPurchaseTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReport(ReportAccountsReceivableAgeingSearch AccountsReceivableAgeingSearchObj);
        List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string Customerids);
        List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string Customerids,string InvoiceType);
        List<AccountsPayableAgeingReport> GetAccountsPayableAgeingReport(ReportAdvanceSearch reportAdvanceSearchObj);
        List<AccountsPayableAgeingSummaryReport> GetAccountsPayableAgeingSummaryReport(ReportAdvanceSearch reportAdvanceSearchObj);
        List<DepositsAndWithdrawalsDetailsReport> GetDepositAndWithdrawalDetail(DateTime? FromDate, DateTime? ToDate, string BankCode, string search);
        List<OtherIncomeSummaryReport> GetOtherIncomeSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string search);
        List<OtherIncomeDetailsReport> GetOtherIncomeDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string search);
        List<DailyLedgerReport> GetDailyLedgerDetails(DateTime? FromDate, DateTime? ToDate, DateTime? Date, string MainHead, string search,string Bank);
        List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? ToDate,string Filter,string Company,string Customer,string InvoiceType);        
       // List<SupplierExpeditingReport> GetSupplierExpeditingDetail(DateTime? ToDate, string Filter,string Company,string Supplier);
        List<SupplierExpeditingReport> GetSupplierExpeditingDetail(ReportAdvanceSearch advanceSearchObject);
        List<TrialBalance> GetTrialBalanceReport(DateTime? Date);
        List<FollowupReport> GetFollowupReport(FollowupReportAdvanceSearch advanceSearchObject);

    }
}
