using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class TaxTypesViewModel
    {
        [Display(Name = "Code")]
        [MaxLength(10)]
        //[Required(ErrorMessage = "Code is required")]
        //[RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Entered code is not valid.")]
        public string Code { get; set; }
        [Display(Name = "Description")]
        [MaxLength(50)]
        public string Description { get; set; }
        [Display(Name = "Rate (₹)")]
        [Range(0, 999.99)]
        public decimal Rate { get; set; }
        public CommonViewModel commonObj { get; set; }
        public PaymentTermsViewModel PaymentTermsObj { get; set; }
        public List<SelectListItem> TaxTypesList { get; set; }
        public string hdnCode { get; set; }
        public string isUpdate { get; set; }
    }
}