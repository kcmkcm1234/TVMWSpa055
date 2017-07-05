﻿using Newtonsoft.Json;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UserInterface.Models;
using AutoMapper;

namespace UserInterface.Controllers
{
    public class CustomerInvoicesController : Controller
    {
        #region Constructor_Injection

        ICustomerInvoicesBusiness _customerInvoicesBusiness;
        AppConst c = new AppConst();
        IMasterBusiness _masterBusiness;
        ICustomerBusiness _customerBusiness;

        public CustomerInvoicesController(ICustomerInvoicesBusiness customerInvoicesBusiness,IMasterBusiness masterBusiness,ICustomerBusiness customerBusiness)
        {
            _customerInvoicesBusiness = customerInvoicesBusiness;
            _masterBusiness = masterBusiness;
            _customerBusiness = customerBusiness;
        }
        #endregion Constructor_Injection

        // GET: Invoices
        public ActionResult Index()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            CustomerInvoicesViewModel CI = new CustomerInvoicesViewModel();

            CI.CompanyList = new List<SelectListItem>();
            CI.TaxList = new List<SelectListItem>();
            CI.customerObj = new CustomerViewModel();
            CI.paymentTermsObj = new PaymentTermsVieModel();


            CI.customerObj.CustomerList= new List<SelectListItem>();            
            selectListItem = new List<SelectListItem>();
            List<CustomerViewModel> CustList= Mapper.Map<List<Customer>, List< CustomerViewModel >>(_customerBusiness.GetAllCustomers());
            foreach (CustomerViewModel Cust in CustList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Cust.CompanyName,
                    Value = Cust.ID.ToString(),
                    Selected = false
                });
            }
            CI.customerObj.CustomerList = selectListItem;

            CI.paymentTermsObj.PaymentTermsList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<PaymentTermsVieModel> PayTermList = Mapper.Map<List<PaymentTerms>, List<PaymentTermsVieModel>>(_masterBusiness.GetAllPayTerms());
            foreach (PaymentTermsVieModel PayT in PayTermList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = PayT.NoOfDays.ToString(),
                    Value = PayT.Code,
                    Selected = false
                });
            }
            CI.paymentTermsObj.PaymentTermsList = selectListItem;
            return View(CI);
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
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllInvoices

        [HttpPost]
        public string InsertUpdateInvoice(CustomerInvoicesViewModel _customerInvoicesObj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AppUA ua = new AppUA();
                    ua.UserName = "Thomson";
                    ua.UserID = Guid.Empty;
                    CustomerInvoicesViewModel CIVM = Mapper.Map<CustomerInvoice, CustomerInvoicesViewModel>(_customerInvoicesBusiness.InsertUpdateInvoice(Mapper.Map<CustomerInvoicesViewModel, CustomerInvoice>(_customerInvoicesObj), ua));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CIVM });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "Error", Message = c.InsertFailure});
                }
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
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
                    ToolboxViewModelObj.savebtn.Event = "$('#btnSave').click();";

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