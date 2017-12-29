using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class CustomerPaymentsViewModel
    {

        public Guid ID { get; set; }
        public string EntryNo { get; set; } 
       
        [Display(Name = "Payment/Credit Adjustment")]
        public string Type { get; set; }

        [Display(Name = "Credit Note To Adjust")]
        public Guid CreditID { get; set; }
        public string CreditNo { get; set; }
        public string hdfType { get; set; }
        public string hdfCreditID { get; set; }

        [Required(ErrorMessage = "Received To Company is missing")]
        [Display(Name = "Received To Company")]
        public string RecdToComanyCode { get; set; }

        [Required(ErrorMessage = "Payment Mode is missing")]
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }

        public Guid DepWithdID { get; set; }

        [Display(Name = "Deposit To")]
        public string BankCode { get; set; }

        [Display(Name = "Reference No.")]
        public string PaymentRef { get; set; }

        [Required(ErrorMessage = "Payment Date is missing")]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Cheque Date is missing")]
        [Display(Name = "Cheque Date")]
        public string ChequeDate { get; set; }

        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name ="Reference Bank")]
        public string ReferenceBank { get; set; }
        [Required(ErrorMessage = "Amount Received is missing")]
        [Display(Name = "Amount Received")]
        public decimal TotalRecdAmt { get; set; }
        public decimal AdvanceAmount { get; set; }
        public Guid hdnFileID { get; set; }
        public string PaymentDateFormatted { get; set; }
        public List<CustomerPaymentsDetailViewModel> CustomerPaymentsDetail { get; set; }
        public CustomerPaymentsDetailViewModel CustPaymentDetailObj { get; set; }
        public List<SelectListItem> customerList { get; set; }

        public CustomerViewModel customerObj { get; set; }
        public CommonViewModel commonObj { get; set; }
        [Display(Name = "Payment Mode")]
        public PaymentModesViewModel PaymentModesObj { get; set; }
        public BankViewModel bankObj { get; set; }
        [Display(Name = "Recieved to Company")]
        public CompaniesViewModel CompanyObj { get; set; }
        public CustomerCreditNoteViewModel CreditObj { get; set; }

        public string paymentDetailhdf { get; set; }
        public string hdfCustomerID { get; set; }
        public string hdfCreditAmount { get; set; }
        public string OutstandingAmount { get; set; }
        public string InvoiceOutstanding { get; set; }
        public string CreditOutstanding { get; set; }
        public string AdvOutstanding { get; set; }
        public string PaymentOutstanding { get; set; }
        public string Search { get; set; }

    }
    public class CustomerPaymentsDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid InvoiceID { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal OriginalInvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal PaidAmount { get; set; }

        public string PaymentDueDateFormatted { get; set; }
        public CustomerPaymentsDetailViewModel CustPaymentDetailObj { get; set; }

    }
    public class CustomerPaymentsSearchViewModel
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Company { get; set; }
        public string PaymentMode { get; set; }
        public string Customer { get; set; }
        public string Search { get; set; }
    }
}