using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class MasterBusiness:IMasterBusiness
    {
        private IMasterRepository _masterRepository;

        public MasterBusiness(IMasterRepository masterRepository)
        {
            _masterRepository = masterRepository;
        }
        
    }
}