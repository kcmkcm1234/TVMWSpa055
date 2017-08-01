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
        

        public DashboardController(IDashboardBusiness dashboardBusiness)
        {
            _dashboardBusiness = dashboardBusiness;
            
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
        public ActionResult ExpenseSummary(string Company)
        {
            ExpenseSummaryViewModel data = new ExpenseSummaryViewModel();
            data.CompanyName = Company;
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
            data.DocType = "CustomerInvoice";
            data.Title = "Recent Customer Invoices";
            data.Color = "bg-yellow";
            return PartialView("_RecentDocs", data);
        }

        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentCustomerPayments(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "CustomerPayments";
            data.Title = "Recent Customer Payments";
            data.Color = "bg-yellow";
            return PartialView("_RecentDocs", data);
        }
        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentSupplierInvoice(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "SupplierInvoice";
            data.Title = "Recent Supplier Invoices";
            data.Color = "bg-green";
            return PartialView("_RecentDocs", data);
        }

        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentSupplierPayments(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "SupplierPayments";
            data.Title = "Recent Supplier Payments";
            data.Color = "bg-green";
            return PartialView("_RecentDocs", data);
        }


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentOtherIncome(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "OtherIncome";
            data.Title = "Recent Other Income";
            data.Color = "bg-yellow";
            return PartialView("_RecentDocs", data);
        }


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentOtherExpense(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "OtherExpense";
            data.Title = "Recent Other Expense";
            data.Color = "bg-green";
            return PartialView("_RecentDocs", data);
        }



        private ActionResult RedirectToAdminDashboard()
        {
            return RedirectToAction("Admin", "DashBoard");
        }
    }
}