using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
   public interface IChartOfAccountsRepository
    {
        List<ChartOfAccounts> GetChartOfAccountsByType(string type);
        List<ChartOfAccounts> GetAllChartOfAccounts(string type);
        ChartOfAccounts GetChartOfAccountDetails(string Code);
        ChartOfAccounts InsertChartOfAccounts(ChartOfAccounts _chartOfAccountsObj);
        object UpdateChartOfAccounts(ChartOfAccounts _chartOfAccountsObj);
        object DeleteChartOfAccounts(string code);
        object UpdateAssignments(string code);
        List<ChartOfAccounts> GetAllAccountTypesForAccountHeadGroup();

    }
}
