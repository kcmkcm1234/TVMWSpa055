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
    public class CustomerCreditNoteController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        ICustomerBusiness _customerBusiness;
        ICustomerCreditNotesBusiness _CustomerCreditNotesBusiness;

        public CustomerCreditNoteController(ICustomerCreditNotesBusiness customerCreditNotesBusiness, ICustomerBusiness customerBusiness)
        {
            _CustomerCreditNotesBusiness = customerCreditNotesBusiness;
            _customerBusiness = customerBusiness;
        }
        #endregion Constructor_Injection 

        // GET: CustomerCreditNote
        public ActionResult Index()
        {
            CustomerCreditNoteViewModel ccn = new CustomerCreditNoteViewModel();
            ccn.CustomerList = new List<SelectListItem>();
            return View(ccn);
        }

        #region GetAllCustomerCreditNotes
        [HttpGet]
        public string GetAllCustomerCreditNotes()
        {
            try
            {

                List<CustomerCreditNoteViewModel> CustomerCreditNotesList = Mapper.Map<List<CustomerCreditNotes>, List<CustomerCreditNoteViewModel>>(_CustomerCreditNotesBusiness.GetAllCustomerCreditNotes());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = CustomerCreditNotesList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllCustomerCreditNotes

        #region ButtonStyling
        [HttpGet]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";



                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    break;
                case "Edit":


                    break;
                case "Add":

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "saveNow();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    break;
                case "AddSub":

                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion

    }
}