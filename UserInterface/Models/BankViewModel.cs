using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class BankViewModel
    {
        [Display(Name = "Code")]
        [MaxLength(5)]
        [Required(ErrorMessage ="Code is required")]
        public string Code { get; set; }
        public string hdnCode { get; set; }
        [Display(Name = "Name")]
        [MaxLength(100)]
        [Required(ErrorMessage = "Company Name")]
        public string Name { get; set; }
        [Display(Name = "Company")]       
        public string CompanyCode { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> BanksList { get; set; }
        public List<SelectListItem> CompaniesList { get; set; }
        public string isUpdate { get; set; }
    }
}