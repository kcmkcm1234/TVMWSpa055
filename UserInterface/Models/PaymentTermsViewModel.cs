using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class PaymentTermsViewModel
    {
        [Required(ErrorMessage = "Payment Term is missing")]
        [Display(Name = "Payment Term")]
        public string Code { get; set; }
        public string Description { get; set; }
        public int NoOfDays { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> PaymentTermsList { set; get; }
    }
}