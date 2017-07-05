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
        [Display(Name = "Tax Type")]
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> TaxTypesList { get; set; }
    }
}