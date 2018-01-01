using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class ReportController : Controller
    {
        AppConst c = new AppConst();
        IReportBusiness _reportBusiness;
        ICustomerBusiness _customerBusiness;
        ISupplierBusiness _supplierBusiness;
        IChartOfAccountsBusiness _chartOfAccountsBusiness;
        ICompaniesBusiness _companiesBusiness;
        IBankBusiness _bankBusiness;
        IEmployeeBusiness _employeeBusiness;
        IOtherExpenseBusiness _otherExpenseBusiness;
        ICommonBusiness _commonBusiness;
        SecurityFilter.ToolBarAccess _tool;
        public ReportController(IReportBusiness reportBusiness, ICompaniesBusiness companiesBusiness,IEmployeeBusiness employeeBusiness, IOtherExpenseBusiness otherExpenseBusiness, ICommonBusiness commonBusiness, IBankBusiness bankbusiness, ICustomerBusiness customerBusiness, ISupplierBusiness supplierBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _reportBusiness = reportBusiness;
            _supplierBusiness = supplierBusiness;
             _companiesBusiness = companiesBusiness;
            _employeeBusiness = employeeBusiness;
            _otherExpenseBusiness = otherExpenseBusiness;
            _commonBusiness = commonBusiness;
            _bankBusiness = bankbusiness;
            _customerBusiness = customerBusiness;
            _tool = tool; 

        }
        // GET: Report

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public string GetAllAccountTypes(string Type)
        {
            string type = (Type == "ALL") ? "OE" : Type;
            try
            {
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<ChartOfAccountsViewModel> chartOfAccountList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_otherExpenseBusiness.GetAllAccountTypes(type));
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL"
                });
                foreach (ChartOfAccountsViewModel cav in chartOfAccountList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cav.TypeDesc,
                        Value = cav.Code + ":" + cav.ISEmploy
                    });
                }
                //var empList = employeeViewModelList != null ? employeeViewModelList.Select(i => new { i.ID, i.Name }).ToList() : null;
                var accountsList = selectListItem != null? selectListItem.Select(i => new { i.Text, i.Value }).ToList() : null;
                return JsonConvert.SerializeObject(new { Result = "OK", Records = accountsList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
            
        }

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
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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
                   
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = saleObj.saleSummaryList, TotalAmount = saleObj.salesummarySum, InvoicedAmount = saleObj.salesummaryTotalInvoice, PaidAmount= saleObj.salesummarypaid,TaxAmount=saleObj.salesummaryTax,Invoiced= saleObj.salesummaryInvoiced });
                
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
        public string GetSaleDetail(string FromDate, string ToDate, string CompanyCode,string search, Boolean IsInternal, Boolean IsTax,string Customer,string InvoiceType)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    SaleDetailReportViewModel SaledetailObj = Mapper.Map<SaleDetailReport,SaleDetailReportViewModel>(_reportBusiness.GetSaleDetail(FDate, TDate, CompanyCode,search,IsInternal,IsTax, Guid.Parse(Customer), InvoiceType));
                    
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = SaledetailObj.saleDetailList, TotalAmount = SaledetailObj.saledetailSum, InvoicedAmount= SaledetailObj.saledetailinvoice, PaidAmount= SaledetailObj.saledetailpaid,TaxAmount= SaledetailObj.saledetailtax,TotalInvoiced= SaledetailObj.saledetailtotalinvoiced });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }

        #region GetRPTViewCustomerDetail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SalesReport", Mode = "R")]
        public string GetRPTViewCustomerDetail(string FromDate, string ToDate, string CompanyCode,string Customer)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    SaleDetailReportViewModel SaledetailObj = null;
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    SaledetailObj = Mapper.Map<SaleDetailReport, SaleDetailReportViewModel>(_reportBusiness.GetRPTViewCustomerDetail(FDate, TDate, CompanyCode,Guid.Parse(Customer)));

                    return JsonConvert.SerializeObject(new { Result = "OK", Records = SaledetailObj.saleDetailList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }
        #endregion GetRPTViewCustomerDetail

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SalesReport", Mode = "R")]
        public ActionResult SalesDetail()
        {

            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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

            selectListItem = new List<SelectListItem>();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
            if (customerList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (CustomerViewModel Cust in customerList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Cust.CompanyName,
                        Value = Cust.ID.ToString(),
                        Selected = false
                    });
                }
            }
            saleDetailReportViewModel.customerList = selectListItem;
            return View(saleDetailReportViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public ActionResult OtherExpenseSummary()
        {

            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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
        public string GetOtherExpenseSummary(string FromDate, string ToDate, string CompanyCode,string ReportType, string OrderBy,string accounthead, string subtype,string employeeorother,string employeecompany,string search, string ExpenseType)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherExpenseSummaryReportViewModel> otherExpenseSummaryReportList = Mapper.Map<List<OtherExpenseSummaryReport>, List<OtherExpenseSummaryReportViewModel>>(_reportBusiness.GetOtherExpenseSummary(FDate, TDate, CompanyCode,ReportType, OrderBy,accounthead.Split(':')[0], subtype, employeeorother, employeecompany, search, ExpenseType));
                   
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

            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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
        public string GetOtherExpenseDetails(string FromDate, string ToDate, string CompanyCode, string OrderBy, string accounthead, string subtype, string employeeorother,string employeecompany, string search,string ExpenseType)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherExpenseDetailsReportViewModel> otherExpenseDetailsReportList = Mapper.Map<List<OtherExpenseDetailsReport>, List<OtherExpenseDetailsReportViewModel>>(_reportBusiness.GetOtherExpenseDetails(FDate, TDate, CompanyCode,OrderBy, accounthead.Split(':')[0], subtype, employeeorother, employeecompany,search, ExpenseType));
                    decimal otherExpenseDetailsSum = otherExpenseDetailsReportList.Where(OE=>OE.RowType=="N").Sum(OE => OE.Amount);
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
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public string GetOtherExpenseDetailsReport(string FromDate, string ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string employeecompany,string ExpenseType)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherExpenseDetailsReportViewModel> otherExpenseDetailsReportList = Mapper.Map<List<OtherExpenseDetailsReport>, List<OtherExpenseDetailsReportViewModel>>(_reportBusiness.GetOtherExpenseDetails(FDate, TDate, CompanyCode, null, accounthead.Split(':')[0], subtype, employeeorother, employeecompany,null, ExpenseType));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseDetailsReportList});
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
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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

        #region GetPurchaseDetails
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public string GetPurchaseDetails(string FromDate, string ToDate, string CompanyCode, string search, Boolean IsInternal,string Supplier,string InvoiceType,string SubType, string AccountCode)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    PurchaseDetailReportViewModel purchasedetailObj = Mapper.Map<PurchaseDetailReport, PurchaseDetailReportViewModel>(_reportBusiness.GetPurchaseDetails(FDate, TDate, CompanyCode, search, IsInternal, Guid.Parse(Supplier), InvoiceType, Guid.Parse(SubType), AccountCode ));

                    return JsonConvert.SerializeObject(new { Result = "OK", Records = purchasedetailObj.purchaseDetailReportList, TotalAmount= purchasedetailObj.purchaseDetailSum, InvoicedAmount = purchasedetailObj.purchaseDetailInvoice, PaidAmount = purchasedetailObj.purchaseDetailPaid,PaymentProcessed = purchasedetailObj.purchaseDetailPaymentProcess,TaxAmount=purchasedetailObj.purchaseDetailsTaxAmount,TotalInvoice=purchasedetailObj.purchaseDetailsTotalAmount });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }
        #endregion GetPurchaseDetails

        #region GetRPTViewPurchaseDetail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SalesReport", Mode = "R")]
        public string GetRPTViewPurchaseDetail(string FromDate, string ToDate, string CompanyCode, string Supplier)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    PurchaseDetailReportViewModel detailObj = null;
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    detailObj = Mapper.Map<PurchaseDetailReport, PurchaseDetailReportViewModel>(_reportBusiness.GetRPTViewPurchaseDetail(FDate, TDate, CompanyCode, Guid.Parse(Supplier)));

                    return JsonConvert.SerializeObject(new { Result = "OK", Records = detailObj.purchaseDetailReportList });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }

            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CompanyCode is required" });
        }
        #endregion GetRPTViewPurchaseDetail

        #region PurchaseDetails
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public ActionResult PurchaseDetails()
        {

            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            PurchaseDetailReportViewModel purchaseDetailReportViewModel = new PurchaseDetailReportViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();


            List<ChartOfAccountsViewModel> chartOfAccountList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_otherExpenseBusiness.GetAllAccountTypes("SUP"));
            if (chartOfAccountList != null)
            {
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
            }
            purchaseDetailReportViewModel.AccountList = selectListItem;
            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            List<EmployeeViewModel> employeeViewModelList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_otherExpenseBusiness.GetAllEmployeesByType("OTH"));
            if (employeeViewModelList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });
                foreach (EmployeeViewModel EVM in employeeViewModelList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = EVM.Name,
                        Value = EVM.ID.ToString(),
                        Selected = false,
                    });
                }
            }
            purchaseDetailReportViewModel.SubTypeList = selectListItem;
            selectListItem = null;
            selectListItem = new List<SelectListItem>();


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

            selectListItem = new List<SelectListItem>();
            List<SuppliersViewModel> supplierList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_supplierBusiness.GetAllSuppliers());
            if (supplierList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (SuppliersViewModel Supp in supplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Supp.CompanyName,
                        Value = Supp.ID.ToString(),
                        Selected = false
                    });
                }
            }
            purchaseDetailReportViewModel.supplierList = selectListItem;
            return View(purchaseDetailReportViewModel);
        }
        #endregion PurchaseDetails

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
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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

        #region AccountsReceivableAgeingDetails

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult AccountsReceivableAgeingDetails()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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

            selectListItem = new List<SelectListItem>();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
            if (customerList != null)
            {
                foreach (CustomerViewModel Cust in customerList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Cust.CompanyName,
                        Value = Cust.ID.ToString(),
                        Selected = false
                    });
                }
            }

            accountsReceivableAgeingReportViewModel.customerList = selectListItem;

            return View(accountsReceivableAgeingReportViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetAccountsReceivableAgeingDetails(string FromDate, string ToDate, string CompanyCode, string[] Customerids)
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
                        accountsReceivableAgeingReportList = Mapper.Map<List<AccountsReceivableAgeingReport>, List<AccountsReceivableAgeingReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingReportForSA(FDate, TDate, CompanyCode, Customerids != null ? String.Join(",", Customerids) : "ALL"));
                    }
                    else
                    {
                        accountsReceivableAgeingReportList = Mapper.Map<List<AccountsReceivableAgeingReport>, List<AccountsReceivableAgeingReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingReport(FDate, TDate, CompanyCode, Customerids != null ? String.Join(",", Customerids) : "ALL"));
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

        #endregion AccountsReceivableAgeingDetails

        #region AccountsReceivableAgeingSummary

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult AccountsReceivableAgeingSummary()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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

            selectListItem = new List<SelectListItem>();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
            if (customerList != null)
            {
                foreach (CustomerViewModel Cust in customerList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Cust.CompanyName,
                        Value = Cust.ID.ToString(),
                        Selected = false
                    });
                }
            }

            accountsReceivableAgeingSummaryReportViewModel.customerList = selectListItem;
            return View(accountsReceivableAgeingSummaryReportViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetAccountsReceivableAgeingSummary(string FromDate, string ToDate, string CompanyCode, string[] Customerids)
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
                        AccountsReceivableAgeingSummaryList = Mapper.Map<List<AccountsReceivableAgeingSummaryReport>, List<AccountsReceivableAgeingSummaryReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingSummaryReportForSA(FDate, TDate, CompanyCode, Customerids != null ? String.Join(",", Customerids) : "ALL"));
                    }
                    else
                    {
                        AccountsReceivableAgeingSummaryList = Mapper.Map<List<AccountsReceivableAgeingSummaryReport>, List<AccountsReceivableAgeingSummaryReportViewModel>>(_reportBusiness.GetAccountsReceivableAgeingSummaryReport(FDate, TDate, CompanyCode, Customerids != null ? String.Join(",", Customerids) : "ALL"));
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

        #endregion AccountsReceivableAgeingSummary

        #region AccountsPayableAgeingDetails

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult AccountsPayableAgeingDetails()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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

        #endregion AccountsPayableAgeingDetails

        #region AccountsPayableAgeingSummary

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult AccountsPayableAgeingSummary()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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

        #endregion AccountsPayableAgeingSummary

        #region EmployeeExpenseSummary

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OEReport", Mode = "R")]
        public ActionResult EmployeeExpenseSummary()
        {

            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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

        #endregion EmployeeExpenseSummary


        /// <summary>
        /// To Get Deposit And Withdrawal Details in Report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawalDetailReport", Mode = "R")]
        public ActionResult DepositAndWithdrawalDetail()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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
            }
                CustomerPayments.customerList = selectListItem;

                CustomerPayments.CompanyList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<CompaniesViewModel> companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_otherExpenseBusiness.GetAllCompanies());
            if (companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (CompaniesViewModel cvm in companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
            }
                CustomerPayments.companiesList = selectListItem;

               return View(CustomerPayments);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerPaymentLedgerReport", Mode = "R")]
        public string GetCustomerPaymentLedger(string FromDate, string ToDate, string[] CustomerIDs, string Company)
        {
            //if (!string.IsNullOrEmpty(CustomerCode))
            //{
            try
            {
                DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                List<CustomerPaymentLedgerViewModel> customerpaymentledgerList = Mapper.Map<List<CustomerPaymentLedger>, List<CustomerPaymentLedgerViewModel>>(_reportBusiness.GetCustomerPaymentLedger(FDate, TDate, CustomerIDs != null ? String.Join(",", CustomerIDs) : "ALL", Company));

                return JsonConvert.SerializeObject(new { Result = "OK", Records = customerpaymentledgerList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }

            //}
            //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "CustomerCode is required" });
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierPaymentLedgerReport", Mode = "R")]
        public ActionResult SupplierPaymentLedger()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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
            }
                SupplierPayments.supplierList = selectListItem;


                SupplierPayments.CompanyList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<CompaniesViewModel> companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_otherExpenseBusiness.GetAllCompanies());
                if (companiesList != null)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = "All",
                        Value = "ALL",
                        Selected = true
                    });

                    foreach (CompaniesViewModel cvm in companiesList)
                    {
                        selectListItem.Add(new SelectListItem
                        {
                            Text = cvm.Name,
                            Value = cvm.Code.ToString(),
                            Selected = false
                        });
                    }
                
                SupplierPayments.companiesList = selectListItem;

            }
            return View(SupplierPayments);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierPaymentLedgerReport", Mode = "R")]
        public string GetSupplierPaymentLedger(string FromDate, string ToDate, string[] Suppliercode, string Company)
        {
            //if (!string.IsNullOrEmpty(CustomerCode))
            //{
            try
            {
                DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                List<SupplierPaymentLedgerViewModel> supplierpaymentledgerList = Mapper.Map<List<SupplierPaymentLedger>, List<SupplierPaymentLedgerViewModel>>(_reportBusiness.GetSupplierPaymentLedger(FDate, TDate, Suppliercode!=null? String.Join(",", Suppliercode):"ALL",Company));

                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierpaymentledgerList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }

            //}
            //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "SupplierCode is required" });
        }

        /// <summary>
        /// To Get Other Income Summary in Report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncomeReport", Mode = "R")]
        public ActionResult OtherIncomeSummary()
        {

            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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
            otherIncomeSummaryReportViewModel.EmployeeTypeList = selectListItem;


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
            otherIncomeSummaryReportViewModel.EmployeeList = selectListItem;




            return View(otherIncomeSummaryReportViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncomeReport", Mode = "R")]
        public string GetOtherIncomeSummary(string FromDate, string ToDate, string CompanyCode, string accounthead,string subtype, string employeeorother, string search)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherIncomeSummaryReportViewModel> otherIncomeSummaryReportList = Mapper.Map<List<OtherIncomeSummaryReport>, List<OtherIncomeSummaryReportViewModel>>(_reportBusiness.GetOtherIncomeSummary(FDate, TDate, CompanyCode, accounthead.Split(':')[0],subtype,employeeorother, search));

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

            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
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

            //
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
            otherIncomeDetailsViewModel.EmployeeTypeList = selectListItem;

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
            otherIncomeDetailsViewModel.EmployeeList = selectListItem;

            return View(otherIncomeDetailsViewModel);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncomeReport", Mode = "R")]
        public string GetOtherIncomeDetails(string FromDate, string ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string search)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherIncomeDetailsReportViewModel> otherIncomeDetailsReportList = Mapper.Map<List<OtherIncomeDetailsReport>, List<OtherIncomeDetailsReportViewModel>>(_reportBusiness.GetOtherIncomeDetails(FDate, TDate, CompanyCode, accounthead.Split(':')[0],subtype,employeeorother, search));
                    decimal otherIncomeDetailsSum = otherIncomeDetailsReportList.Where(OE => OE.RowType == "N").Sum(OE => OE.Amount);
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
        [AuthSecurityFilter(ProjectObject = "OtherIncomeReport", Mode = "R")]
        public string GetOtherIncomeDetailsReport(string FromDate, string ToDate, string CompanyCode, string accounthead,string subtype, string employeeorother)
        {
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                try
                {
                    DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                    DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                    List<OtherIncomeDetailsReportViewModel> otherIncomeDetailsReportList = Mapper.Map<List<OtherIncomeDetailsReport>, List<OtherIncomeDetailsReportViewModel>>(_reportBusiness.GetOtherIncomeDetails(FDate, TDate, CompanyCode, accounthead.Split(':')[0],subtype,employeeorother, null));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = otherIncomeDetailsReportList});
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
        public ActionResult CustomerPaymentExpeditingDetails(string id)
        {

            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            CustomerExpeditingListViewModel Result = new CustomerExpeditingListViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem {Text = "--Select--", Value = "ALL", Selected = false});
            selectListItem.Add(new SelectListItem { Text = "Coming Week", Value = "ThisWeek", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "Today", Value = "Today", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "1-30 Days", Value = "1To30", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "31-60 Days", Value = "31To60", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "61-90 Days", Value = "61To90", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "90 Above", Value = "90Above", Selected = false });

            if (id == null || id == "")
            {
                var selected = selectListItem.Where(x => x.Value == "ALL").First();
                selected.Selected = true;
            }
            else
            {
                try
                {
                    var selected = selectListItem.Where(x => x.Value == id).First();
                    selected.Selected = true;
                }
                catch (Exception)
                {

                    Result.Filter = "ALL";
                }

            }

            Result.BasicFilters = selectListItem;

            selectListItem = new List<SelectListItem>();
            Result.customerObj = new CustomerViewModel();
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
                        Value = Cust.CompanyName.ToString(),
                        Selected = false
                    });
                }
            }
            Result.customerObj.CustomerList = selectListItem;

            selectListItem = new List<SelectListItem>();
            Result.companyObj = new CompaniesViewModel();
            List<CompaniesViewModel> companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_otherExpenseBusiness.GetAllCompanies());
            if (companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (CompaniesViewModel cvm in companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Name.ToString(),
                        Selected = false
                    });
                }
            }            
            Result.companyObj.CompanyList = selectListItem;




            return View(Result);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetCustomerPaymentExpeditingDetails(string ToDate,string Filter,string Company, string[] Customer)       
        {
            try
            {               
                DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                CustomerExpeditingListViewModel Result = new CustomerExpeditingListViewModel();
                Result.customerExpeditingDetailsList = Mapper.Map<List<CustomerExpeditingReport>, List<CustomerExpeditingReportViewModel>>(_reportBusiness.GetCustomerExpeditingDetail(TDate,Filter,Company,Customer != null ? String.Join(",", Customer) : "ALL"));              
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Result });
            }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message});
                }

            //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Date is required" });
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public ActionResult SupplierPaymentExpeditingDetails(string id)
        {

            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");

            SupplierExpeditingListViewModel Result = new SupplierExpeditingListViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem { Text = "--Select--", Value = "ALL", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "Coming Week", Value = "ThisWeek", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "Today", Value = "Today", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "1-30 Days", Value = "1To30", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "31-60 Days", Value = "31To60", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "61-90 Days", Value = "61To90", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "90 Above", Value = "90Above", Selected = false });

            if (id == null || id == "")
            {
                var selected = selectListItem.Where(x => x.Value == "ALL").First();
                selected.Selected = true;
            }
            else
            {
                try
                {
                    var selected = selectListItem.Where(x => x.Value == id).First();
                    selected.Selected = true;
                }
                catch (Exception)
                {

                    Result.Filter = "ALL";
                }

            }

            Result.BasicFilters = selectListItem;


            return View(Result);


            
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AgeingReport", Mode = "R")]
        public string GetSupplierPaymentExpeditingDetails(string ToDate, string Filter)
        {
            try
            {
                DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                SupplierExpeditingListViewModel Result = new SupplierExpeditingListViewModel();
                Result.SupplierExpeditingDetailsList = Mapper.Map<List<SupplierExpeditingReport>, List<SupplierExpeditingReportViewModel>>(_reportBusiness.GetSupplierExpeditingDetail(TDate, Filter));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Result });
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
            DailyLedgerReportViewModel DL = new DailyLedgerReportViewModel();
            Permission _permission = Session["UserRights"] as Permission;
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
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.DateTime;
            ViewBag.fromdate = dt.ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            ViewBag.ondate = dt.ToString("dd-MMM-yyyy");

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            DL.BanksList = new List<SelectListItem>();
            List<BankViewModel> BanksList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
            if (BanksList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (BankViewModel BL in BanksList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = BL.Name,
                        Value = BL.Code,
                        Selected = false
                    });
                }
            }

                DL.BanksList= selectListItem;
                return View(DL);
            
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DailyLedgerReport", Mode = "R")]
        public string GetDailyLedgerDetails(string FromDate, string ToDate, string OnDate, string MainHead, string search,string Bank)
        {
            try
            {
                DateTime? FDate = string.IsNullOrEmpty(FromDate) ? (DateTime?)null : DateTime.Parse(FromDate);
                DateTime? TDate = string.IsNullOrEmpty(ToDate) ? (DateTime?)null : DateTime.Parse(ToDate);
                DateTime? NDate = string.IsNullOrEmpty(OnDate) ? (DateTime?)null : DateTime.Parse(OnDate);
                List< DailyLedgerReportViewModel > dailyLedgerList = Mapper.Map<List< DailyLedgerReport >, List<DailyLedgerReportViewModel>>(_reportBusiness.GetDailyLedgerDetails(FDate, TDate,NDate,MainHead,search,Bank));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = dailyLedgerList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #region TrialBalanceReport
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "TrialBalanceReport", Mode = "R")]
        public ActionResult TrialBalance()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            ViewBag.Currentdate = _appUA.DateTime.ToString("dd-MMM-yyyy");

            return View(); 
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "TrialBalanceReport", Mode = "R")]
        public string GetTrialBalanceReport(string Date)
        {
            try
            {
                DateTime? FDate = string.IsNullOrEmpty(Date) ? (DateTime?)null : DateTime.Parse(Date);
                List<TrialBalanceViewModel> TBlist =Mapper.Map<List<TrialBalance>, List<TrialBalanceViewModel>>(_reportBusiness.GetTrialBalanceReport(FDate));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = TBlist });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion TrialBalanceReport



        #region ButtonStyling
        [HttpGet]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            Permission _permission = Session["UserRights"] as Permission;

            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = false;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";
                   
                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);
                    break;
                case "CustDetail":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = false;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "closeNav();";

                    //ToolboxViewModelObj.PrintBtn.Visible = true;
                    //ToolboxViewModelObj.PrintBtn.Text = "Export";
                    //ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                   
                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);
                    break;

                case "ListWithReset":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = false;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";                
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";

                   
                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);

                    break;

                case "ListWithPrint":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    ToolboxViewModelObj.downloadBtn.Visible = true;
                    ToolboxViewModelObj.downloadBtn.Text = "Download";
                    ToolboxViewModelObj.downloadBtn.Title = "Download";
                    ToolboxViewModelObj.downloadBtn.Event = "DownloadReport();";

                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);

                    break;


                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}