using AutoMapper;
using Newtonsoft.Json;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class OtherExpensesController : Controller
    {
        // GET: OtherExpenses
        IOtherExpenseBusiness _otherExpenseBusiness;
        AppConst c = new AppConst();
        public OtherExpensesController(IOtherExpenseBusiness otherExpenseBusiness)
        {
            _otherExpenseBusiness = otherExpenseBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }
        #region GetAllSupplierCreditNotes
        [HttpGet]
        public string GetAllOtherExpenses()
        {
            try
            {

                List<OtherExpenseViewModel> otherExpenseViewModelList = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetAllOtherExpenses());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseViewModelList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllSupplierCreditNotes
    }
}