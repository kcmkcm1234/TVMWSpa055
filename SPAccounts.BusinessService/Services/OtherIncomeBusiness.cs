using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class OtherIncomeBusiness : IOtherIncomeBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IOtherIncomeRepository _iOtherIncomeRepository;

        public OtherIncomeBusiness(IOtherIncomeRepository iOtherIncomeRepository, ICommonBusiness commonBusiness)
        {
            _iOtherIncomeRepository = iOtherIncomeRepository;
            _commonBusiness = commonBusiness;
        }

        public List<OtherIncome> GetAllOtherIncome(string IncomeDate)
        {
            return _iOtherIncomeRepository.GetAllOtherIncome(IncomeDate);
        }

        public OtherIncome GetOtherIncomeDetails(Guid ID)
        {
            OtherIncome otherIncomeObj = new OtherIncome();
            otherIncomeObj = _iOtherIncomeRepository.GetOtherIncomeDetails(ID);            
            return otherIncomeObj;
        }
        public object InsertUpdateOtherIncome(OtherIncome _otherIncomeObj, AppUA ua)
        {
            object result = null;
            try
            {
                if (_otherIncomeObj.ID == Guid.Empty)
                {                    
                    result = _iOtherIncomeRepository.InsertOtherIncome(_otherIncomeObj, ua);
                }
                else
                {
                    result = _iOtherIncomeRepository.UpdateOtherIncome(_otherIncomeObj, ua);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public object DeleteOtherIncome(Guid ID)
        {
            object result = null;
            try
            {
                result = _iOtherIncomeRepository.DeleteOtherIncome(ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}