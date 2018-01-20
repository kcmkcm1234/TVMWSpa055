using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class SystemReportViewModel
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
    public class SaleSummaryViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string Search { get; set; }
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Paid { get; set; }
        public decimal Total { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetDue { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string OriginCompany { get; set; }
        public string RowType { get; set; }
        public Boolean IncludeInternal { get; set; }
        public Boolean IncludeTax { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public string salesummarySum { get; set; }
        public string salesummaryInvoiced { get; set; }
        public string salesummaryTotalInvoice { get; set; }
        public string salesummarypaid { get; set; }
        public string salesummaryTax { get; set; }
        public List<SaleSummaryViewModel> saleSummaryList { get; set; }
    }
    public class SaleDetailReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        [Display(Name = "Customer")]
        public string CustomerCode { get; set; }
        public string InvoiceNo { get; set; }
        public string Date { get; set; }
        public string PaymentDueDate { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal Total { get; set; }
        public decimal TaxAmount { get; set; }
        public string OriginCompany { get; set; }
        public string Origin { get; set; }        //--To get Company name--
        public string GeneralNotes { get; set; }
        public string CustomerName { get; set; }
        public decimal Credit { get; set; }
        public string Search { get; set; }
        public string RowType { get; set; }
        public Boolean IncludeInternal { get; set; }
        public Boolean IncludeTax { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<SelectListItem> customerList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public string saledetailSum { get; set; }
        public string saledetailinvoice { get; set; }
        public string saledetailpaid { get; set; }
        public string saledetailtax { get; set; }
        public string saledetailtotalinvoiced { get; set; }
        public string InvoiceType { get; set; }
        public List<SaleDetailReportViewModel> saleDetailList { get; set; }

    }

    public class OtherExpenseSummaryReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        [Display(Name = "Employee Company")]
        public string EmpCompany { get; set; }
        [Display(Name = "Account Head")]
        public string AccountHead { get; set; }
        public string Subtype { get; set; }
        [Display(Name = "Employee/Other")]
        public string EmployeeOrOther { get; set; }
        public List<SelectListItem> AccountHeadList { get; set; }
        public List<SelectListItem> SubtypeList { get; set; }
        public string AccountHeadORSubtype { get; set; }
        public string Employee { get; set; }
        public Guid EmployeeID { get; set; } 
        public string SubTypeDesc { get; set; }
        public decimal Amount { get; set; }
        public string OriginCompany { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public List<SelectListItem> EmployeeTypeList { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public string Search { get; set; }
        public decimal TotalAmount { get; set; }
        public string Description { get; set; }

        [Display(Name = "Expense Type")]
        public string ExpenseType { get; set; }
        public decimal ReversedAmount { get; set; }
    }


    public class OtherExpenseDetailsReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        [Display(Name = "Employee/Other")]
        public string EmployeeOrOther { get; set; }
        [Display(Name = "Employee Company")]
        public string EmpCompany { get; set; }
        [Display(Name = "Account Head")]
        public string AccountHead { get; set; }
        public string SubType { get; set; }
        public string Date { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentReference { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal ReversedAmount { get; set; }
        public string OriginCompany { get; set; }
        public string Company { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public List<SelectListItem> EmployeeTypeList { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public List<SelectListItem> AccountHeadList { get; set; }
        public List<SelectListItem> SubtypeList { get; set; }
        public string Search { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReversedTotal { get; set; }
        public string RowType { get; set; }
        public CompaniesViewModel CompanyObj { get; set; }
        public OtherExpenseViewModel OtherExpenseObj { get; set; }
        [Display(Name = "Expense Type")]
        public string ExpenseType { get; set; }
        public OtherExpenseLimitedExpenseAdvanceSearchViewModel OtherExpenseLEAdvSearch {get;set;}
    }

    public class OtherExpenseLimitedExpenseAdvanceSearchViewModel
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string ExpenseType { get; set; }
        public string Company { get; set; }
        public string Search { get; set; }
        [Display(Name = "Employee/Other")]
        public string EmployeeOrOther { get; set; }
        public string EmpCompany { get; set; }
        public string AccountHead { get; set; }
        public string SubType { get; set; }
    }

    public class CustomerContactDetailsReportViewModel
    {
      
        public string CompanyCode { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string OtherPhoneNos { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string Search { get; set; }

    }

    public class SalesTransactionLogReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Date { get; set; }
        public string TransactionType { get; set; }
        public string DocNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string Search { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }
    public class PurchaseSummaryReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public Guid SupplierID { get; set; }
        public string SupplierName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Credit { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public decimal NetDue { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public string OriginCompany { get; set; }
        public string Search { get; set; }
        public string RowType { get; set; }
        public Boolean IncludeInternal { get; set; }
        public Boolean IncludeTax { get; set; }
        public string purchaseSummarySum { get; set; }
        public string purchaseSummaryPaid { get; set; }
        public string purchaseSummaryInvoice { get; set; }
        public List<PurchaseSummaryReportViewModel> purchaseSummaryReportList { get; set; }


    }

    public class PurchaseDetailReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        [Display(Name = "Supplier")]
        public string SupplierCode { get; set; }
        public string InvoiceNo { get; set; }
        public string Date { get; set; }
        public string PaymentDueDate { get; set; }
        public decimal PaymentProcessed { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal TotalInvoice { get; set; }
        public decimal BalanceDue { get; set; }
        public string OriginCompany { get; set; }
        public string Origin { get; set; }
        public string GeneralNotes { get; set; }
        public string SupplierName { get; set; }
        public string AccountHead { get; set; }
        public string EmpName { get; set; }
        public decimal Credit { get; set; }
        public decimal Tax { get; set; }

        [Display(Name = "Account Head")]
        public string AccountCode { get; set; }
        public List<SelectListItem> AccountList { get; set; }
        [Display(Name ="Sub Type")]
        public string SubType { get; set; }
        public List<SelectListItem> SubTypeList { get; set; }
        public string EmpID { get; set; }

        public List<SelectListItem> CompanyList { get; set; }
        public List<SelectListItem> supplierList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public string Search { get; set; }
        public string RowType { get; set; }
        public Boolean IncludeInternal { get; set; }
        public Boolean IncludeTax { get; set; }
        public string purchaseDetailSum { get; set; }
        public string purchaseDetailPaid { get; set; }
        public string purchaseDetailInvoice { get; set; }
        public string purchaseDetailPaymentProcess { get; set; }
        public string purchaseDetailsTaxAmount { get; set; }
        public string purchaseDetailsTotalAmount { get; set; }
        public string InvoiceType { get; set; }
        public List<PurchaseDetailReportViewModel> purchaseDetailReportList { get; set; }

    }

    public class SupplierContactDetailsReportViewModel
    {

        public string CompanyCode { get; set; }
        public string SupplierName { get; set; }
        public string PhoneNumber { get; set; }
        public string OtherPhoneNos { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string Search { get; set; }

    }

    public class PurchaseTransactionLogReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Date { get; set; }
        public string TransactionType { get; set; }
        public string DocNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public string Search { get; set; }
    }

    public class AccountsReceivableAgeingReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
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
        [Display(Name = "Customer")]
        public string CustomerCode { get; set; }
        public List<SelectListItem> customerList;
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }

    public class AccountsReceivableAgeingSummaryReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Customer { get; set; }
        public string Current { get; set; }
        public string OneToThirty { get; set; }
        public string ThirtyOneToSixty { get; set; }
        public string SixtyOneToNinety { get; set; }
        public string NinetyOneAndOver { get; set; }
        [Display(Name = "Customer")]
        public string CustomerCode { get; set; }
        public List<SelectListItem> customerList;
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }

    public class AccountsPayableAgeingReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        [Display(Name = "Supplier")]
        public string SupplierCode { get; set; }
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
        public List<SelectListItem> supplierList { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public ReportAdvanceSearchViewModel reportAdvanceSearchObj { get; set; }
        public string Search { get; set; }
    }

    public class AccountsPayableAgeingSummaryReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode {get;set;}
        [Display(Name = "Supplier")]
        public string SupplierCode { get; set; }
        public string OriginatedCompany {get;set;}
        public string Supplier {get;set;}
        public string Current {get;set;}
        public string OneToThirty {get;set;}
        public string ThirtyOneToSixty {get;set;}
        public string SixtyOneToNinety {get;set;}
        public string NinetyOneAndOver {get;set;}
        public List<SelectListItem> CompanyList{get;set;}
        public List<CompaniesViewModel> companiesList;
        public List<SelectListItem> supplierList { get; set; }
            }



    public class EmployeeExpenseSummaryReportViewModel
    {
        public string EmployeeCode { get; set; }
        public Guid EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCompany { get; set; }
        public string AccountHead { get; set; }
        public decimal Amount { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public List<EmployeeTypeViewModel> employeesList;
    }

    public class EmployeeExpenseDetailReportViewModel
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
        public List<SelectListItem> EmployeeList { get; set; }
        public List<EmployeeTypeViewModel> employeesList;
    }

    public class DepositsAndWithdrawalsDetailsReportViewModel
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
        public string Search { get; set; }
        public BankViewModel bankObj { get; set; }        
    }

    public class OtherIncomeSummaryReportViewModel
    {
        public string CompanyCode { get; set; }
        public string AccountHead { get; set; }
        public List<SelectListItem> AccountHeadList { get; set; }
        public string AccountHeadORSubtype { get; set; }
        public decimal Amount { get; set; }
        public string OriginCompany { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public string Search { get; set; }
        public List<SelectListItem> EmployeeTypeList { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public string Subtype { get; set; }
        [Display(Name = "Employee/Other")]
        public string EmployeeOrOther { get; set; }
        public List<SelectListItem> SubtypeList { get; set; }      
        public string Employee { get; set; }
        public Guid EmployeeID { get; set; }
        public string SubTypeDesc { get; set; }
    }

    public class OtherIncomeDetailsReportViewModel
    {
        public string CompanyCode { get; set; }
        public string AccountHead { get; set; }
        public string Date { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentReference { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string OriginCompany { get; set; }
        public string Company { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public List<SelectListItem> AccountHeadList { get; set; }
        public string Search { get; set; }
        public string RowType { get; set; }
        [Display(Name = "Employee/Other")]
        public string EmployeeOrOther { get; set; }
        public List<SelectListItem> EmployeeTypeList { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public string SubType { get; set; }
        public List<SelectListItem> SubtypeList { get; set; }
        public string AccountHeadORSubtype { get; set; }
        public string Employee { get; set; }
        public Guid EmployeeID { get; set; }
        public string SubTypeDesc { get; set; }

    }
    public class CustomerPaymentLedgerViewModel
    {
        public string Date { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }        
        public string CustomerName { get; set; }
        public string Company { get; set; }
        
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public List<SelectListItem> customerList { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        [Required(ErrorMessage = "Customer required")]
        [Display(Name = "Customer")]
        public string CustomerCode { get; set; }
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public Guid CustomerID { get; set; }        
        public List<SelectListItem> companiesList { get; set; }
        public PDFTools  pdfToolsObj { get; set; }
        [Display(Name = "Invoice Type")]
        public string InvoiceType { get; set; }
    }


    public class SupplierPaymentLedgerViewModel
    {
        public string Date { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }
        public string SupplierName { get; set; }
        public string Company { get; set; }       
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<SelectListItem> supplierList { get; set; }
        [Required(ErrorMessage = "Supplier required")]
        [Display(Name = "Supplier")]
        public string SupplierCode { get; set; }
        public Guid SupplierID { get; set; }        
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public List<SelectListItem> companiesList { get; set; }
        public PDFTools  pdfToolsObj { get; set; }
        [Display(Name = "Invoice Type")]
        public string InvoiceType { get; set; }
    }


    public class TrialBalanceViewModel
    {
        public string Date { get; set; }
        public string Account { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }

    public class DailyLedgerReportViewModel
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
        public string Search { get; set; }
        public string Particulars { get; set; }
        public string BankCode { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public BankViewModel bankObj { get; set; }
        public List<SelectListItem> BanksList { get; set; }
    }

    public class CustomerExpeditingReportViewModel
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
        public string Date { get; set; }
        public CompaniesViewModel companyObj { get; set; }
        public CustomerViewModel customerObj { get; set; }
        public CustomerContactDetailsReportViewModel customerContactObj { get; set; }       
    }

    public class CustomerExpeditingListViewModel {
        public List<CustomerExpeditingReportViewModel> customerExpeditingDetailsList { get; set; }
        [Display(Name = "Filter")]
        public List<SelectListItem> BasicFilters { get; set; }
        public string Filter { get; set; }
        public CompaniesViewModel companyObj { get; set; }
        public CustomerViewModel customerObj { get; set; }        
        public string Search { get; set; }
        public string Company { get; set; }
        public string Customer { get; set; }       
    }

    public class SupplierExpeditingReportViewModel
    {
        public string SupplierName { get; set; }
        public string SupplierName1 { get; set; }
        public string ContactNo { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string OtherPhoneNos { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public Decimal Amount { get; set; }
        public string NoOfDays { get; set; }
        public string Remarks { get; set; }
        public CompaniesViewModel companyObj { get; set; }
        public SupplierContactDetailsReportViewModel supplierContactObj { get; set; }
        public SuppliersViewModel supplierObj { get; set; }
    }

    public class SupplierExpeditingListViewModel
    {
        public List<SupplierExpeditingReportViewModel> SupplierExpeditingDetailsList { get; set; }
        [Display(Name = "Filter")]
        public List<SelectListItem> BasicFilters { get; set; }
        public string Filter { get; set; }
        public CompaniesViewModel companyObj { get; set; }
        public SupplierContactDetailsReportViewModel supplierContactObj { get; set; }
        public SuppliersViewModel supplierObj { get; set; }
        public string Search { get; set; }
        public string Company { get; set; }
        public string Supplier { get; set; }
    }

    public class ReportAdvanceSearchViewModel
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
