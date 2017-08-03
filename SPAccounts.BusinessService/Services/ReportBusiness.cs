using SPAccounts.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;

namespace SPAccounts.BusinessService.Services
{
    public class ReportBusiness : IReportBusiness
    {
        IReportRepository _reportRepository;
        public ReportBusiness(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public List<SystemReport> GetAllSysReports(AppUA appUA)
        {
            List<SystemReport> systemReportList = null;
            try
            {
                systemReportList = _reportRepository.GetAllSysReports(appUA);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return systemReportList;
        }

        public List<SaleDetail> GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<SaleDetail> saleDetailList = null;
            try
            {
                saleDetailList = _reportRepository.GetSaleDetail(FromDate,ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saleDetailList;
        }

        public List<SaleSummary> GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<SaleSummary> saleSummaryList = null;
            try
            {
                saleSummaryList = _reportRepository.GetSaleSummary(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saleSummaryList;
        }
    }
}