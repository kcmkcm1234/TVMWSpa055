using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface IDepartmentRepository
    {
        List<Department> GetAllDetpartments();
        object InsertDepartment(Department _DepartmentObj);
        object UpdateDepartment(Department _DepartmentObj);
        object DeleteDepartment(string Code);

    }
}
