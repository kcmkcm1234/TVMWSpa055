using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class OtherIncomeViewModel
    {

        public Guid ID { get; set; }
        public string AccountDesc { get; set; }
        public DateTime? IncomeDate { get; set; }
        [Display(Name = "Account")]
        [Required(ErrorMessage ="Please Select Account")]
        public string AccountCode { get; set; }
        [Display(Name = "Payment Received Company")]
        [Required(ErrorMessage = "Please Select Payment Received Company")]
        public string PaymentRcdComanyCode { get; set; }
        [Display(Name = "Payment Mode")]
        [Required(ErrorMessage ="Please Select Payment Mode")]
        public string PaymentMode { get; set; }
        public Guid DepWithdID { get; set; }
        [Display(Name = "Bank")]
        public string BankCode { get; set; }
        [Display(Name = "Income Reference")]
        [MaxLength(20)]
        public string IncomeRef { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Amount")]
        [Required(ErrorMessage ="Please Enter An Amount")]
        public decimal Amount { get; set; }
        public int slNo { get; set; }
        [Display(Name = "Income Date")]
        [Required(ErrorMessage = "Please Select Income Date")]
        public string IncomeDateFormatted { get; set; }
        public List<SelectListItem> accountCodeList { get; set; }
        public List<SelectListItem> bankList { get; set; }
        public List<SelectListItem> companiesList { get; set; }
        public List<SelectListItem> paymentModeList { get; set; }
        public CommonViewModel commonObj { get; set; }
    }
}