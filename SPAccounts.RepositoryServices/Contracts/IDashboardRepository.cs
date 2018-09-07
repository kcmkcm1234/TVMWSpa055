using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface IDashboardRepository
    {
         MonthlyRecap GetMonthlyRecap(MonthlyRecap Company);
        MonthlySalesPurchase GetSalesPurchase(MonthlySalesPurchase data);
        TopDocs GetTopDocs(string DocType, string Company, bool IsInternal);
        List<SalesSummary> GetSalesSummaryChart(SalesSummary duration);
    }
}
