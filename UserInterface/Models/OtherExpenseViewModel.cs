using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class OtherExpenseViewModel
    {
        [Display(Name = "Date")]
        public string CRNDateFormatted { get; set; }
        public Guid ID { get; set; }
        public string ExpenseDate { get; set; }

        public string PaidFromCompanyCode { get; set; }
        [Required(ErrorMessage = "Choose company")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public CompaniesViewModel companies { get; set; }
        public EmployeeViewModel employee { get; set; }
        public Guid EmpID { get; set; }
        public string PaymentMode { get; set; }
        public Guid DepWithID { get; set; }
        public DepositAndWithdrwalViewModel depositAndWithdrwal { get; set; }
        public string BankCode { get; set; }
        public string ExpenseRef { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Choose Account Type")]
        [Display(Name = "Account")]
        public Guid AccountCode { get; set; }
        public List<SelectListItem> AccountTypes { get; set; }
        public CommonViewModel commonObj { get; set; }
    }
}