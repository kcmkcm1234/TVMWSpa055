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
        //[RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Entered code is not valid.")]
        public string Code { get; set; }
        public string hdnCode { get; set; }
        [Display(Name = "Name")]
        [MaxLength(100)]
        //[RegularExpression(@"^[A-Za-z0-9? ,_-]+$", ErrorMessage = "Entered name is not valid.")]
        [Required(ErrorMessage = "Company Name")]
        public string Name { get; set; }
        [Display(Name = "Company")]
        [MaxLength(10)]
        public string CompanyCode { get; set; }
        [Display(Name = "Actual OD Limit")]
        [Required(ErrorMessage = "Actual OD Limit")]
        public decimal? ActualODLimit { get; set; }
        [Display(Name = "OD Limit")]
        public decimal? DisplayODLimit { get; set; }
        public bool ShowODLimit { get; set; }
        public CommonViewModel commonObj { get; set; }
        [Display(Name ="Bank")]
        public List<SelectListItem> BanksList { get; set; }
        public List<SelectListItem> CompaniesList { get; set; }
        public string isUpdate { get; set; }
        public CompaniesViewModel Company { get; set; }
    }
}