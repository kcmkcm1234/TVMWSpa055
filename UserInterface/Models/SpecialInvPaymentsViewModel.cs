using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class SpecialInvPaymentsViewModel
    {
        public Guid ID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid GroupID { get; set; }
        [Display(Name = "Customer")]
        [Required(ErrorMessage = "Customer is missing")]     
        public Guid CustID { get; set; }

        [Display(Name = "Payment Date")]
        [Required(ErrorMessage = "Payment Date is missing")]      
        public DateTime PaymentDate { get; set; }
       

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }


        [Display(Name = "Reference No.")]      
        public string PaymentRef { get; set; }


        [Display(Name = "Reference Bank")]
        public string RefBank { get; set; }

        [Display(Name = "Payment Mode")]
        [Required(ErrorMessage = "Payment Mode is missing")]        
        public string PaymentMode { get; set; }

        [Display(Name = "Cheque Date")]
        public DateTime? ChequeDate { get; set; }
       

        public CustomerViewModel customerObj { get; set; }

        public decimal InvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public string PaymentDueDate { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        [Display(Name = "Received To Comapny")]
        public CompaniesViewModel companiesObj { get; set; }

        [Display(Name = "Payment Mode")]
        public PaymentModesViewModel PaymentModesObj { get; set; }
        //public List<SelectListItem> SpecialPaymentList { get; set; }

        //  public decimal CurrentAmount { get; set; }
        public string paymentDateFormatted { get; set; }
        public string chequeDateFormatted { get; set; }
        public string hdfCustomerID { get; set; }
        public string hdfPaymentID { get; set; }

        public string InvoiceOutstanding { get; set; }
        public string BalanceOutstanding { get; set; }
        public string DetailXml { get; set; }
        public string hdfpaymentDetail { get; set; }
        public CommonViewModel commonObj { get; set; }
        public SpecialInvPaymentsDetailViewModel specialDetailObj { get; set; }
        public List<SpecialInvPaymentsDetailViewModel> specialList { get; set; }
        public string Search { get; set; }
    }
    public class SpecialInvPaymentsDetailViewModel
    {
        public Guid InvoiceID { get; set; }
        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; }
        public decimal CurrentAmount { get; set; }      
    }

    public class SpecialPaymentsSearchViewModel
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Company { get; set; }
        public string PaymentMode { get; set; }
        public string Customer { get; set; }
        public string Search { get; set; }
    }
}