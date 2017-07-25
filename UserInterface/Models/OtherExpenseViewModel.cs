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
        [Required(ErrorMessage = "Expense date required")]
        [Display(Name = "Date")]
        public string ExpenseDate { get; set; }
        public Guid ID { get; set; }
       
        public string CRNDateFormatted { get; set; }


        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string PaidFromCompanyCode { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public CompaniesViewModel companies { get; set; }
       
        [Required(ErrorMessage = "Payment required")]
        [Display(Name = "Payment mode")]
        public string PaymentMode { get; set; }
        public List<SelectListItem> paymentModeList { get; set; }
        public Guid DepWithID { get; set; }
        public DepositAndWithdrwalViewModel depositAndWithdrwal { get; set; }
        [Required(ErrorMessage = "Bank required")]
        [Display(Name = "Bank")]
        public string BankCode { get; set; }
        public List<SelectListItem> bankList { get; set; }
        public string ExpenseRef { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Account Type required")]
        [Display(Name = "Account")]
        public string AccountCode { get; set; }
        public List<SelectListItem> AccountTypes { get; set; }

        [Required(ErrorMessage = "Employee required")]
        [Display(Name = "Employee")]
        public Guid EmpID { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public EmployeeViewModel employee { get; set; }
       

        public CommonViewModel commonObj { get; set; }
    }
}