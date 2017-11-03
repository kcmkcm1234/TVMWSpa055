using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Controllers
{
    public class BankBalanceController : Controller
    {
        // GET: BankBalance
        public ActionResult Index()
        {
            return View();
        }
    }
}