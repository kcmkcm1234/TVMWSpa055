using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class DashboardBusiness:IDashboardBusiness
    {
        private IDashboardRepository _dashboardRepository;

        public DashboardBusiness(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public MonthlyRecap GetMonthlyRecap(string Company)
        {
            return _dashboardRepository.GetMonthlyRecap(Company);
        }

    }
}