using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace SPAccounts.DataAccessObject.DTO
{
    public class SystemReport
    {
        public Guid AppID { get; set; }
        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReportGroup { get; set; }
        public int GroupOrder { get; set; }
        public string SPName { get; set; }
        public string SQL { get; set; }
        public int ReportOrder { get; set; }
    }

    public class SaleSummary
    {
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Total { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Paid { get; set; }
        public decimal NetDue { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string OriginCompany { get; set; }
        public string RowType { get; set; }
        public Boolean IncludeInternal { get; set; }
        public Boolean IncludeTax { get; set; }
        public string salesummarySum { get; set; }
        public string salesummaryTotalInvoice { get; set; }
        public string salesummaryInvoiced { get; set; }
        public string salesummarypaid { get; set; }
        public string salesummaryTax { get; set; }
        public List<SaleSummary> saleSummaryList { get; set; }
    }

    public class SaleDetailReport
    {
        public string InvoiceNo { get; set; }
        public string Date { get; set; }
        public string PaymentDueDate { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public string OriginCompany { get; set; }
        public string Origin { get; set; }        //--To get Company name--
        public string GeneralNotes { get; set; }
        public string CustomerName { get; set; }
        public decimal Credit { get; set; }
        public string RowType { get; set; }
        public decimal Total { get; set; }
        public decimal TaxAmount { get; set; }
        public Boolean IncludeInternal { get; set; }
        public Boolean IncludeTax { get; set; }
        public string saledetailSum { get; set; }
        public string saledetailinvoice { get; set; }
        public string saledetailpaid { get; set; }
        public string saledetailtax { get; set; }
        public string saledetailtotalinvoiced { get; set; }
        public List<SaleDetailReport> saleDetailList { get; set; }

    }
    public class OtherExpenseSummaryReport
    {
        public string AccountHeadORSubtype { get; set; }
        public string SubTypeDesc { get; set; }
        public decimal Amount { get; set; }
        public string EmpCompany { get; set; }
        public string OriginCompany { get; set; }
        public string Employee { get; set; }
        public Guid EmployeeID { get; set; }
        public string Search { get; set; }
        public string Subtype { get; set; }
        public string AccountHead { get; set; }
        public string EmployeeOrOther { get; set; }
        public decimal TotalAmount { get; set; }
        public string Description { get; set; }
        public string ExpenseType { get; set; }
        public decimal ReversedAmount { get; set; }
    }


    public class OtherExpenseDetailsReport
    {
        public string AccountHeadORSubtype { get; set; }
        public string AccountHead { get; set; }
        public string SubType { get; set; }
        public string EmpCompany { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentReference { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal ReversedAmount { get; set; }
        public string Company { get; set; }
        public string OriginCompany { get; set; }
        public string EmployeeOrOther { get; set; }
        public decimal TotalAmount { get; set; }
        public string Date { get; set; }
        public string RowType { get; set; }
        public string ExpenseType { get; set; }
    }

    public class OtherExpenseLimitedExpenseAdvanceSearch
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string ExpenseType { get; set; }
        public string Company { get; set; }
        public string Search { get; set; }
        public string EmployeeOrOther { get; set; }
        public string EmpCompany { get; set; }
        public string AccountHead { get; set; }
        public string SubType { get; set; }
    }

    public class CustomerContactDetailsReport
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string OtherPhoneNos { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
    }

    public class SalesTransactionLogReport
    {

        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Date { get; set; }
        public string TransactionType { get; set; }
        public string DocNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }

    }

    public class PurchaseSummaryReport
    {
        public Guid SupplierID { get; set; }
        public string CompanyCode { get; set; }
        public string SupplierName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Credit { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public decimal NetDue { get; set; }
        public string OriginCompany { get; set; }
        public string RowType { get; set; }
        public Boolean IncludeInternal { get; set; }
        public Boolean IncludeTax { get; set; }
        public string purchaseSummarySum { get; set; }
        public string purchaseSummaryPaid { get; set; }
        public string purchaseSummaryInvoice { get; set; }
        public List<PurchaseSummaryReport> purchaseSummaryReportList { get; set; }

    }
    public class PurchaseDetailReport
    {
        public string CompanyCode { get; set; }
        public string InvoiceNo { get; set; }
        public string Date { get; set; }
        public string PaymentDueDate { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PaymentProcessed { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal TotalInvoice { get; set; }
        public string OriginCompany { get; set; }
        public string Origin { get; set; }
        public string GeneralNotes { get; set; }
        public string SupplierName { get; set; }
        public string AccountHead { get; set; }
        public string EmpName { get; set; }
        public decimal Credit { get; set; }
        public decimal Tax { get; set; }
        public string RowType { get; set; }
        public Boolean IncludeInternal { get; set; }
        public Boolean IncludeTax { get; set; }
        public string purchaseDetailSum { get; set; }
        public string purchaseDetailPaid { get; set; }
        public string purchaseDetailInvoice { get; set; }
        public string purchaseDetailPaymentProcess { get; set; }
        public string purchaseDetailsTaxAmount { get; set; }
        public string purchaseDetailsTotalAmount { get; set; }
        public List<PurchaseDetailReport> purchaseDetailReportList { get; set; }
        public string AccountCode { get; set; }
        public string SubType { get; set; }

    }
    public class SupplierContactDetailsReport
    {

        public string CompanyCode { get; set; }
        public string SupplierName { get; set; }
        public string PhoneNumber { get; set; }
        public string OtherPhoneNos { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }

    }
    public class PurchaseTransactionLogReport
    {

        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Date { get; set; }
        public string TransactionType { get; set; }
        public string DocNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }

    }

    public class AccountsReceivableAgeingReport
    {
        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string TransactionDate { get; set; }
        public string DocNo { get; set; }
        public string CustomerName { get; set; }
        public string DueDate { get; set; }
        public string DaysPastDue { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public string Group { get; set; }
        public string InvoiceType { get; set; }

    }
    public class AccountsReceivableAgeingSummaryReport
    {

        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Customer { get; set; }
        public string Current { get; set; }
        public string OneToThirty { get; set; }
        public string ThirtyOneToSixty { get; set; }
        public string SixtyOneToNinety { get; set; }
        public string NinetyOneAndOver { get; set; }

    }
    public class AccountsPayableAgeingReport
    {
        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string TransactionDate { get; set; }
        public string DocNo { get; set; }
        public string CustomerName { get; set; }
        public string DueDate { get; set; }
        public string DaysPastDue { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public string Group { get; set; }
        public ReportAdvanceSearch ReportAdvanceSearchObj { get; set; }
        public string Search { get; set; }
    }
    public class AccountsPayableAgeingSummaryReport
    {

        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Supplier { get; set; }
        public string Current { get; set; }
        public string OneToThirty { get; set; }
        public string ThirtyOneToSixty { get; set; }
        public string SixtyOneToNinety { get; set; }
        public string NinetyOneAndOver { get; set; }

    }

    public class EmployeeExpenseSummaryReport
    {
        public string EmployeeCode { get; set; }
        public Guid EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCompany { get; set; }
        public string AccountHead { get; set; }
        public decimal Amount { get; set; }
    }

    public class EmployeeExpenseDetailReport
    {
        public string EmployeeCode { get; set; }
        public Guid EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCompany { get; set; }
        public string OriginCompany { get; set; }
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public string AccountHead { get; set; }
        public decimal Amount { get; set; }
    }

    public class DepositsAndWithdrawalsDetailsReport
    {
        public string TransactionDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string ReferenceBank { get; set; }
        public string OurBank { get; set; }
        public string Mode { get; set; }
        public string CheckClearDate { get; set; }
        public string Withdrawal { get; set; }
        public string Deposit { get; set; }
        public string DepositNotCleared { get; set; }
    }

    public class CustomerPaymentLedger

    {
        public string Date { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }
        public string Company { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string CustomerCode { get; set; }
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CompanyCode { get; set; }
        public string InvoiceType { get; set; }
    }


    public class SupplierPaymentLedger
    {
        public string Date { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }
        public string Company { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public Guid SupplierID { get; set; }
        public string SupplierName { get; set; }
        public decimal Balance { get; set; }
        public string SupplierCode { get; set; }
        public string InvoiceType { get; set; }
    }

    public class TrialBalance
    {
        public string Date { get; set; }
        public string Account { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }

    public class OtherIncomeSummaryReport
    {
        public string AccountHeadORSubtype { get; set; }
        public decimal Amount { get; set; }
        public string OriginCompany { get; set; }
        public string Search { get; set; }
        public string AccountHead { get; set; }
        public string Subtype { get; set; }
        public string EmployeeOrOther { get; set; }
        public string Employee { get; set; }
        public string SubTypeDesc { get; set; }

        public Guid EmployeeID { get; set; }
    }

    public class OtherIncomeDetailsReport
    {
        public string AccountHeadORSubtype { get; set; }
        public string AccountHead { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentReference { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Company { get; set; }
        public string OriginCompany { get; set; }
        public string Date { get; set; }
        public string RowType { get; set; }
        public string SubType { get; set; }
        public string EmployeeOrOther { get; set; }
    }

    public class DailyLedgerReport
    {
        public string TransactionDate { get; set; }
        public string EntryType { get; set; }
        public string MainHead { get; set; }
        public string AccountHead { get; set; }
        public string ReferenceNo { get; set; }
        public string CustomerORemployee { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string PayMode { get; set; }
        public string Remarks { get; set; }
        public string Particulars { get; set; }
        public List<SaleSummary> BankList { get; set; }
    }

    public class CustomerExpeditingReport
    {
        public string CustomerName { get; set; }
        public string CustomerName1 { get; set; }
        public string ContactNo { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string OtherPhoneNos { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public Decimal Amount { get; set; }
        public string NoOfDays { get; set; }
        public string Remarks { get; set; }
        public CustomerContactDetailsReport CustomerContactObj { get; set; }
        public Companies CompanyObj { get; set; }
        public Customer CustomerObj { get; set; }
    }


    public class SupplierExpeditingReport
    {
        public string SupplierName { get; set; }
        public string SupplierName1 { get; set; }
        public string LandLine { get; set; }
        public string ContactNo { get; set; }
        public string Mobile { get; set; }
        public string OtherPhoneNos { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public Decimal Amount { get; set; }
        public string NoOfDays { get; set; }
        public string Remarks { get; set; }
        public SupplierContactDetailsReport SupplierContactObj { get; set; }
        public Companies CompanyObj { get; set; }
        public Supplier SupplierObj { get; set; }

    }


    public class ReportAdvanceSearch
    {
        public string ToDate { get; set; }
        public string Filter { get; set; }
        public string Company { get; set; }
        public string Supplier { get; set; }
        public string FromDate { get; set; }
        public string InvoiceType { get; set; }
        public string Customer { get; set; }
        public string CompanyCode { get; set; }
        public string SupplierIDs { get; set; }
        public string Search { get; set; }
    }


}


