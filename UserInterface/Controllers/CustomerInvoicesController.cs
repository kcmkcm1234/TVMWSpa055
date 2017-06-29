using Newtonsoft.Json;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;
using AutoMapper;

namespace UserInterface.Controllers
{
    public class CustomerInvoicesController : Controller
    {
        #region Constructor_Injection

        ICustomerInvoicesBusiness _customerInvoicesBusiness;
        Const c = new Const();

        public CustomerInvoicesController(ICustomerInvoicesBusiness customerInvoicesBusiness)
        {
            _customerInvoicesBusiness = customerInvoicesBusiness;

        }
        #endregion Constructor_Injection

        // GET: Invoices
        public ActionResult Index()
        {
            return View();
        }


        #region GetAllInvoices
        [HttpGet]    
        public string GetInvoicesAndSummary()
        {
            try
            {

                CustomerInvoiceBundleViewModel Result = new CustomerInvoiceBundleViewModel();

                Result.CustomerInvoiceSummary= Mapper.Map<CustomerInvoiceSummary,CustomerInvoiceSummaryViewModel>(_customerInvoicesBusiness.GetCustomerInvoicesSummary());
                Result.CustomerInvoices = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetAllCustomerInvoices());


                return JsonConvert.SerializeObject(new { Result = "OK", Records = Result });
            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllForm8


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