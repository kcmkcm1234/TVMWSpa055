using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class AccountHeadGroupBusiness: IAccountHeadGroupBusiness
    {
        private IAccountHeadGroupRepository _accountHeadGroupRepository;

        public AccountHeadGroupBusiness(IAccountHeadGroupRepository accountHeadGroupRepository)
        {
            _accountHeadGroupRepository = accountHeadGroupRepository;
        }
        public List<AccountHeadGroup> GetAllAccountHeadGroup()
        {
            return _accountHeadGroupRepository.GetAllAccountHeadGroup();
        }

        public List<AccountHeadGroup> GetDisabledCodeForAccountHeadGroup(string ID)
        {
            return _accountHeadGroupRepository.GetDisabledCodeForAccountHeadGroup(ID);
        }
        public AccountHeadGroup InsertUpdateAccountHeadGroup(AccountHeadGroup accountHeadGroup, AppUA ua)
        {
            if (accountHeadGroup.ID != null && accountHeadGroup.ID != Guid.Empty)
            {
                return _accountHeadGroupRepository.UpdateAccountHeadGroup(accountHeadGroup, ua);
            }
            else
            {
                return _accountHeadGroupRepository.InsertAccountHeadGroup(accountHeadGroup, ua);
            }

        }

             public AccountHeadGroup GetAccountHeadGroupDetailsByID(Guid ID)
        {
            return _accountHeadGroupRepository.GetAccountHeadGroupDetailsByID(ID);
        }

        public object DeleteAccountHeadGroup(Guid ID)
        {
            return _accountHeadGroupRepository.DeleteAccountHeadGroup(ID);
        }

    }
}