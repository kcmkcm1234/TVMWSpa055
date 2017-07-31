using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class TaxTypesBusiness:ITaxTypesBusiness
    {
        private ITaxTypesRepository _taxTypesRepository;

        public TaxTypesBusiness(ITaxTypesRepository taxTypesRepository)
        {
            _taxTypesRepository = taxTypesRepository;
        }
        public List<TaxTypes> GetAllTaxTypes()
        {
            return _taxTypesRepository.GetAllTaxTypes();
        }
        public TaxTypes GetTaxTypeDetailsByCode(string Code)
        {
            return _taxTypesRepository.GetTaxTypeDetailsByCode(Code);
        }
        public object InsertUpdateTaxType(TaxTypes _taxTypesObj)
        {
            object result = null;
            try
            {
                if ((_taxTypesObj.isUpdate) == "0")
                {
                    result = _taxTypesRepository.InsertTaxType(_taxTypesObj);
                }
                else
                {
                    result = _taxTypesRepository.UpdateTaxType(_taxTypesObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public object DeleteTaxType(string Code)
        {
            object result = null;
            try
            {
                result = _taxTypesRepository.DeleteTaxType(Code);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


    }
}