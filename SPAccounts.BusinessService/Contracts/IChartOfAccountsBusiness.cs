﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
   public interface IChartOfAccountsBusiness
    {
        List<ChartOfAccounts> GetChartOfAccountsByType(string type);
        List<ChartOfAccounts> GetAllChartOfAccounts();
        ChartOfAccounts GetChartOfAccountDetails(string Code);
        object InsertUpdateChartOfAccounts(ChartOfAccounts chartOfAccountsObj);
        object DeleteChartOfAccounts(string Code);
    }
}
