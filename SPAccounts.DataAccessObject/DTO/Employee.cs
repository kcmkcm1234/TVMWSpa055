using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class Employee
    {
         public Guid ID { get; set; }
         public string Code { get; set; }
         public string Name { get; set; }
         public string MobileNo { get; set; }
         public string Address { get; set; }
         public EmployeeType employeeTypeObj { get; set; }
         public string EmployeeType { get; set; } 
         public string ImageURL { get; set; }
         public Companies companies { get; set; }
         public string companyID { get; set; }
         public string Department { get; set; }
         public string EmployeeCategory { get; set; }
         public string GeneralNotes { get; set; }
         public Common commonObj { get; set; }
    }
    public class EmployeeType
    {
        public string Code { get; set; }
        public string Name { get; set; }

    }
}