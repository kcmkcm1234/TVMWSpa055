using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ITaxTypesBusiness
    {
        List<TaxTypes> GetAllTaxTypes();
        TaxTypes GetTaxTypeDetailsByCode(string Code);
        object InsertUpdateTaxType(TaxTypes _taxTypesObj, AppUA ua);
        object DeleteTaxType(string Code);
    }
}