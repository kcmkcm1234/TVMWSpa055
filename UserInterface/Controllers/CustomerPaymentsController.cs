using AutoMapper;
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
    public class CustomerPaymentsController : Controller
    {
        #region Constructor_Injection 

        AppConst c = new AppConst();
        IMasterBusiness _masterBusiness;
        ICustomerPaymentsBusiness _CustPaymentBusiness; 

        public CustomerPaymentsController(ICustomerPaymentsBusiness custPaymentBusiness, IMasterBusiness masterBusiness)
        {
            _CustPaymentBusiness = custPaymentBusiness;
            _masterBusiness = masterBusiness;
        }
        #endregion Constructor_Injection 
        // GET: CustomerPayments
        public ActionResult Index()
        {
            CustomerPaymentsViewModel CP = new CustomerPaymentsViewModel(); 

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<CustomerViewModel> CustList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_masterBusiness.GetAllCustomers());
            foreach (CustomerViewModel Cust in CustList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Cust.CompanyName,
                    Value = Cust.ID.ToString(),
                    Selected = false
                });
            }
            CP.customerObj.CustomerList = selectListItem;
            return View();
        }


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