using SPAccounts.DataAccessObject.DTO; 
using System.Collections.Generic;
 

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface IDynamicUIRepository
    {
        List<Menu> GetAllMenues();
    }
}
