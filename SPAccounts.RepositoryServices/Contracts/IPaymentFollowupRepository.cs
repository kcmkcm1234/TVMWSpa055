using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface IPaymentFollowupRepository
    {
        List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? toDate, string filter, string company, string customer, string outstanding);
        List<FollowUp> GetFollowUpDetails(Guid customerID);
        FollowUp InsertFollowUp(FollowUp followupObj);
        FollowUp UpdateFollowUp(FollowUp followupObj);
        FollowUp GetFollowupDetailsByFollowUpID(Guid ID);
        List<FollowUp> GetRecentFollowUpCount(DateTime? toDay);
        object DeleteFollowUp(Guid ID);
    }
}