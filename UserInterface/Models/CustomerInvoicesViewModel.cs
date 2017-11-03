using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace UserInterface.Models
{
    public class CustomerInvoiceBundleViewModel {
        public List<CustomerInvoicesViewModel> CustomerInvoices { get; set; }
        public CustomerInvoiceSummaryViewModel CustomerInvoiceSummary { get; set; }

    }

    public class CustomerInvoicesViewModel
    {
        public Guid ID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Invoice Date is missing")]
        [Display(Name = "Invoice Date")]
        public String InvoiceDateFormatted { get; set; }

        [Required(ErrorMessage = "Invoice No is missing")]
        [Display(Name = "Invoice Number")]
        [MaxLength(20)]
        public String InvoiceNo { get; set; }
        public int DueDays { get; set; }
        public string hdfCustomerID { get; set; }
        public CompaniesViewModel companiesObj { get; set; }
        public CustomerViewModel customerObj { get; set; }        
        public TaxTypesViewModel TaxTypeObj { get; set; }
        public PaymentTermsViewModel paymentTermsObj { get; set; }
        public CommonViewModel commonObj { get; set; }

        [Required(ErrorMessage = "Billing Address is missing")]
        [Display(Name = "Billing Address")]
        [DataType(DataType.MultilineText)]
        public String BillingAddress { get; set; }

        public DateTime PaymentDueDate { get; set; }

        [Required(ErrorMessage = "Payment Due Date is missing")]
        [Display(Name = "Payment Due Date")]
        public String PaymentDueDateFormatted { get; set; }

        [Required(ErrorMessage = "Gross Amount is missing")]
        [Display(Name = "Gross Amount")]
        public decimal GrossAmount { get; set; }

        [Required(ErrorMessage = "Discount Amount is missing")]
        [Display(Name = "Discount Amount")]
        public decimal Discount { get; set; }
        
        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Net Taxable Amount")]
        public decimal NetTaxableAmount { get; set; }

        [Display(Name ="Tax Percentage Applied")]
        public decimal TaxPercApplied { get; set; }

        [Display(Name = "Total Invoice Amount")]
        public decimal TotalInvoiceAmount { get; set; }

        [Display(Name = "General Notes")]
        [DataType(DataType.MultilineText)]
        public String Notes { get; set; }

        public string BalanceDuestring { get; set; }
        public string TotalInvoiceAmountstring { get; set; }
        public string PaidAmountstring { get; set; }
        public Guid hdnFileID { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OtherPayments { get; set; }
        public CustomerPaymentsViewModel CustPaymentObj { get; set; }

        public DateTime LastPaymentDate { get; set; }
        public String LastPaymentDateFormatted { get; set; }
        public String Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string Search { get; set; }
        [Display(Name = "Invoice Type")]
        public string InvoiceType { get; set; }


        [Display(Name = "Payment Status")]
        public string SpecialPayStatus { get; set; }
        

        [Required(ErrorMessage = "Discount Amount is missing")]
        [Display(Name = "Invoice Reference")]
        public Guid? RefInvoice { get; set; }
        public List<SelectListItem> InvoiceList { get; set; }

        public SpecialPaymentViewModel SpecialPayObj { get; set; }

    }

    public class SpecialPaymentViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Paid Amount is missing")]
        [Display(Name = "Paid Amount")]
        public decimal SpecialPaidAmount { get; set; }
        [Required(ErrorMessage = "Payment Date is missing")]
        [Display(Name = "Payment Date")]
        public string SpecialPaymentDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Reference No.")]
        public string PaymentRef { get; set; }

        [Display(Name = "Reference Bank")]
        public string RefBank { get; set; }

        [Required(ErrorMessage = "Payment Mode is missing")]
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }

        [Display(Name = "Cheque Date")]
        public string ChequeDate { get; set; }

        public PaymentModesViewModel PaymentModesObj { get; set; } 
        public List<SelectListItem> SpecialPaymentList { get; set; }
    }

    public class CustomerInvoiceSummaryViewModel
    {
        public decimal OverdueAmount { get; set; }
        public string OverdueAmountFormatted { get; set; }
        public int OverdueInvoices { get; set; }

        public decimal OpenAmount { get; set; }
        public string OpenAmountFormatted { get; set; }
        public int OpenInvoices { get; set; }

        public decimal PaidAmount { get; set; }
        public string PaidAmountFormatted { get; set; }
        public int PaidInvoices { get; set; }

    }

    public class CustomerInvoicesSummaryForMobileViewModel
    {
        public InvoiceSummaryformobileViewModel CustInvSumObj { get; set; }
        public List<CustomerInvoice> CustInv { get; set; }
    }

    public class InvoiceSummaryformobileViewModel
    {
        public decimal Amount { get; set; }
        public string AmountFormatted { get; set; }
        public int count { get; set; }
      
    }


    public class CustomerInvoiceAgeingSummaryViewModel
    {
        public int total;
        public int Todays;
        public int Count1To30;
        public int Count31To60;
        public int Count61To90;
        public int Count91Above;
        public int ThisWeek;


    }
}