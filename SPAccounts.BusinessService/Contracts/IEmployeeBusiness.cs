using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
   public interface IEmployeeBusiness
    {
        List<Employee> GetAllEmployees(string filter);
        List<Employee> GetAllOtherEmployees(string filter);
        List<EmployeeType> GetAllEmployeeTypes();
        List<EmployeeType> GetAllDepartment();
        List<EmployeeType> GetAllCategory();
        Employee GetEmployeeDetails(Guid ID);
        object InsertUpdateEmployee(Employee _employeeObj);
        object DeleteEmployee(Guid ID);


        List<EmployeeCategory> GetAllEmployeeCategories();
        EmployeeCategory GetEmployeeCategoryDetails(string Code);
        object InsertEmployeeCategory(EmployeeCategory employeeCategory);
        object UpdateEmployeeCategory(EmployeeCategory employeeCategory);
        object DeleteEmployeeCategory(string Code);
    }
}
