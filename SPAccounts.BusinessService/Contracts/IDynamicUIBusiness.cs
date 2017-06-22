using SPAccounts.DataAccessObject.DTO;
 
using System.Collections.Generic;
 

namespace SPAccounts.BusinessService.Contracts
{
    public interface IDynamicUIBusiness
    {
        List<Menu> GetAllMenues();
    }
}
