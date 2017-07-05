using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class CompaniesViewModel
    {
        [Required(ErrorMessage = "Company is missing")]
        [Display(Name = "Originated Company")]
        public string Code { get; set; }
        public string Name { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
    }
}