using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class CustomerCreditNoteViewModel
    {
        public Guid? ID { get; set; }
        [Display(Name="Credit Note No")]
        [Required(ErrorMessage = "Please enter Credit Note No")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Entered Credit Note is not valid.")]
        [MaxLength(20)]
        public string CreditNoteNo { get; set; }
        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Please select customer")]
        public string CustomerID { get; set; }
        [Display(Name ="Company Name")]
        [Required(ErrorMessage = "Please select company")]
        public string OriginComanyCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        [Display(Name = "Credit Note Date")]
        [Required(ErrorMessage = "Please select credit note date")]
        public string CreditNoteDateFormatted { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Credit Amount")]
        [Required(ErrorMessage = "Please enter credit amount")]
        public decimal CreditAmount { get; set; }
        public decimal AvailableCredit { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public string creditAmountFormatted { get; set; }
        public string adjustedAmountFormatted { get; set; }
        public decimal adjustedAmount { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> CompaniesList { get; set; }
        public List<SelectListItem> CreditList { get; set; }
    }
}