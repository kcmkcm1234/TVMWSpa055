using SPAccounts.BusinessService.Contracts;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;

namespace SPAccounts.BusinessService.Services
{
    public class ApprovalStatusBusiness:IApprovalStatusBusiness
    {
        private IApprovalStatusRepository _approvalStatusRepository;

        public ApprovalStatusBusiness(IApprovalStatusRepository approvalStatusRepository)
        {
            _approvalStatusRepository = approvalStatusRepository;
        }

        public List<ApprovalStatus> GetAllApprovalStatus()
        {
            return _approvalStatusRepository.GetAllApprovalStatus();
        }
    }
}