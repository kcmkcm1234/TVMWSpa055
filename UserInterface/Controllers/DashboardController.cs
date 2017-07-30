using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class DashboardController : Controller
    {
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

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
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

        private ActionResult RedirectToAdminDashboard()
        {
            return RedirectToAction("Admin", "DashBoard");
        }
    }
}