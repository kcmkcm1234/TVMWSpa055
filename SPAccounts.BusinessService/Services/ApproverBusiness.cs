using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class ApproverBusiness: IApproverBusiness
    {
        IApproverRepository _approvalRepository;
        public ApproverBusiness(IApproverRepository approvalRepository)
        {
            _approvalRepository = approvalRepository;
        }
        public List<Approver> GetPreviousApprovalsInfo(Approver approver)
        {
            try
            {
                return _approvalRepository.GetPreviousApprovalsInfo(approver);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}