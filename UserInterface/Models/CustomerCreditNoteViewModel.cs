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
        public string CreditNoteNo { get; set; }
        [Display(Name = "Credit Name")]
        public Guid? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string OriginComanyCode { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        [Display(Name = "Credit Note Date")]
        public string CreditNoteDateFormatted { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Credit Amount")]
        public decimal? CreditAmount { get; set; }
        public string Type { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public CommonViewModel commonObj { get; set; }
    }
}