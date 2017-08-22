using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployees();
        List<EmployeeType> GetAllEmployeeTypes();
        List<EmployeeType> GetAllDepartment();
        List<EmployeeType> GetAllCategory();
        Employee InsertEmployee(Employee _employeeObj);
        object UpdateEmployee(Employee _employeeObj);
        object DeleteEmployee(Guid ID);
        Employee GetEmployeeDetails(Guid ID);

        List<EmployeeCategory> GetAllEmployeeCategories();
        object InsertEmployeeCategory(EmployeeCategory employeeCategory);
        object UpdateEmployeeCategory(EmployeeCategory employeeCategory);
        object DeleteEmployeeCategory(string Code);
    }
}
