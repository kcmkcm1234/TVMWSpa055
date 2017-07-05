using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class CustomerPaymentsViewModel
    {

        public Guid ID { get; set; }
        public string EntryNo { get; set; }

        [Display(Name = "Received To Company")]
        public string RecdToComanyCode { get; set; }

        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }

        public Guid DepWithdID { get; set; }

        [Display(Name = "Deposit To")]
        public string BankCode { get; set; }

        [Display(Name = "Reference No.")]
        public string PaymentRef { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }

        public decimal TotalRecdAmt { get; set; }

        public string PaymentDateFormatted { get; set; }
        public List<CustomerPaymentsDetailViewModel> CustomerPaymentsDetail { get; set; }
        

        public CustomerViewModel customerObj { get; set; }
        public CommonViewModel commonObj { get; set; }
        public PaymentModesViewModel PaymentModesObj { get; set; }
        public BankViewModel bankObj { get; set; }

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

    }
}