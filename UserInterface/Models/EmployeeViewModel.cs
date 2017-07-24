using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class EmployeeViewModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public EmployeeTypeViewModel employeeType { get; set; }
        public string ImageURL { get; set; }
        public CompaniesViewModel companies { get; set; }
        public string companyID { get; set; }
        public string GeneralNotes { get; set; }
        public CommonViewModel commonObj { get; set; }
    }
    public class EmployeeTypeViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

    }
}