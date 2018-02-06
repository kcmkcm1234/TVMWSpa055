using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class PaymentFollowupBusiness : IPaymentFollowupBusiness
    {
        IPaymentFollowupRepository _paymentFollowupRepository;
        ICommonBusiness _commonBusiness;
        public PaymentFollowupBusiness(IPaymentFollowupRepository paymentFollowupRepository, ICommonBusiness commonBusiness)
        {
            _paymentFollowupRepository = paymentFollowupRepository;
            _commonBusiness = commonBusiness;
        }
        public List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? toDate, string filter, string company, string customer, string outstanding)
        {
            List<CustomerExpeditingReport> customerExpeditingList = null;
            try
            {
                customerExpeditingList = _paymentFollowupRepository.GetCustomerExpeditingDetail(toDate, filter, company, customer, outstanding);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerExpeditingList;
        }

        #region FollowUPList
        public List<FollowUp> GetFollowUpDetails(Guid customerID)
        {
            return _paymentFollowupRepository.GetFollowUpDetails(customerID);
        }
        #endregion FollowUpList

        #region Insertfollowup
        public FollowUp InsertUpdateFollowUp(FollowUp followupObj)
        {
            FollowUp result = null;
            try
            {
                if (followupObj.ID == Guid.Empty)
                {
                    result = _paymentFollowupRepository.InsertFollowUp(followupObj);
                }
                else
                {
                    result = _paymentFollowupRepository.UpdateFollowUp(followupObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion Insertfollowup

        #region FollowupDetailsByID
        public FollowUp GetFollowupDetailsByFollowUpID(Guid ID)
        {
            return _paymentFollowupRepository.GetFollowupDetailsByFollowUpID(ID);
        }

        #endregion FollowupDetailsByID

        public object DeleteFollowUp(Guid ID)
        {
            return _paymentFollowupRepository.DeleteFollowUp(ID);
        }
    }
}