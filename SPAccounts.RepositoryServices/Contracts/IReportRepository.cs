using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public  interface IReportRepository
    {
        List<SystemReport> GetAllSysReports(AppUA appUA);
        List<SaleSummary> GetSaleSummary(DateTime? FromDate,DateTime? ToDate, string CompanyCode);
        List<SaleDetail> GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode);
    }
}
