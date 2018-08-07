using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IReportBusiness
    {
        List<SystemReport> GetAllSysReports(AppUA appUA);
        List<CustomerPaymentLedger> GetCustomerPaymentLedger(DateTime? fromDate, DateTime? toDate, string customerIDs,string company,string invoiceType,string search);
        List<SupplierPaymentLedger>GetSupplierPaymentLedger(DateTime? fromDate, DateTime? toDate, string supplierCode, string company,string invoiceType);
        SaleSummary GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Boolean IsTax);
        SaleDetailReport GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Boolean IsTax, Guid Customer,string InvoiceType);
        SaleDetailReport GetRPTViewCustomerDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, Guid Customer);
        List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string ReportType, string OrderBy,string accounthead, string subtype, string employeeorother,string employeecompany,string search, string ExpenseType, string Unit);
        List<EmployeeExpenseSummaryReport> GetEmployeeExpenseSummary(DateTime? FDate, DateTime? TDate, string EmployeeCode, string OrderBy);
        List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string OrderBy, string accounthead, string subtype, string employeeorother, string employeecompany, string search,string ExpenseType,string Unit);
        List<OtherExpenseDetailsReport> GetOtherExpenseLimitedDetailReport(OtherExpenseLimitedExpenseAdvanceSearch otherExpenseLimitedDetailsAdvanceSearchObject);
        List<CustomerContactDetailsReport> GetCustomerContactDetailsReport(string search);
        List<SalesTransactionLogReport> GetSalesTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        PurchaseSummaryReport GetPurchaseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal);
        PurchaseDetailReport GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Guid Supplier,string InvoiceType,Guid EmpID,string AccountCode);
        PurchaseDetailReport GetRPTViewPurchaseDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, Guid SupplierID);
        List<SupplierContactDetailsReport> GetSupplierContactDetailsReport(string search);
        List<PurchaseTransactionLogReport> GetPurchaseTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReport(ReportAccountsReceivableAgeingSearch AccountsReceivableAgeingSearchObj);
        List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReportForSA(ReportAccountsReceivableAgeingSearch AccountsReceivableAgeingSearchObj);
        List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string Customerids);
        List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string Customerids,string InvoiceType);
        List<AccountsPayableAgeingReport> GetAccountsPayableAgeingReport(ReportAdvanceSearch reportAdvanceSearchObj);
        List<AccountsPayableAgeingSummaryReport> GetAccountsPayableAgeingSummaryReport(ReportAdvanceSearch reportAdvanceSearchObject);
        List<DepositsAndWithdrawalsDetailsReport> GetDepositAndWithdrawalDetail(DateTime? FromDate, DateTime? ToDate, string BankCode, string search);
        List<OtherIncomeSummaryReport> GetOtherIncomeSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string accounthead, string subtype, string employeeorother, string search);
        List<OtherIncomeDetailsReport> GetOtherIncomeDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string search);
        List<DailyLedgerReport> GetDailyLedgerDetails(DateTime? FromDate, DateTime? ToDate, DateTime? Date, string MainHead, string search, string Bank);
        List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? ToDate,string Filter, string Company, string Customer,string InvoiceType);
        //List<CustomerInvoiceRegisterReport> GetCustomerInvoiceRegister( string Filter, string Company, string Customer, string InvoiceType, string Search);
        List<CustomerInvoiceRegisterReport> GetCustomerInvoiceRegister(CustomerInvoiceRegisterAdvanceSearch CustomerInvoiceRegisterAdvanceSearchSearchObject);
        //List<SupplierExpeditingReport> GetSupplierExpeditingDetail(DateTime? ToDate, string Filter,string Company,string Supplier);
        List<CustomerPaidAmountDetail> GetPaidAmountDetail(Guid InvoiceID, Guid CustomerID, string RowType, string Filter, string Company, string InvoiceType);
        List<SupplierExpeditingReport> GetSupplierExpeditingDetail(ReportAdvanceSearch supplierPayementAdvanceSearchObj);
        List<SupplierInvoiceRegisterReport> GetSupplierInvoiceRegister(SupplierInvoiceRegisterAdvanceSearch supplierPayementAdvanceSearchObj);
        List<SupplierPaidAmountDetail> GetSupplierPaidAmountDetail(Guid InvoiceID, Guid SupplierID, string RowType, string Filter, string Company, string InvoiceType);
        List<TrialBalance> GetTrialBalanceReport(DateTime? Date);
        List<FollowupReport> GetFollowupReportDetail(FollowupReportAdvanceSearch advanceSearchObject);
        List<AccountHeadGroupReport> GetOtherExpenseAccountHeadGroupSummaryReport(AccountHeadGroupAdvanceSearch accountHeadGroupSummaryAdvanceSearchObject);
        List<AccountHeadGroupDetailReport> GetOtherExpenseAccountHeadGroupDetailReport(AccountHeadGroupAdvanceSearch accountHeadGroupSummaryAdvanceSearchObject);
        List<BankLedgerReport> GetBankLedgerDetails(DateTime? FromDate, DateTime? ToDate, DateTime? Date, string search, string Bank);
        DataTable GetMonthWiseIncomeExpenseSummary(string IsGrouped, string Search);
        MonthWiseIncomeExpenseSummary GetMonthWiseIncomeExpenseDetail(string month, string year, string IsGrouped, string GroupCode,string Transaction);
        List<CustomerOutStanding> GetCustomerOutStanding(DateTime? fromDate, DateTime? toDate, string invoiceType,string company, string search);
    }
}
