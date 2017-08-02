using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using UserInterface.Models;
using AutoMapper;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;

namespace UserInterface.Controllers
{
    public class DashboardController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        IDashboardBusiness _dashboardBusiness;
        IOtherExpenseBusiness _otherExpenseBusiness;

        public DashboardController(IDashboardBusiness dashboardBusiness, IOtherExpenseBusiness otherExpenseBusiness)
        {
            _dashboardBusiness = dashboardBusiness;
            _otherExpenseBusiness = otherExpenseBusiness;

        }
        #endregion Constructor_Injection 


        // GET: Dashboard
        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult Admin()
        {
            return View();
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult MonthlyRecap(string Company)
        {
            MonthlyRecapViewModel data = new MonthlyRecapViewModel();   

            data = Mapper.Map<MonthlyRecap, MonthlyRecapViewModel>(_dashboardBusiness.GetMonthlyRecap(Company));
            data.CompanyName = Company;

            return PartialView("_MontlyRecap", data);
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult ExpenseSummary(ExpenseSummaryViewModel d)
        {
            if (d.month == 0) {
                d.month = DateTime.Today.Month;
                d.year = DateTime.Today.Year;
                d.CompanyName = "ALL";
            }
            ExpenseSummaryViewModel data = new ExpenseSummaryViewModel();
            data.OtherExpSummary= Mapper.Map<OtherExpSummary, OtherExpSummaryViewModel>(_otherExpenseBusiness.GetOtherExpSummary(d.month,d.year,d.CompanyName));
            data.CompanyName = d.CompanyName;
            return PartialView("_ExpenseSummary", data);
        }

        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult OutstandingSummary(string Company)
        {
            OutstandingSummaryViewModel data = new OutstandingSummaryViewModel();
            data.CompanyName = Company;
            return PartialView("_OutstandingSummary", data);
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult TopCustomers(string Company)
        {
            TopCustomersViewModel data = new TopCustomersViewModel();
            data.CompanyName = Company;
            return PartialView("_TopCustomers", data);
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult TopSuppliers(string Company)
        {
            TopSuppliersViewModel data = new TopSuppliersViewModel();
            data.CompanyName = Company;
            return PartialView("_TopSuppliers", data);
        }


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentCustomerInvoice(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();

            data.DocType = "CINV";
            data.Title = "Recent Customer Invoices";
            data.Color = "bg-yellow";
            data.BaseURL =  "../CustomerInvoices/Index";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType,"ALL", data.BaseURL));


            return PartialView("_RecentDocs", data);
        }

         


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentCustomerPayments(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "CPAY";
            data.Title = "Recent Customer Payments";
            data.Color = "bg-yellow";
            data.BaseURL = "../CustomerPayments/Index";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL));

            return PartialView("_RecentDocs", data);
        }
        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentSupplierInvoice(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "SINV";
            data.Title = "Recent Supplier Invoices";
            data.Color = "bg-green";
            data.BaseURL = "../SupplierInvoices/Index";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL));
            return PartialView("_RecentDocs", data);
        }

        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentSupplierPayments(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "SPAY";
            data.Title = "Recent Supplier Payments";
            data.Color = "bg-green";
            data.BaseURL = "../SupplierPayments/Index";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL));
            return PartialView("_RecentDocs", data);
        }


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentOtherIncome(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "OI";
            data.Title = "Recent Other Income";
            data.Color = "bg-yellow";
            data.BaseURL = "../OtherIncome/Index";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL));
            return PartialView("_RecentDocs", data);
        }


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentOtherExpense(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "OE";
            data.Title = "Recent Other Expense";
            data.Color = "bg-green";
            data.BaseURL = "../OtherExpenses/Index";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL));
            return PartialView("_RecentDocs", data);
        }



        private ActionResult RedirectToAdminDashboard()
        {
            return RedirectToAction("Admin", "DashBoard");
        }
    }
}