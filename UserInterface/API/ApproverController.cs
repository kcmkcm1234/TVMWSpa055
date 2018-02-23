using AutoMapper;
using Newtonsoft.Json;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserInterface.Models;

namespace UserInterface.API
{
    public class ApproverController : ApiController
    {
        #region Constructor Injection
        IApproverBusiness _approvalBusiness;
        #endregion Constructor Injection

        public ApproverController(IApproverBusiness approvalBusiness)
        {
            _approvalBusiness = approvalBusiness;

        }

        #region GetAllApprovedExpenseAndSupplierPayments
        [HttpPost]
        public string GetPreviousApprovalsInfo(Approver approver)
        {
            try
            {
                List<ApproverViewModel> previousApprovals = Mapper.Map<List<Approver>, List<ApproverViewModel>>(_approvalBusiness.GetPreviousApprovalsInfo(approver));
                return JsonConvert.SerializeObject(new { Result = true, Records = previousApprovals });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion GetAllApprovedExpenseAndSupplierPayments
    }
}
