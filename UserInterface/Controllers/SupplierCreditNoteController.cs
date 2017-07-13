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
    public class SupplierCreditNoteController : Controller
    {
        ISupplierCreditBusines _supplierCreditBusines;
        AppConst c = new AppConst();
        public SupplierCreditNoteController(ISupplierCreditBusines supplierCreditBusines)
        {
            _supplierCreditBusines = supplierCreditBusines;
        }
        
        // GET: SupplierCredit
        public ActionResult Index()
        {
            SupplierCreditNoteViewModel supplierCreditNoteViewModel = new SupplierCreditNoteViewModel();
            supplierCreditNoteViewModel.SupplierList = new List<SelectListItem>();
            return View(supplierCreditNoteViewModel);
        }


        #region GetAllSupplierCreditNotes
        [HttpGet]
        public string GetAllSupplierCreditNotes()
        {
            try
            {

                List<SupplierCreditNoteViewModel> SupplierCreditNoteList = Mapper.Map<List<SupplierCreditNote>, List<SupplierCreditNoteViewModel>>(_supplierCreditBusines.GetAllSupplierCreditNotes());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = SupplierCreditNoteList });
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