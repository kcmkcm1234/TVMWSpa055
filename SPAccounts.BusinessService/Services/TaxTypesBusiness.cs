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
        public TaxTypes GetTaxTypeDetails(string Code)
        {
            return _taxTypesRepository.GetTaxTypeDetails(Code);
        }
    }
}