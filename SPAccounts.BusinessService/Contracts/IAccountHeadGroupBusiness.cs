using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IAccountHeadGroupBusiness
    {
        List<AccountHeadGroup> GetAllAccountHeadGroup();
        List<AccountHeadGroup> GetDisabledCodeForAccountHeadGroup(string ID);
        AccountHeadGroup InsertUpdateAccountHeadGroup(AccountHeadGroup accountHeadGroup, AppUA ua);
        AccountHeadGroup GetAccountHeadGroupDetailsByID(Guid ID);
        object DeleteAccountHeadGroup(Guid ID);
        List<AccountHeadGroup> GetAllGroupName();

    }
}
