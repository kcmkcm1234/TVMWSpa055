using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class BankBusiness : IBankBusiness
    {
        private IBankRepository _bankRepository;

        public BankBusiness(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }
        public List<Bank> GetAllBanks()
        {
            return _bankRepository.GetAllBank();
        }

        public Bank GetBankDetailsByCode(string Code)
        {
            return _bankRepository.GetBankDetailsByCode(Code);
        }

        public object InsertUpdateBank(Bank bankObj, AppUA ua)
        {
            object result = null;
            try
            {
                if ((bankObj.isUpdate)=="0")
                {
                    result = _bankRepository.InsertBank(bankObj,ua);
                }
                else
                {                   
                    result = _bankRepository.UpdateBank(bankObj,ua);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public object DeleteBank(string Code)
        {
            object result = null;
            try
            {
                  result = _bankRepository.DeleteBank(Code);
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}