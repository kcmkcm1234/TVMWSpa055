using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ICompaniesBusiness
    {
        List<Companies> GetAllCompanies();
        List<Companies> GetAllunits();
        Companies GetCompanyDetailsByCode(string Code);
        Companies InsertUpdateCompany(Companies _companyObj);
    }
}