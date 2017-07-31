using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class ChartOfAccountsBusiness : IChartOfAccountsBusiness
    {
        private IChartOfAccountsRepository _chartOfAccountsRepository;

        public ChartOfAccountsBusiness(IChartOfAccountsRepository chartOfAccountsRepository)
        {
            _chartOfAccountsRepository = chartOfAccountsRepository;
        }

        public List<ChartOfAccounts> GetChartOfAccountsByType(string type)
        {
            return _chartOfAccountsRepository.GetChartOfAccountsByType(type);
        }

        public List<ChartOfAccounts> GetExpenseTypeDetails(ChartOfAccounts account)
        {
            return _chartOfAccountsRepository.GetExpenseTypeDetails(account);
        }
    }
}