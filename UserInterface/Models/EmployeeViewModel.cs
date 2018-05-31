using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class EmployeeViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Code")]
        [Required(ErrorMessage = "Please Enter Code")]
        [MaxLength(10)]
        //[RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Entered Code is not valid.")]
        public string Code { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please Enter Name")]
        [MaxLength(100)]
        //[RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Entered Name is not valid.")]
        public string Name { get; set; }
        [Display(Name = "Mobile No.")]
        [MaxLength(50)]
        public string MobileNo { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        public EmployeeTypeViewModel employeeTypeObj { get; set; }
        [Display(Name = "Employee Type")]
        [Required(ErrorMessage = "Please Select Employee Type")]
        public string EmployeeType { get; set; }
        [Display(Name = "Photo")]
        public string ImageURL { get; set; }
        public CompaniesViewModel companies { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please Select Company")]
        public string companyID { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public string Department { get; set; }
        public string EmployeeCategory { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> CompaniesList { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> EmployeeTypeList { get; set; }
        [Display(Name ="Is Active(Y/N)")]
        public bool IsActive { get; set; }       
        public string Filter { get; set; }
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }       
        public DateTime JoiningDate { get; set; }
        [Display(Name = "Date Of Joining")]
        public string JoiningDateFormatted { get; set; }
    }
    public class EmployeeTypeViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

    }

    public class SubTypeNarrationViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Code")]
        [Required(ErrorMessage = "Please Enter Code")]
        [MaxLength(10)]
        //[RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Entered Code is not valid.")]
        public string Code { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please Enter Name")]
        [MaxLength(100)]
        //[RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Entered Name is not valid.")]
        public string Name { get; set; }
        [Display(Name = "Mobile No.")]
        [MaxLength(50)]
        public string MobileNo { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        public EmployeeTypeViewModel employeeTypeObj { get; set; }
        [Display(Name = "Employee Type")]
        [Required(ErrorMessage = "Please Select Employee Type")]
        public string EmployeeType { get; set; }
        [Display(Name = "Photo")]
        public string ImageURL { get; set; }
        public CompaniesViewModel companies { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please Select Company")]
        public string companyID { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public string Department { get; set; }
        public string EmployeeCategory { get; set; }
        public CommonViewModel commonObj { get; set; }
        public List<SelectListItem> CompaniesList { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> EmployeeTypeList { get; set; }
    }

    public class EmployeeCategoryViewModel
    {
        [Display(Name = "Code")]
        [Required(ErrorMessage = "Please Enter Code")]
        [MaxLength(10)]
        public string Code { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please Enter Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        public CommonViewModel commonObj { get; set; }
        public string Operation { get; set; }
    }
}