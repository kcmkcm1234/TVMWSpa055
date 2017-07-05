using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class PaymentTermsVieModel
    {

        [Required(ErrorMessage = "Payment Term is missing")]
        [Display(Name = "Payment Term")]
        public string Code { get; set; }
        public string Description { get; set; }
        public int NoOfDays { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> PaymentTermsList { set; get; }
    }
    public class TaxTypesViewModel
    {
        [Display(Name = "Tax Type")]
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> TaxTypesList { get; set; }
    }
    public class CompaniesViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public CommonViewModel commonObj { get; set; }
    }

    public class TransactionTypesViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

}