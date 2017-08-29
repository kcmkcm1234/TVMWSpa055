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
        [AuthSecurityFilter(ProjectObject = "SalesReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "SalesReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "SalesReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "SalesReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public string GetOtherExpenseSummary(string FromDate, string ToDate, string CompanyCode, string OrderBy)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherExpenseSummaryReportViewModel> otherExpenseSummaryReportList = Mapper.Map<List<OtherExpenseSummaryReport>, List<OtherExpenseSummaryReportViewModel>>(_reportBusiness.GetOtherExpenseSummary(FDate, TDate, CompanyCode, OrderBy));
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
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public ActionResult OtherExpenseDetails()
        {

            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            OtherExpenseDetailsReportViewModel otherExpenseDetailsViewModel = new OtherExpenseDetailsReportViewModel();
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

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public string GetOtherExpenseDetails(string FromDate, string ToDate, string CompanyCode, string OrderBy)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherExpenseDetailsReportViewModel> otherExpenseDetailsReportList = Mapper.Map<List<OtherExpenseDetailsReport>, List<OtherExpenseDetailsReportViewModel>>(_reportBusiness.GetOtherExpenseDetails(FDate, TDate, CompanyCode,OrderBy));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseDetailsReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerReport", Mode = "R")]
        public ActionResult CustomerContactDetails()
        {

            return View();
        }



        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerReport", Mode = "R")]
        public string GetCustomerContactDetails()
        {
           
                try
                {
                  
                    List<CustomerContactDetailsReportViewModel> CustomerContactDetailsReportList = Mapper.Map<List<CustomerContactDetailsReport>, List<CustomerContactDetailsReportViewModel>>(_reportBusiness.GetCustomerContactDetailsReport());
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = CustomerContactDetailsReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

         
          
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SalesReport", Mode = "R")]
        public ActionResult SalesTransactionLog()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            SalesTransactionLogReportViewModel salesTransactionLogReportViewModel = new SalesTransactionLogReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            salesTransactionLogReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (salesTransactionLogReportViewModel.companiesList != null)
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
                foreach (CompaniesViewModel cvm in salesTransactionLogReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
            salesTransactionLogReportViewModel.CompanyList = selectListItem;
            return View(salesTransactionLogReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SalesReport", Mode = "R")]
        public string GetsalesTransactionLog(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<SalesTransactionLogReportViewModel> salesTransactionLogReportViewModelList = Mapper.Map<List<SalesTransactionLogReport>, List<SalesTransactionLogReportViewModel>>(_reportBusiness.GetSalesTransactionLogDetails(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = salesTransactionLogReportViewModelList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }

            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }




        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public ActionResult PurchaseSummaryReport()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            PurchaseSummaryReportViewModel purchaseSummaryReportViewModel = new PurchaseSummaryReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            purchaseSummaryReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (purchaseSummaryReportViewModel.companiesList != null)
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
                foreach (CompaniesViewModel cvm in purchaseSummaryReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
            purchaseSummaryReportViewModel.CompanyList = selectListItem;
            return View(purchaseSummaryReportViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public string GetPurchaseSummaryDetails(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<PurchaseSummaryReportViewModel> purchaseSummaryReportViewModelList = Mapper.Map<List<PurchaseSummaryReport>, List<PurchaseSummaryReportViewModel>>(_reportBusiness.GetPurchaseSummary(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = purchaseSummaryReportViewModelList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }

            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }



        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public string GetPurchaseDetails(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<PurchaseDetailReportViewModel> purchaseDetailReportViewModelList = Mapper.Map<List<PurchaseDetailReport>, List<PurchaseDetailReportViewModel>>(_reportBusiness.GetPurchaseDetails(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = purchaseDetailReportViewModelList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public ActionResult PurchaseDetails()
        {

            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            PurchaseDetailReportViewModel purchaseDetailReportViewModel = new PurchaseDetailReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            purchaseDetailReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (purchaseDetailReportViewModel.companiesList != null)
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
                foreach (CompaniesViewModel cvm in purchaseDetailReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }

            purchaseDetailReportViewModel.CompanyList = selectListItem;
            return View(purchaseDetailReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierReport", Mode = "R")]
        public ActionResult SupplierContactDetails()
        {
            return View();
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierReport", Mode = "R")]
        public string GetSupplierContactDetails()
        {
            try
            {
                List<SupplierContactDetailsReportViewModel> supplierContactDetailsReportList = Mapper.Map<List<SupplierContactDetailsReport>, List<SupplierContactDetailsReportViewModel>>(_reportBusiness.GetSupplierContactDetailsReport());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierContactDetailsReportList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public ActionResult PurchaseTransactionLog()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            PurchaseTransactionLogReportViewModel purchaseTransactionLogReportViewModel = new PurchaseTransactionLogReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            purchaseTransactionLogReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (purchaseTransactionLogReportViewModel.companiesList != null)
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
                foreach (CompaniesViewModel cvm in purchaseTransactionLogReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
            purchaseTransactionLogReportViewModel.CompanyList = selectListItem;
            return View(purchaseTransactionLogReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public string GetPurchaseTransactionLog(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<PurchaseTransactionLogReportViewModel> purchaseTransactionLogReportList = Mapper.Map<List<PurchaseTransactionLogReport>, List<PurchaseTransactionLogReportViewModel>>(_reportBusiness.GetPurchaseTransactionLogDetails(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = purchaseTransactionLogReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }

            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult AccountsReceivableAgeingDetails()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            AccountsReceivableAgeingReportViewModel accountsReceivableAgeingReportViewModel = new AccountsReceivableAgeingReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            accountsReceivableAgeingReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (accountsReceivableAgeingReportViewModel.companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });
               
                foreach (CompaniesViewModel cvm in accountsReceivableAgeingReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
            accountsReceivableAgeingReportViewModel.CompanyList = selectListItem;
            return View(accountsReceivableAgeingReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetAccountsReceivableAgeingDetails(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<AccountsReceivableAgeingReportViewModel> accountsReceivableAgeingReportList = Mapper.Map<List<AccountsReceivableAgeingReport>, List<AccountsReceivableAgeingReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingReport(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = accountsReceivableAgeingReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }

            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult AccountsReceivableAgeingSummary()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            AccountsReceivableAgeingSummaryReportViewModel accountsReceivableAgeingSummaryReportViewModel = new AccountsReceivableAgeingSummaryReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            accountsReceivableAgeingSummaryReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (accountsReceivableAgeingSummaryReportViewModel.companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (CompaniesViewModel cvm in accountsReceivableAgeingSummaryReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
            accountsReceivableAgeingSummaryReportViewModel.CompanyList = selectListItem;
            return View(accountsReceivableAgeingSummaryReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetAccountsReceivableAgeingSummary(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<AccountsReceivableAgeingSummaryReportViewModel> AccountsReceivableAgeingSummaryList = Mapper.Map<List<AccountsReceivableAgeingSummaryReport>, List<AccountsReceivableAgeingSummaryReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingSummaryReport(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = AccountsReceivableAgeingSummaryList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }

            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult AccountsPayableAgeingDetails()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            AccountsPayableAgeingReportViewModel accountsPayableAgeingReportViewModel = new AccountsPayableAgeingReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            accountsPayableAgeingReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (accountsPayableAgeingReportViewModel.companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (CompaniesViewModel cvm in accountsPayableAgeingReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
            accountsPayableAgeingReportViewModel.CompanyList = selectListItem;
            return View(accountsPayableAgeingReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetAccountsPayableAgeingDetails(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<AccountsPayableAgeingReportViewModel> accountsPayableAgeingReportList = Mapper.Map<List<AccountsPayableAgeingReport>, List<AccountsPayableAgeingReportViewModel>>(_reportBusiness.GetAccountsPayableAgeingReport(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = accountsPayableAgeingReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }

            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult AccountsPayableAgeingSummary()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            AccountsPayableAgeingSummaryReportViewModel accountsPayableAgeingSummaryReportViewModel = new AccountsPayableAgeingSummaryReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            accountsPayableAgeingSummaryReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (accountsPayableAgeingSummaryReportViewModel.companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (CompaniesViewModel cvm in accountsPayableAgeingSummaryReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
            accountsPayableAgeingSummaryReportViewModel.CompanyList = selectListItem;
            return View(accountsPayableAgeingSummaryReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetAccountsPayableAgeingSummary(string FromDate, string ToDate, string CompanyCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<AccountsPayableAgeingSummaryReportViewModel> accountsPayableAgeingSummaryReportList = Mapper.Map<List<AccountsPayableAgeingSummaryReport>, List<AccountsPayableAgeingSummaryReportViewModel>>(_reportBusiness.GetAccountsPayableAgeingSummaryReport(FDate, TDate, CompanyCode));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = accountsPayableAgeingSummaryReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }

            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }

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