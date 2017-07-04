using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class CustomerViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company Name is missing")]
        public string CompanyName { get; set; }
        [Display(Name = "Contact Person Name")]
        public string ContactPerson { get; set; }
        [Display(Name = "Email")]
        public string ContactEmail { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        [Display(Name = "Website")]
        public string Website { get; set; }
        [Display(Name = "Phone")]
        public string LandLine { get; set; }
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "Other Number(s) if any")]
        public string OtherPhoneNos { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Default Payment Term")]
        public string PaymentTermCode { get; set; }
        public List<SelectListItem> DefaultPaymentTermList { get; set; }
        [Display(Name = "Tax Registration Number")]
        public string TaxRegNo { get; set; }
        [Display(Name = "Pan Number")]
        public string PANNO { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public CommonViewModel commonObj { get; set; }
    }
}