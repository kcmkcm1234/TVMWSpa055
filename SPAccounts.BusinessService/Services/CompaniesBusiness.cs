using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class CompaniesBusiness:ICompaniesBusiness
    {
        private ICompaniesRepository _companiesRepository;

        public CompaniesBusiness(ICompaniesRepository companiesRepository)
        {
            _companiesRepository = companiesRepository;
        }
        public List<Companies> GetAllCompanies()
        {
            return _companiesRepository.GetAllCompanies();
        }
    }
}