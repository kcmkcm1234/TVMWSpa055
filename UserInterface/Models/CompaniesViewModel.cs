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
        //public Guid ID { get; set; }
        [Required(ErrorMessage = "Company is missing")]
        [Display(Name = "Originated Company")]
        public string Code { get; set; }
        public string Name { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        
        //public string isUpdate { get;set;}
        public Guid ApproverID { get; set; }
       // public Guid UserID { get; set; }
        public UserViewModel userObj { get; set; }
        //public string hdnCode { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public string LogoURL { get; set; }

    }
}