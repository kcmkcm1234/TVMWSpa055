using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class DepositAndWithdrwalViewModel
    {
        public Guid ID { get; set; }

        public string TransactionType { get; set; }
        [Display(Name = "Reference No.")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Reference Number Missing")]
        public string ReferenceNo { get; set; }
        [Required(ErrorMessage = "Date is missing")]
        public string Date { get; set; }
        [Display(Name = "Start Date")]
        public string FromDate { get; set; }
        [Display(Name = "End Date")]
        public string ToDate { get; set; }
        public decimal Amount { get; set; }
        [Display(Name = "Bank")]
        [Required(ErrorMessage = "Bank Missing")]
        public string BankCode { get; set; }
        public string GeneralNotes { get; set; }
        public string DateFormatted { get; set; }
        public CommonViewModel CommonObj { get; set; }
        public List<SelectListItem> bankList { get; set; }
        public string BankName { get; set; }
        public string DepositRowValues { get; set; }
        public List<DepositAndWithdrwalViewModel> CheckedRows { get; set; }
        public string DepositMode { get; set; }
        [Display(Name = "Cheque Status")]
        public string ChequeStatus { get; set; }
        [Display(Name = "Deposit Mode")]
        public string PaymentMode { get; set; }
        public List<SelectListItem> paymentModeList { get; set; }
    }
}