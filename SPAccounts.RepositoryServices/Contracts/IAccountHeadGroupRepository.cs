using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
   public interface IAccountHeadGroupRepository
    {
        List<AccountHeadGroup> GetAllAccountHeadGroup();
        List<AccountHeadGroup> GetDisabledCodeForAccountHeadGroup(string ID);
        AccountHeadGroup InsertAccountHeadGroup(AccountHeadGroup accountHeadGroup, AppUA ua);
        AccountHeadGroup UpdateAccountHeadGroup(AccountHeadGroup accountHeadGroup, AppUA ua);
        AccountHeadGroup GetAccountHeadGroupDetailsByID(Guid ID);
        object DeleteAccountHeadGroup(Guid ID);

    }
}
