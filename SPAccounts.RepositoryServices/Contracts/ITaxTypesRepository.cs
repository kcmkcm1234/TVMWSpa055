using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ITaxTypesRepository
    {
        List<TaxTypes> GetAllTaxTypes();
        TaxTypes GetTaxTypeDetailsByCode(string Code);
        TaxTypes InsertTaxType(TaxTypes _taxTypesObj, AppUA ua);
        object UpdateTaxType(TaxTypes _taxTypesObj, AppUA ua);
        object DeleteTaxType(string code);
    }
}