using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IDepartmentBusiness
    {
        List<Department> GetAllDetpartments();
        Department GetDepartmentDetails(string Code);
   
        object InsertDepartment(Department department);
        object UpdateDepartment(Department department);
        object DeleteDepartment(string Code);
    }
}
