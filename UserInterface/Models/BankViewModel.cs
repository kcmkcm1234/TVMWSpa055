using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class BankViewModel
    {
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Company Code")]
        public string CompanyCode { get; set; }
        public CommonViewModel commonObj { get; set; }
    }
}