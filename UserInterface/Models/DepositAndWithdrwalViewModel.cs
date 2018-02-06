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
        [Display(Name = "Cheque Clear Date")]
        [Required(ErrorMessage = "Cheque Clear Date is missing")]
        public string ChequeClearDate { get; set; }
        [Display(Name = "Start Date")]
        public string FromDate { get; set; }
        [Display(Name = "End Date")]
        public string ToDate { get; set; }
        public decimal Amount { get; set; }
        [Display(Name = "Deposit To")]
        [Required(ErrorMessage = "Bank Missing")]
        public string BankCode { get; set; }
        public string GeneralNotes { get; set; }
        public string DateFormatted { get; set; }
        public string ChequeFormatted { get; set; }
        public CommonViewModel CommonObj { get; set; }
        public List<SelectListItem> bankList { get; set; }
        public string BankName { get; set; }
        public string DepositRowValues { get; set; }
        public List<DepositAndWithdrwalViewModel> CheckedRows { get; set; }
        public string DepositMode { get; set; }
        public string hdnChequeStatus { get; set; }
        public string hdnChequeDate { get; set; }
        [Display(Name = "Cheque Status")]
        [Required(ErrorMessage = "Cheque Status is missing")]
        public string ChequeStatus { get; set; }
        [Display(Name = "Deposit Mode")]
        [Required(ErrorMessage = "Deposit Mode is missing")]
        public string PaymentMode { get; set; }
        public List<SelectListItem> paymentModeList { get; set; }
        public string ReferenceBank { get; set; }
        public string UndepositedChequeCount { get; set; }
        public string ChequeDate { get; set; }
        public string CustomerName { get; set; }
        [Display(Name = "Customer Name")]
        public Guid? CustomerID { get; set; }
        public List<SelectListItem> customerList { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        [Display(Name = "From Bank")]
        public string FromBank { get; set; }
        [Display(Name = "To Bank")]
        public string ToBank { get; set; }
        public Guid TransferID { get; set; }
        public OutgoingChequeAdvanceSearchViewModel OutAdvanceSearch { get; set; }
        public OutGoingChequesViewModel OutGoingObj { get; set; }
        public IncomingChequesViewModel IncomingObj { get; set; }
        public CompaniesViewModel CompanyObj { get; set; }
        public UndepositedChequeAdvanceSearchViewModel undepositedChequeAdvanceSearchObj { get; set; }
        public CustomerViewModel CustomerObj { get; set; }
        public string Company { get; set; }
        public string Customer { get; set; }
        public BankViewModel BankObj { get; set; }
        public string Bank { get; set; }
        public string Search { get; set; }
    }

    public class OutGoingChequesViewModel
    { 
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Cheque Date Missing")]
        [Display(Name = "Cheque Date")]
        public string ChequeDate { get; set; }
        [Required(ErrorMessage = "Cheque No Missing")]
        [Display(Name = "Cheque No")]
        public string ChequeNo { get; set; }
        public string Bank { get; set; }
        public string Party { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Company { get; set; }
        public string Remarks { get; set; }
        public string CreatedDate { get; set; }
        public CommonViewModel CommonObj { get; set; }
    }

    public class IncomingChequesViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Cheque Date Missing")]
        [Display(Name = "Cheque Date")]
        public string ChequeDate { get; set; }
        [Required(ErrorMessage = "Cheque No Missing")]
        [Display(Name = "Cheque No")]
        public string ChequeNo { get; set; }
        public string Bank { get; set; }
        public Guid Customer { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Company { get; set; }
        public string Remarks { get; set; }
        public string CreatedDate { get; set; }
        public CommonViewModel CommonObj { get; set; }
        public CustomerViewModel customerObj { get; set; }
    }
    public class OutgoingChequeAdvanceSearchViewModel
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public string Search { get; set; }
        public Guid Customer { get; set; }
        public string CustomerName { get; set; }
    }

    public class UndepositedChequeAdvanceSearchViewModel
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Company { get; set; }
        public string Customer { get; set; }
        public string BankCode { get; set; }
        public string Search { get; set; }
    }
}