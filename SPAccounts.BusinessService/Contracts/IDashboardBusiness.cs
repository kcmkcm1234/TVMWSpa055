using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IDashboardBusiness
    {
        MonthlyRecap GetMonthlyRecap(string Company);
        TopDocs GetTopDocs(string DocType, string Company,string BaseURL);
    }
}
