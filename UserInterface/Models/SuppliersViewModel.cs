using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class SuppliersViewModel
    {

        [Required(ErrorMessage = "Supplier is missing")]
        [Display(Name = "Supplier")]
        public Guid ID { get; set; }
        [Display(Name = "Company Name")]
        [MaxLength(150)]
        [Required(ErrorMessage = "Company Name is missing")]
        public string CompanyName { get; set; }
        [Display(Name = "Contact Person Name")]
        [MaxLength(100)]
        public string ContactPerson { get; set; }
        [Display(Name = "Email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Entered email is not valid.")]
        [MaxLength(150)]
        public string ContactEmail { get; set; }

        [Display(Name = "Product Name")]
        [MaxLength(250)]
        public string Product { get; set; }

        [Display(Name = "Title")]
        [MaxLength(10)]
        public string ContactTitle { get; set; }
        [Display(Name = "Website")]
        [MaxLength(500)]
        public string Website { get; set; }
        [Display(Name = "Phone")]
        [StringLength(50, MinimumLength = 5)]
        public string LandLine { get; set; }
        [Display(Name = "Mobile")]
       
        [StringLength(50, MinimumLength = 5)]
        public string Mobile { get; set; }
        [Display(Name = "Fax")]
        [StringLength(50, MinimumLength = 5)]
        public string Fax { get; set; }
        [Display(Name = "Other Number(s) if any")]
        [MaxLength(250)]
        public string OtherPhoneNos { get; set; }
        [Display(Name = "Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Default Payment Term")]
        [MaxLength(10)]
        public string PaymentTermCode { get; set; }
        public List<SelectListItem> DefaultPaymentTermList { get; set; }
        [Display(Name = "Tax Registration Number")]
        [MaxLength(50)]
        public string TaxRegNo { get; set; }
        [Display(Name = "Pan Number")]
        [MaxLength(50)]
        public string PANNO { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Available Advance Amount:")]
        public decimal AdvanceAmount { get; set; }
        public decimal OutStanding { get; set; }
        public decimal PaidAmount { get; set; }
        public CommonViewModel commonObj { get; set; }
        public PaymentTermsViewModel PaymentTermsObj { get; set; }
        public List<SelectListItem> SupplierList { get; set; }
        public List<SelectListItem> TitlesList { get; set; }
    }
}