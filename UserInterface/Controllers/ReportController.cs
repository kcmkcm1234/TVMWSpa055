using AutoMapper;
using Newtonsoft.Json;
using SAMTool.DataAccessObject.DTO;
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
        ICustomerBusiness _customerBusiness;
        ISupplierBusiness _supplierBusiness;
        ICompaniesBusiness _companiesBusiness;
        IBankBusiness _bankBusiness;
        IEmployeeBusiness _employeeBusiness;
        IOtherExpenseBusiness _otherExpenseBusiness;
        ICommonBusiness _commonBusiness;
        public ReportController(IReportBusiness reportBusiness, ICompaniesBusiness companiesBusiness,IEmployeeBusiness employeeBusiness, IOtherExpenseBusiness otherExpenseBusiness, ICommonBusiness commonBusiness, IBankBusiness bankbusiness, ICustomerBusiness customerBusiness, ISupplierBusiness supplierBusiness)
        {
            _reportBusiness = reportBusiness;
            _supplierBusiness = supplierBusiness;
             _companiesBusiness = companiesBusiness;
            _employeeBusiness = employeeBusiness;
            _otherExpenseBusiness = otherExpenseBusiness;
            _commonBusiness = commonBusiness;
            _bankBusiness = bankbusiness;
            _customerBusiness = customerBusiness;

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
                //selectListItem.Add(new SelectListItem
                //{
                //    Text = "Company wise",
                //    Value = "companywise",
                //    Selected = false
                //});
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
        public string GetSaleSummary(string FromDate,string ToDate,string CompanyCode, string search,Boolean IsInternal,Boolean IsTax)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                   DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                   DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                   SaleSummaryViewModel saleObj = Mapper.Map<SaleSummary,SaleSummaryViewModel>(_reportBusiness.GetSaleSummary(FDate, TDate, CompanyCode,search,IsInternal, IsTax));
                   
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = saleObj.saleSummaryList, TotalAmount = saleObj.salesummarySum, InvoicedAmount = saleObj.salesummaryinvoice, PaidAmount= saleObj.salesummarypaid,TaxAmount=saleObj.salesummaryTax});
                
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
        public string GetSaleDetail(string FromDate, string ToDate, string CompanyCode,string search, Boolean IsInternal, Boolean IsTax)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    SaleDetailReportViewModel SaledetailObj = Mapper.Map<SaleDetailReport,SaleDetailReportViewModel>(_reportBusiness.GetSaleDetail(FDate, TDate, CompanyCode,search,IsInternal,IsTax));
                    
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = SaledetailObj.saleDetailList, TotalAmount = SaledetailObj.saledetailSum, InvoicedAmount= SaledetailObj.saledetailinvoice, PaidAmount= SaledetailObj.saledetailpaid,TaxAmount= SaledetailObj.saledetailtax });
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
                //selectListItem.Add(new SelectListItem
                //{
                //    Text = "Company wise",
                //    Value = "companywise",
                //    Selected = false
                //});
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
            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            List<ChartOfAccountsViewModel> chartOfAccountList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_otherExpenseBusiness.GetAllAccountTypes("OE"));
            selectListItem.Add(new SelectListItem
            {
                Text = "All",
                Value = "ALL",
                Selected = true
            });

            foreach (ChartOfAccountsViewModel cav in chartOfAccountList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = cav.TypeDesc,
                    Value = cav.Code + ":" + cav.ISEmploy,
                    Selected = false,


                });
            }
            otherExpenseSummaryReportViewModel.AccountHeadList = selectListItem;
            selectListItem = null;

            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            List<EmployeeTypeViewModel> empTypeList = Mapper.Map<List<EmployeeType>, List<EmployeeTypeViewModel>>(_otherExpenseBusiness.GetAllEmployeeTypes());
            foreach (EmployeeTypeViewModel etvm in empTypeList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = etvm.Name,
                    Value = etvm.Code,
                    Selected = false
                });
            }
            otherExpenseSummaryReportViewModel.EmployeeTypeList = selectListItem;


            selectListItem = new List<SelectListItem>();
            List<EmployeeViewModel> empList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_otherExpenseBusiness.GetAllEmployees());
            foreach (EmployeeViewModel evm in empList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = evm.Name,
                    Value = evm.ID.ToString(),
                    Selected = false
                });
            }
            otherExpenseSummaryReportViewModel.EmployeeList = selectListItem;

            return View(otherExpenseSummaryReportViewModel);
        }



        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public string GetOtherExpenseSummary(string FromDate, string ToDate, string CompanyCode,string ReportType, string OrderBy,string accounthead, string subtype,string employeeorother,string employeecompany,string search)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherExpenseSummaryReportViewModel> otherExpenseSummaryReportList = Mapper.Map<List<OtherExpenseSummaryReport>, List<OtherExpenseSummaryReportViewModel>>(_reportBusiness.GetOtherExpenseSummary(FDate, TDate, CompanyCode,ReportType, OrderBy,accounthead.Split(':')[0], subtype, employeeorother, employeecompany, search));
                   
                    decimal otherExpenseSum = otherExpenseSummaryReportList.Sum(OE => OE.Amount);
                    string otherExpenseSumFormatted=_commonBusiness.ConvertCurrency(otherExpenseSum, 2);


                    return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseSummaryReportList, TotalAmount= otherExpenseSumFormatted });
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
            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            List<ChartOfAccountsViewModel> chartOfAccountList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_otherExpenseBusiness.GetAllAccountTypes("OE"));
            selectListItem.Add(new SelectListItem
            {
                Text = "All",
                Value = "ALL",
                Selected = true
            });
            foreach (ChartOfAccountsViewModel cav in chartOfAccountList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = cav.TypeDesc,
                    Value = cav.Code + ":" + cav.ISEmploy,
                    Selected = false,


                });
            }
            otherExpenseDetailsViewModel.AccountHeadList = selectListItem;
            selectListItem = null;

            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            List<EmployeeTypeViewModel> empTypeList = Mapper.Map<List<EmployeeType>, List<EmployeeTypeViewModel>>(_otherExpenseBusiness.GetAllEmployeeTypes());
            foreach (EmployeeTypeViewModel etvm in empTypeList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = etvm.Name,
                    Value = etvm.Code,
                    Selected = false
                });
            }
            otherExpenseDetailsViewModel.EmployeeTypeList = selectListItem;


            selectListItem = new List<SelectListItem>();
            List<EmployeeViewModel> empList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_otherExpenseBusiness.GetAllEmployees());
            foreach (EmployeeViewModel evm in empList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = evm.Name,
                    Value = evm.ID.ToString(),
                    Selected = false
                });
            }
            otherExpenseDetailsViewModel.EmployeeList = selectListItem;
            return View(otherExpenseDetailsViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public string GetOtherExpenseDetails(string FromDate, string ToDate, string CompanyCode, string OrderBy, string accounthead, string subtype, string employeeorother,string employeecompany, string search)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherExpenseDetailsReportViewModel> otherExpenseDetailsReportList = Mapper.Map<List<OtherExpenseDetailsReport>, List<OtherExpenseDetailsReportViewModel>>(_reportBusiness.GetOtherExpenseDetails(FDate, TDate, CompanyCode,OrderBy, accounthead.Split(':')[0], subtype, employeeorother, employeecompany,search));
                    decimal otherExpenseDetailsSum = otherExpenseDetailsReportList.Sum(OE => OE.Amount);
                    string otherExpenseDetailsSumFormatted = _commonBusiness.ConvertCurrency(otherExpenseDetailsSum, 2);
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseDetailsReportList, TotalAmount = otherExpenseDetailsSumFormatted });
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
        public string GetCustomerContactDetails(string search)
        {
           
                try
                {
                  
                    List<CustomerContactDetailsReportViewModel> CustomerContactDetailsReportList = Mapper.Map<List<CustomerContactDetailsReport>, List<CustomerContactDetailsReportViewModel>>(_reportBusiness.GetCustomerContactDetailsReport(search));
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
        public string GetsalesTransactionLog(string FromDate, string ToDate, string CompanyCode, string search)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<SalesTransactionLogReportViewModel> salesTransactionLogReportViewModelList = Mapper.Map<List<SalesTransactionLogReport>, List<SalesTransactionLogReportViewModel>>(_reportBusiness.GetSalesTransactionLogDetails(FDate, TDate, CompanyCode,search));
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
                //selectListItem.Add(new SelectListItem
                //{
                //    Text = "Company Wise",
                //    Value = "companywise",
                //    Selected = false
                //});
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
        public string GetPurchaseSummaryDetails(string FromDate, string ToDate, string CompanyCode, string search, Boolean IsInternal)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    PurchaseSummaryReportViewModel purchaseSummaryReportList = Mapper.Map<PurchaseSummaryReport, PurchaseSummaryReportViewModel>(_reportBusiness.GetPurchaseSummary(FDate, TDate, CompanyCode,search, IsInternal));
                    
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = purchaseSummaryReportList.purchaseSummaryReportList, TotalAmount= purchaseSummaryReportList.purchaseSummarySum, InvoicedAmount= purchaseSummaryReportList.purchaseSummaryInvoice, PaidAmount= purchaseSummaryReportList.purchaseSummaryPaid });
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
        public string GetPurchaseDetails(string FromDate, string ToDate, string CompanyCode, string search, Boolean IsInternal)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                  PurchaseDetailReportViewModel purchasedetailObj = Mapper.Map<PurchaseDetailReport, PurchaseDetailReportViewModel>(_reportBusiness.GetPurchaseDetails(FDate, TDate, CompanyCode,search,IsInternal));
                    
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = purchasedetailObj.purchaseDetailReportList, TotalAmount= purchasedetailObj.purchaseDetailSum, InvoicedAmount = purchasedetailObj.purchaseDetailInvoice, PaidAmount = purchasedetailObj.purchaseDetailPaid });
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
                //selectListItem.Add(new SelectListItem
                //{
                //    Text = "Company wise",
                //    Value = "companywise",
                //    Selected = false
                //});
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
        public string GetSupplierContactDetails(string search)
        {
            try
            {
                List<SupplierContactDetailsReportViewModel> supplierContactDetailsReportList = Mapper.Map<List<SupplierContactDetailsReport>, List<SupplierContactDetailsReportViewModel>>(_reportBusiness.GetSupplierContactDetailsReport(search));
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
        public string GetPurchaseTransactionLog(string FromDate, string ToDate, string CompanyCode, string search)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<PurchaseTransactionLogReportViewModel> purchaseTransactionLogReportList = Mapper.Map<List<PurchaseTransactionLogReport>, List<PurchaseTransactionLogReportViewModel>>(_reportBusiness.GetPurchaseTransactionLogDetails(FDate, TDate, CompanyCode,search));
                   
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
                    AppUA _appUA = Session["AppUA"] as AppUA;
                    List<AccountsReceivableAgeingReportViewModel> accountsReceivableAgeingReportList = new List<AccountsReceivableAgeingReportViewModel>();
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);

                    string[] arr = _appUA.RolesCSV.Split(',');
                    if (arr.Contains("SAdmin") || arr.Contains("CEO"))
                    {
                        accountsReceivableAgeingReportList = Mapper.Map<List<AccountsReceivableAgeingReport>, List<AccountsReceivableAgeingReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingReportForSA(FDate, TDate, CompanyCode));
                    }
                    else
                    {
                        accountsReceivableAgeingReportList = Mapper.Map<List<AccountsReceivableAgeingReport>, List<AccountsReceivableAgeingReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingReport(FDate, TDate, CompanyCode));
                    }
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
                    AppUA _appUA = Session["AppUA"] as AppUA;
                    List<AccountsReceivableAgeingSummaryReportViewModel> AccountsReceivableAgeingSummaryList = new List<AccountsReceivableAgeingSummaryReportViewModel>();
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);

                    string[] arr = _appUA.RolesCSV.Split(',');
                    if (arr.Contains("SAdmin") || arr.Contains("CEO"))
                    {
                        AccountsReceivableAgeingSummaryList = Mapper.Map<List<AccountsReceivableAgeingSummaryReport>, List<AccountsReceivableAgeingSummaryReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingSummaryReportForSA(FDate, TDate, CompanyCode));
                    }
                    else
                    {
                        AccountsReceivableAgeingSummaryList = Mapper.Map<List<AccountsReceivableAgeingSummaryReport>, List<AccountsReceivableAgeingSummaryReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingSummaryReport(FDate, TDate, CompanyCode));
                    }
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

            selectListItem = new List<SelectListItem>();
            List<SuppliersViewModel> supplierList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_supplierBusiness.GetAllSuppliers());
            if (supplierList != null)
            {
                foreach (SuppliersViewModel Supp in supplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Supp.CompanyName,
                        Value = Supp.ID.ToString(),
                        Selected = false
                    });
                }
                accountsPayableAgeingReportViewModel.supplierList = selectListItem;
            }
                return View(accountsPayableAgeingReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetAccountsPayableAgeingDetails(string FromDate, string ToDate, string CompanyCode,string[] SupplierIDs)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<AccountsPayableAgeingReportViewModel> accountsPayableAgeingReportList = Mapper.Map<List<AccountsPayableAgeingReport>, List<AccountsPayableAgeingReportViewModel>>(_reportBusiness.GetAccountsPayableAgeingReport(FDate, TDate, CompanyCode, SupplierIDs != null ? String.Join(",", SupplierIDs) : "ALL"));
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

            selectListItem = new List<SelectListItem>();
            List<SuppliersViewModel> supplierList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_supplierBusiness.GetAllSuppliers());
            if (supplierList != null)
            {
                foreach (SuppliersViewModel Supp in supplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Supp.CompanyName,
                        Value = Supp.ID.ToString(),
                        Selected = false
                    });
                }
                accountsPayableAgeingSummaryReportViewModel.supplierList = selectListItem;
            }
            return View(accountsPayableAgeingSummaryReportViewModel);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetAccountsPayableAgeingSummary(string FromDate, string ToDate, string CompanyCode, string[] SupplierIDs)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<AccountsPayableAgeingSummaryReportViewModel> accountsPayableAgeingSummaryReportList = Mapper.Map<List<AccountsPayableAgeingSummaryReport>, List<AccountsPayableAgeingSummaryReportViewModel>>(_reportBusiness.GetAccountsPayableAgeingSummaryReport(FDate, TDate, CompanyCode, SupplierIDs != null ? String.Join(",", SupplierIDs) : "ALL"));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = accountsPayableAgeingSummaryReportList });
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
        public ActionResult EmployeeExpenseSummary()
        {

            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            EmployeeExpenseSummaryReportViewModel employeeExpenseSummaryReportViewModel = new EmployeeExpenseSummaryReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            employeeExpenseSummaryReportViewModel.employeesList = Mapper.Map<List<EmployeeType>, List<EmployeeTypeViewModel>>(_employeeBusiness.GetAllEmployeeTypes());
            if (employeeExpenseSummaryReportViewModel.employeesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });
                selectListItem.Add(new SelectListItem
                {
                    Text = "Employee Wise",
                    Value = "employeewise",
                    Selected = false
                });
                foreach (EmployeeTypeViewModel evm in employeeExpenseSummaryReportViewModel.employeesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = evm.Name,
                        Value = evm.Code.ToString(),
                        Selected = false
                    });
                }
            }

            employeeExpenseSummaryReportViewModel.EmployeeList = selectListItem;
            return View(employeeExpenseSummaryReportViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public string GetEmployeeExpenseSummary(string FromDate, string ToDate, string EmployeeCode, string OrderBy)
        {
            if (!string.IsNullOrEmpty(EmployeeCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<EmployeeExpenseSummaryReportViewModel> employeeExpenseSummaryReportList = Mapper.Map<List<EmployeeExpenseSummaryReport>, List<EmployeeExpenseSummaryReportViewModel>>(_reportBusiness.GetEmployeeExpenseSummary(FDate, TDate, EmployeeCode, OrderBy));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = employeeExpenseSummaryReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "EmployeeCode is required" });
        }

        /// <summary>
        /// To Get Deposit And Withdrawal Details in Report
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawalDetailReport", Mode = "R")]
        public ActionResult DepositAndWithdrawalDetail()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            DepositsAndWithdrawalsDetailsReportViewModel DWVM = new DepositsAndWithdrawalsDetailsReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();


            List<BankViewModel> bankList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks()).ToList();
            if (bankList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = false
                });

                foreach (BankViewModel cvm in bankList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
            DWVM.bankObj = new BankViewModel();
            DWVM.bankObj.BanksList = selectListItem;
            return View(DWVM);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawalDetailReport", Mode = "R")]
        public string GetDepositAndWithdrawalDetail(string FromDate, string ToDate, string BankCode, string search)
        {
            if (!string.IsNullOrEmpty(BankCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<DepositsAndWithdrawalsDetailsReportViewModel> DepositsAndWithdrawalsDetailsReport = Mapper.Map<List<DepositsAndWithdrawalsDetailsReport>, List<DepositsAndWithdrawalsDetailsReportViewModel>>(_reportBusiness.GetDepositAndWithdrawalDetail(FDate, TDate, BankCode, search));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = DepositsAndWithdrawalsDetailsReport });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }

            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "BankCode is required" });
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerPaymentLedgerReport", Mode = "R")]
        public ActionResult CustomerPaymentLedger()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            CustomerPaymentLedgerViewModel CustomerPayments = new CustomerPaymentLedgerViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
            if (customerList != null)
            {
                //selectListItem.Add(new SelectListItem
                //{
                //    Text = "All",
                //    Value = "ALL",
                //    Selected = true
                //});

                foreach (CustomerViewModel Cust in customerList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Cust.CompanyName,
                        Value = Cust.ID.ToString(),
                        Selected = false
                    });
                }

                CustomerPayments.customerList = selectListItem;
                
            }
            return View(CustomerPayments);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerPaymentLedgerReport", Mode = "R")]
        public string GetCustomerPaymentLedger(string FromDate, string ToDate, string[] CustomerIDs)
        {
            //if (!string.IsNullOrEmpty(CustomerCode))
            //{
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<CustomerPaymentLedgerViewModel> customerpaymentledgerList = Mapper.Map<List<CustomerPaymentLedger>, List<CustomerPaymentLedgerViewModel>>(_reportBusiness.GetCustomerPaymentLedger(FDate, TDate, CustomerIDs!=null?String.Join(",", CustomerIDs):"ALL"));
               
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = customerpaymentledgerList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            //}
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CustomerCode is required" });
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierPaymentLedgerReport", Mode = "R")]
        public ActionResult SupplierPaymentLedger()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            SupplierPaymentLedgerViewModel SupplierPayments = new SupplierPaymentLedgerViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<SuppliersViewModel> supplierList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_supplierBusiness.GetAllSuppliers());
            if (supplierList != null)
            {
                //selectListItem.Add(new SelectListItem
                //{
                //    Text = "All",
                //    Value = "ALL",
                //    Selected = true
                //});

                foreach (SuppliersViewModel Supp in supplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Supp.CompanyName,
                        Value = Supp.ID.ToString(),
                        Selected = false
                    });
                }

                SupplierPayments.supplierList = selectListItem;

            }
            return View(SupplierPayments);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierPaymentLedgerReport", Mode = "R")]
        public string GetSupplierPaymentLedger(string FromDate, string ToDate, string[] Suppliercode)
        {
            //if (!string.IsNullOrEmpty(CustomerCode))
            //{
            try
            {
                DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                List<SupplierPaymentLedgerViewModel> supplierpaymentledgerList = Mapper.Map<List<SupplierPaymentLedger>, List<SupplierPaymentLedgerViewModel>>(_reportBusiness.GetSupplierPaymentLedger(FDate, TDate, Suppliercode!=null? String.Join(",", Suppliercode):"ALL"));

                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierpaymentledgerList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }

            //}
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "SupplierCode is required" });
        }

        /// <summary>
        /// To Get Other Income Summary in Report
        /// </summary>
        /// <returns></returns>


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncomeReport", Mode = "R")]
        public ActionResult OtherIncomeSummary()
        {

            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            OtherIncomeSummaryReportViewModel otherIncomeSummaryReportViewModel = new OtherIncomeSummaryReportViewModel();

            List<SelectListItem> selectListItem = new List<SelectListItem>();

            otherIncomeSummaryReportViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (otherIncomeSummaryReportViewModel.companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (CompaniesViewModel cvm in otherIncomeSummaryReportViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }

            otherIncomeSummaryReportViewModel.CompanyList = selectListItem;
            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            List<ChartOfAccountsViewModel> chartOfAccountList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_otherExpenseBusiness.GetAllAccountTypes("OI"));
            selectListItem.Add(new SelectListItem
            {
                Text = "All",
                Value = "ALL",
                Selected = true
            });

            foreach (ChartOfAccountsViewModel cav in chartOfAccountList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = cav.TypeDesc,
                    Value = cav.Code + ":" + cav.ISEmploy,
                    Selected = false,


                });
            }
            otherIncomeSummaryReportViewModel.AccountHeadList = selectListItem;

            return View(otherIncomeSummaryReportViewModel);
        }



        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncomeReport", Mode = "R")]
        public string GetOtherIncomeSummary(string FromDate, string ToDate, string CompanyCode, string accounthead, string search)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherIncomeSummaryReportViewModel> otherIncomeSummaryReportList = Mapper.Map<List<OtherIncomeSummaryReport>, List<OtherIncomeSummaryReportViewModel>>(_reportBusiness.GetOtherIncomeSummary(FDate, TDate, CompanyCode, accounthead.Split(':')[0], search));

                    decimal otherIncomeSum = otherIncomeSummaryReportList.Sum(OE => OE.Amount);
                    string otherIncomeSumFormatted = _commonBusiness.ConvertCurrency(otherIncomeSum, 2);


                    return JsonConvert.SerializeObject(new { Result = "OK", Records = otherIncomeSummaryReportList, TotalAmount = otherIncomeSumFormatted });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncomeReport", Mode = "R")]
        public ActionResult OtherIncomeDetails()
        {

            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            OtherIncomeDetailsReportViewModel otherIncomeDetailsViewModel = new OtherIncomeDetailsReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            otherIncomeDetailsViewModel.companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            if (otherIncomeDetailsViewModel.companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (CompaniesViewModel cvm in otherIncomeDetailsViewModel.companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }

            otherIncomeDetailsViewModel.CompanyList = selectListItem;
            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            List<ChartOfAccountsViewModel> chartOfAccountList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_otherExpenseBusiness.GetAllAccountTypes("OI"));
            selectListItem.Add(new SelectListItem
            {
                Text = "All",
                Value = "ALL",
                Selected = true
            });
            foreach (ChartOfAccountsViewModel cav in chartOfAccountList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = cav.TypeDesc,
                    Value = cav.Code + ":" + cav.ISEmploy,
                    Selected = false,


                });
            }
            otherIncomeDetailsViewModel.AccountHeadList = selectListItem;
            selectListItem = null;

            return View(otherIncomeDetailsViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncomeReport", Mode = "R")]
        public string GetOtherIncomeDetails(string FromDate, string ToDate, string CompanyCode, string accounthead, string search)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherIncomeDetailsReportViewModel> otherIncomeDetailsReportList = Mapper.Map<List<OtherIncomeDetailsReport>, List<OtherIncomeDetailsReportViewModel>>(_reportBusiness.GetOtherIncomeDetails(FDate, TDate, CompanyCode, accounthead.Split(':')[0], search));
                    decimal otherIncomeDetailsSum = otherIncomeDetailsReportList.Sum(OE => OE.Amount);
                    string otherIncomeDetailsSumFormatted = _commonBusiness.ConvertCurrency(otherIncomeDetailsSum, 2);
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = otherIncomeDetailsReportList, TotalAmount = otherIncomeDetailsSumFormatted });
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
        public ActionResult CustomerPaymentExpeditingDetails()
        {

            DateTime dt = DateTime.Now;
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");

            return View();
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetCustomerPaymentExpeditingDetails(string ToDate)
        {
            try
            { 
            DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
            List<CustomerExpeditingReportViewModel> customerExpeditingDetailsList = Mapper.Map<List<CustomerExpeditingReport>, List<CustomerExpeditingReportViewModel>>(_reportBusiness.GetCustomerExpeditingDetail(TDate));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = customerExpeditingDetailsList});
            }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message});
                }

            //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Date is required" });
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult SupplierPaymentExpeditingDetails()
        {

            DateTime dt = DateTime.Now;
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");

            return View();
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetSupplierPaymentExpeditingDetails(string ToDate)
        {
            try
            {
                DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                List<SupplierExpeditingReportViewModel> supplierExpeditingDetailsList = Mapper.Map<List<SupplierExpeditingReport>, List<SupplierExpeditingReportViewModel>>(_reportBusiness.GetSupplierExpeditingDetail(TDate));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierExpeditingDetailsList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }

            //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Date is required" });
        }



        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DailyLedgerReport", Mode = "R")]
        public ActionResult DailyLedgerDetails()
        {
            Permission _permission =  Session["UserRights"] as Permission;
            if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "SingleDateFilter").AccessCode : string.Empty).Contains("R"))
            {
                ViewBag.DateFilterDisplay = "display:none";
                ViewBag.SingleDateFilterDisplay = "display:block";
                ViewBag.Disabled = "disabled";
            }
            if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "DateFilter").AccessCode : string.Empty).Contains("R"))
            {
                ViewBag.DateFilterDisplay = "display:block";
                ViewBag.SingleDateFilterDisplay = "display:none";
                
            }
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            ViewBag.ondate = dt.ToString("dd-MMM-yyyy");
            
            DailyLedgerReportViewModel dailyLedgerViewModel = new DailyLedgerReportViewModel();
            return View(dailyLedgerViewModel);

        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DailyLedgerReport", Mode = "R")]
        public string GetDailyLedgerDetails(string FromDate, string ToDate, string OnDate, string MainHead, string search)
        {
            try
            {
                DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                DateTime? NDate = string.IsNullOrEmpty(OnDate) ? (DateTime?)null : DateTime.Parse(OnDate);
                List< DailyLedgerReportViewModel > dailyLedgerList = Mapper.Map<List< DailyLedgerReport >, List<DailyLedgerReportViewModel>>(_reportBusiness.GetDailyLedgerDetails(FDate, TDate,NDate,MainHead,search));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = dailyLedgerList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
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

                case "ListWithReset":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = false;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    break;

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion

    }
}