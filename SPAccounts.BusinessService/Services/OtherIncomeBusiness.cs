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

        public List<OtherIncome> GetAllOtherIncome(string IncomeDate,string DefaultDate)
        {
            return _iOtherIncomeRepository.GetAllOtherIncome(IncomeDate,DefaultDate);
        }

        public OtherIncome GetOtherIncomeDetails(Guid ID)
        {
            OtherIncome otherIncomeObj = new OtherIncome();
            otherIncomeObj = _iOtherIncomeRepository.GetOtherIncomeDetails(ID);
            if (otherIncomeObj != null)
            {
                otherIncomeObj.creditAmountFormatted = _commonBusiness.ConvertCurrency(otherIncomeObj.Amount, 2);

            }
            return otherIncomeObj;
        }
        public object InsertUpdateOtherIncome(OtherIncome _otherIncomeObj)
        {
            object result = null;
            try
            {
                if (_otherIncomeObj.ID == Guid.Empty)
                {                    
                    result = _iOtherIncomeRepository.InsertOtherIncome(_otherIncomeObj);
                }
                else
                {
                    result = _iOtherIncomeRepository.UpdateOtherIncome(_otherIncomeObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public object DeleteOtherIncome(Guid ID, string userName)
        {
            object result = null;
            try
            {
                result = _iOtherIncomeRepository.DeleteOtherIncome(ID,userName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}