using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class TaxTypeViewModel
    {
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Rate")]
        public decimal Rate { get; set; }
        public CommonViewModel commonObj { get; set; }
    }
}