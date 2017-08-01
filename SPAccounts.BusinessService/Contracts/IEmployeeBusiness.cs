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
        List<Employee> GetAllEmployees();
        List<EmployeeType> GetAllEmployeeTypes();
        Employee GetEmployeeDetails(Guid ID);
        object InsertUpdateEmployee(Employee _employeeObj);
        object DeleteEmployee(Guid ID);
    }
}
