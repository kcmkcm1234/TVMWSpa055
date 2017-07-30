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
        


        private ActionResult RedirectToAdminDashboard()
        {
            return RedirectToAction("Admin", "DashBoard");
        }
    }
}