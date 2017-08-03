using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class SystemReportViewModel
    {
        public Guid AppID { get; set; }
        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReportGroup { get; set; }
        public int GroupOrder { get; set; }
        public string SPName { get; set; }
        public string SQL { get; set; }
        public int ReportOrder { get; set; }
    }
    public class SaleSummaryViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string CustomerName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Paid { get; set; }
        public decimal NetDue { get; set; }
        public string OriginCompany { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }
}