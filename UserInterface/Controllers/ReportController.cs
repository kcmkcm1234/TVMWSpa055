using AutoMapper;
using Newtonsoft.Json;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class ReportController : Controller
    {
        IReportBusiness _reportBusiness;
        ICompaniesBusiness _companiesBusiness;
        public ReportController(IReportBusiness reportBusiness, ICompaniesBusiness companiesBusiness)
        {
            _reportBusiness = reportBusiness;
            _companiesBusiness = companiesBusiness;
        }
        // GET: Report
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public ActionResult Index()
        {
            AppUA _appUA= Session["AppUA"] as AppUA;
            List<SystemReportViewModel> systemReportList = Mapper.Map<List<SystemReport>, List<SystemReportViewModel>>(_reportBusiness.GetAllSysReports(_appUA));
            systemReportList = systemReportList != null ? systemReportList.OrderBy(s => s.GroupOrder).ToList() : null;
            return View(systemReportList);
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public ActionResult SaleSummary()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            SaleSummaryViewModel SaleSummary = new SaleSummaryViewModel();
            List<SelectListItem>  selectListItem = new List<SelectListItem>();
            SaleSummary.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if(SaleSummary.companiesList!=null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });
                selectListItem.Add(new SelectListItem
                {
                    Text = "Company wise",
                    Value = "companywise",
                    Selected = false
                });
                foreach (CompaniesViewModel cvm in SaleSummary.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
          
            SaleSummary.CompanyList = selectListItem;
            return View(SaleSummary);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public string GetSaleSummary(string FromDate,string ToDate,string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                   DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                   DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                   List<SaleSummaryViewModel>salesummaryList= Mapper.Map<List<SaleSummary>,List<SaleSummaryViewModel>>(_reportBusiness.GetSaleSummary(FDate, TDate, CompanyCode));
                   return JsonConvert.SerializeObject(new { Result = "OK", Records = salesummaryList });
                }
                catch(Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public string GetSaleDetail(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<SaleDetailReportViewModel> saleDetailReportList = Mapper.Map<List<SaleDetailReport>, List<SaleDetailReportViewModel>>(_reportBusiness.GetSaleDetail(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = saleDetailReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public ActionResult SalesDetail()
        {
            
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            SaleDetailReportViewModel saleDetailReportViewModel = new SaleDetailReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            saleDetailReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (saleDetailReportViewModel.companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });
                selectListItem.Add(new SelectListItem
                {
                    Text = "Company wise",
                    Value = "companywise",
                    Selected = false
                });
                foreach (CompaniesViewModel cvm in saleDetailReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }

            saleDetailReportViewModel.CompanyList = selectListItem;
            return View(saleDetailReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public ActionResult OtherExpenseSummary()
        {

            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            OtherExpenseSummaryReportViewModel otherExpenseSummaryReportViewModel = new OtherExpenseSummaryReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            otherExpenseSummaryReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (otherExpenseSummaryReportViewModel.companiesList != null)
            { 
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });
                selectListItem.Add(new SelectListItem
                {
                    Text = "Company Wise",
                    Value = "companywise",
                    Selected = false
                });
                foreach (CompaniesViewModel cvm in otherExpenseSummaryReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }

            otherExpenseSummaryReportViewModel.CompanyList = selectListItem;
            return View(otherExpenseSummaryReportViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public string GetOtherExpenseSummary(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherExpenseSummaryReportViewModel> otherExpenseSummaryReportList = Mapper.Map<List<OtherExpenseSummaryReport>, List<OtherExpenseSummaryReportViewModel>>(_reportBusiness.GetOtherExpenseSummary(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseSummaryReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }



        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public ActionResult OtherExpenseDetails()
        {

            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            OtherExpenseDetailsViewModel otherExpenseDetailsViewModel = new OtherExpenseDetailsViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            otherExpenseDetailsViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (otherExpenseDetailsViewModel.companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });
                selectListItem.Add(new SelectListItem
                {
                    Text = "Company Wise",
                    Value = "companywise",
                    Selected = false
                });
                foreach (CompaniesViewModel cvm in otherExpenseDetailsViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }

            otherExpenseDetailsViewModel.CompanyList = selectListItem;
            return View(otherExpenseDetailsViewModel);
        }

        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        //public string GetOtherExpenseSummary(string FromDate, string ToDate, string CompanyCode)
        //{
        //    if (!string.IsNullOrEmpty(CompanyCode))
        //    {
        //        try
        //        {
        //            DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
        //            DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
        //            List<OtherExpenseSummaryReportViewModel> otherExpenseSummaryReportList = Mapper.Map<List<OtherExpenseSummaryReport>, List<OtherExpenseSummaryReportViewModel>>(_reportBusiness.GetOtherExpenseSummary(FDate, TDate, CompanyCode));
        //            return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseSummaryReportList });
        //        }
        //        catch (Exception ex)
        //        {
        //            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
        //        }

        //    }
        //    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        //}


        #region ButtonStyling
        [HttpGet]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = false;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    break;
              
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion

    }
}