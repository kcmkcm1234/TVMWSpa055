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
        public Guid? SupplierID { get; set; }
        public List<SelectListItem> SupplierList { get; set; }

        [Display(Name = "Supplier Address")]
        public string SupplierAddress { get; set; }
        
        public string CompanyCode { get; set; }

        [Display(Name = "Credit Note Number")]
        public string CRNRefNo { get; set; }

       
        public string CRNDate { get; set; }

        [Display(Name = "Credit Note Date")]
        public string CRNDateFormatted { get; set; }

        [Display(Name = "Credit Amount")]
        public decimal Amount { get; set; }

        public string Type { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public SuppliersViewModel supplier { get; set; }
        public CompaniesViewModel Company { get; set; }
        public CommonViewModel commonObj { get; set; }
    }
}