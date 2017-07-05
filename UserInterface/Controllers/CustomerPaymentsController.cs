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
        ICustomerBusiness _customerBusiness;

        public CustomerPaymentsController(ICustomerPaymentsBusiness custPaymentBusiness, IMasterBusiness masterBusiness,ICustomerBusiness customerBusiness)
        {
            _CustPaymentBusiness = custPaymentBusiness;
            _masterBusiness = masterBusiness;
            _customerBusiness = customerBusiness;
        }
        #endregion Constructor_Injection 
        // GET: CustomerPayments
        public ActionResult Index()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            CustomerPaymentsViewModel CP = new CustomerPaymentsViewModel();

            CP.customerObj = new CustomerViewModel();
            
            CP.customerObj.CustomerList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<CustomerViewModel> CustList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
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

            CP.TransObj = new TransactionTypesViewModel();
            CP.TransObj.TransactionTypesList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<TransactionTypesViewModel> TransTypList = Mapper.Map<List<TransactionTypes>, List<TransactionTypesViewModel>>(_masterBusiness.GetAllTransactionTypes());
            foreach (TransactionTypesViewModel Trans in TransTypList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Trans.Name,
                    Value = Trans.Code,
                    Selected = false
                });
            }
            CP.TransObj.TransactionTypesList = selectListItem;
            return View(CP);
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