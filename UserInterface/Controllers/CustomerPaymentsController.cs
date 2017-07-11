using AutoMapper;
using Newtonsoft.Json;
using SAMTool.DataAccessObject.DTO;
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
        ICustomerPaymentsBusiness _CustPaymentBusiness;
        ICustomerBusiness _customerBusiness;
        IBankBusiness _bankBusiness;
        ICompaniesBusiness _companiesBusiness;
        IPaymentModesBusiness _paymentmodesBusiness;
        ICustomerInvoicesBusiness _customerInvoicesBusiness;

        public CustomerPaymentsController(ICustomerPaymentsBusiness custPaymentBusiness,
            IPaymentModesBusiness paymentmodeBusiness,
            ICustomerBusiness customerBusiness,IBankBusiness bankBusiness,ICompaniesBusiness companiesBusiness,
            ICustomerInvoicesBusiness customerInvoicesBusiness)
        {
            _CustPaymentBusiness = custPaymentBusiness;
            _paymentmodesBusiness = paymentmodeBusiness;
            _customerInvoicesBusiness = customerInvoicesBusiness;
            _customerBusiness = customerBusiness;
            _bankBusiness = bankBusiness;
            _companiesBusiness = companiesBusiness;
        }
        #endregion Constructor_Injection 

        #region Index
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

            CP.PaymentModesObj = new PaymentModesViewModel();
            CP.PaymentModesObj.PaymentModesList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<PaymentModesViewModel> PaymentModeList = Mapper.Map<List<PaymentModes>, List<PaymentModesViewModel>>(_paymentmodesBusiness.GetAllPaymentModes());
            foreach (PaymentModesViewModel PMVM in PaymentModeList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = PMVM.Description,
                    Value = PMVM.Code,
                    Selected = false
                });
            }
            CP.PaymentModesObj.PaymentModesList = selectListItem;


            CP.bankObj = new BankViewModel();
            CP.bankObj.BanksList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<BankViewModel> BankList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
            foreach (BankViewModel BL in BankList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = BL.Name,
                    Value = BL.Code,
                    Selected = false
                });
            }
            CP.bankObj.BanksList = selectListItem;

            CP.CompanyObj = new CompaniesViewModel();
            CP.CompanyObj.CompanyList  = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<CompaniesViewModel> CompaniesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            foreach (CompaniesViewModel BL in CompaniesList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = BL.Name,
                    Value = BL.Code,
                    Selected = false
                });
            }
            CP.CompanyObj.CompanyList= selectListItem;   
            return View(CP);
        }
        #endregion Index

        #region GetAllCustomerPayments
        [HttpGet]
       
        public string GetAllCustomerPayments(string FromDate, string ToDate)
        { 
            List<CustomerPaymentsViewModel> CustPayList = Mapper.Map<List<CustomerPayments>, List<CustomerPaymentsViewModel>>(_CustPaymentBusiness.GetAllCustomerPayments());
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CustPayList });
        }
        #endregion GetAllCustomerPayments

        #region GetAllCustomerPaymentsByID

        [HttpGet]
        public string GetCustomerPaymentsByID(string ID)
        {
            CustomerPaymentsViewModel custpaylist = Mapper.Map<CustomerPayments, CustomerPaymentsViewModel>(_CustPaymentBusiness.GetCustomerPaymentsByID(ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = custpaylist });

        }
        #endregion GetAllCustomerPaymentsByID

        #region  GetOutStandingInvoices

        [HttpGet]
        public string GetOutStandingInvoices(string ID)
        {
            List<CustomerInvoicesViewModel> List = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetOutStandingInvoices(Guid.Parse(ID)));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = List });

        }
        #endregion  GetOutStandingInvoices

        #region InsertUpdatePayments

        [HttpPost]
        public string InsertUpdatePayments(CustomerPaymentsViewModel _customerObj)
        {
            try
            {
                AppUA ua = new AppUA();
               
                _customerObj.commonObj = new CommonViewModel();
                _customerObj.commonObj.CreatedBy = ua.UserName;
                _customerObj.commonObj.CreatedDate = DateTime.Now;
                _customerObj.commonObj.UpdatedBy = ua.UserName;
                _customerObj.commonObj.UpdatedDate = DateTime.Now;
                CustomerPaymentsViewModel CIVM = Mapper.Map<CustomerPayments, CustomerPaymentsViewModel>(_CustPaymentBusiness.InsertUpdatePayments(Mapper.Map<CustomerPaymentsViewModel, CustomerPayments>(_customerObj)));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CIVM });
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion InsertUpdatePayments


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
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";



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