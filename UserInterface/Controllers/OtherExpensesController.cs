using SPAccounts.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Controllers
{
    public class OtherExpensesController : Controller
    {
        // GET: OtherExpenses
        IOtherExpenseBusiness _otherExpenseBusiness;
        public OtherExpensesController(IOtherExpenseBusiness otherExpenseBusiness)
        {
            _otherExpenseBusiness = otherExpenseBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}