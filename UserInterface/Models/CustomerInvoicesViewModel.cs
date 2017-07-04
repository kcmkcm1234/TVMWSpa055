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

        [Required(ErrorMessage = "Invoice Date is missing")]
        [Display(Name = "Invoice Date")]
        public String InvoiceDateFormatted { get; set; }

        [Required(ErrorMessage = "Invoice No is missing")]
        [Display(Name = "Invoice Number")]
        [MaxLength(20)]
        public String InvoiceNo { get; set; }

        public string OriginCompanyCode { get; set; }

        [Required(ErrorMessage = "Customer is missing")]
        [Display(Name = "Customer")]
        public String Customer { get; set; }

        public CustomerViewModel customerObj { get; set; }
        

        [Required(ErrorMessage = "Payment Term is missing")]
        [Display(Name = "Payment Term")]      
        public int PaymentTerm { get; set; }
        public PaymentTermsVieModel paymentTermsObj { get; set; }

        [Required(ErrorMessage = "Company is missing")]
        [Display(Name = "Originated Company")]
        public String Company { get; set; }

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

        
        [Display(Name = "Tax Type")]
        public String TaxType { get; set; }
        public TaxTypesViewModel taxTypesObj { get; set; }
        
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

        public decimal BalanceDue { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public String LastPaymentDateFormatted { get; set; }
        public String Status { get; set; }

        public List<SelectListItem> TaxList { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public List<SelectListItem> PaymentTermList { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
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
}