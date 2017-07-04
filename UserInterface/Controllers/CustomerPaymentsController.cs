using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Controllers
{
    public class CustomerPaymentsController : Controller
    {
        // GET: CustomerPayments
        public ActionResult Index()
        {
            return View();
        }
    }
}