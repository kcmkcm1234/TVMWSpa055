using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class SupplierCreditNoteViewModel
    {
        public Guid? ID { get; set; }

        [Display(Name = "Supplier")]
        [Required(ErrorMessage = "Please Select Supplier")]
        public Guid SupplierID { get; set; }
        public List<SelectListItem> SupplierList { get; set; }
        public List<SelectListItem> CompaniesList { get; set; }
        [Display(Name ="Credit To Company")]
        [Required(ErrorMessage = "Please Select Company")]
        public string CreditToComanyCode { get; set; }
        public string creditAmountFormatted { get; set; }
        public string adjustedAmountFormatted { get; set; }
        [Display(Name = "Supplier Address")]
        public string SupplierAddress { get; set; }
        
        public string CompanyCode { get; set; }

        [Display(Name = "Credit Note Number")]
        [Required(ErrorMessage = "Please enter credit Note Number")]
        //[RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Entered Credit Note is not valid.")]
        [MaxLength(20)]
        public string CRNRefNo { get; set; }

       
        public string CRNDate { get; set; }

        [Display(Name = "Credit Note Date")]
        [Required(ErrorMessage = "Please Select Credit Note Date")]
        public string CRNDateFormatted { get; set; }

        
        public decimal Amount { get; set; }
        public decimal AvailableCredit { get; set; }


        public string Type { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public SuppliersViewModel supplier { get; set; }
        public CompaniesViewModel Company { get; set; }
        public CommonViewModel commonObj { get; set; }
        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }
        [Display(Name = "Credit Amount")]
        [Required(ErrorMessage = "Please Enter Credit Amount")]
        public decimal CreditAmount { get; set; }
    }
}