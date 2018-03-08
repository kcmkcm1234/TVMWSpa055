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
        public List<ChartOfAccounts> GetAllChartOfAccounts(string type)
        {
            return _chartOfAccountsRepository.GetAllChartOfAccounts(type);
        }

        public ChartOfAccounts GetChartOfAccountDetails(string Code)
        {
            return _chartOfAccountsRepository.GetChartOfAccountDetails(Code);
        }

        public object InsertUpdateChartOfAccounts(ChartOfAccounts chartOfAccountsObj)
        {
            object result = null;
            try
            {
                if ((chartOfAccountsObj.isUpdate) == "0")
                {
                    result = _chartOfAccountsRepository.InsertChartOfAccounts(chartOfAccountsObj);
                }
                else
                {
                    result = _chartOfAccountsRepository.UpdateChartOfAccounts(chartOfAccountsObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public object UpdateAssignments(string code)
        {
            object result = null;
            try
            {

             result = _chartOfAccountsRepository.UpdateAssignments(code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    
        public object DeleteChartOfAccounts(string Code)
        {
            object result = null;
            try
            {
                result = _chartOfAccountsRepository.DeleteChartOfAccounts(Code);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<ChartOfAccounts> GetAllAccountTypesForAccountHeadGroup()
        {
            return _chartOfAccountsRepository.GetAllAccountTypesForAccountHeadGroup();
        }

    }
}