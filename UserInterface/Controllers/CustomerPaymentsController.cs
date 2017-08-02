using AutoMapper;
using Newtonsoft.Json;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
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
        ICustomerCreditNotesBusiness _customerCreditNotesBusiness;
        IPaymentModesBusiness _paymentmodesBusiness;
        ICustomerInvoicesBusiness _customerInvoicesBusiness;

        public CustomerPaymentsController(ICustomerPaymentsBusiness custPaymentBusiness,
            IPaymentModesBusiness paymentmodeBusiness,
            ICustomerBusiness customerBusiness,IBankBusiness bankBusiness,ICompaniesBusiness companiesBusiness,
            ICustomerInvoicesBusiness customerInvoicesBusiness, ICustomerCreditNotesBusiness CustomerCreditNotesBusiness)
        {
            _CustPaymentBusiness = custPaymentBusiness;
            _paymentmodesBusiness = paymentmodeBusiness;
            _customerInvoicesBusiness = customerInvoicesBusiness;
            _customerBusiness = customerBusiness;
            _bankBusiness = bankBusiness;
            _customerCreditNotesBusiness = CustomerCreditNotesBusiness;
            _companiesBusiness = companiesBusiness;
        }
        #endregion Constructor_Injection 

        #region Index
        // GET: CustomerPayments
        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "R")]
        [HttpGet]
        public ActionResult Index()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            ViewBag.Currentdate = _appUA.DateTime.ToString("dd-MMM-yyyy");

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            CustomerPaymentsViewModel CP = new CustomerPaymentsViewModel();
            //-------------1.CustomerList-------------------//
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
            //-------------2.PaymentModes-------------------//
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
            //-------------3.BanksList-------------------//
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
            //-------------4.CompanyList-------------------//
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
        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "R")]
        [HttpGet]       
        public string GetAllCustomerPayments(string FromDate, string ToDate)
        { 
            List<CustomerPaymentsViewModel> CustPayList = Mapper.Map<List<CustomerPayments>, List<CustomerPaymentsViewModel>>(_CustPaymentBusiness.GetAllCustomerPayments());
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CustPayList });
        }
        #endregion GetAllCustomerPayments

        #region GetAllCustomerPaymentsByID
        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "R")]
        [HttpGet]
        public string GetCustomerPaymentsByID(string ID)
        {
            CustomerPaymentsViewModel custpaylist = Mapper.Map<CustomerPayments, CustomerPaymentsViewModel>(_CustPaymentBusiness.GetCustomerPaymentsByID(ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = custpaylist });

        }
        #endregion GetAllCustomerPaymentsByID

        #region  GetOutStandingInvoices
        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "R")]
        [HttpGet]
        public string GetOutStandingInvoices(string PaymentID,string CustID)
        {
            List<CustomerInvoicesViewModel> List = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetOutStandingInvoices(Guid.Parse(PaymentID), Guid.Parse(CustID)));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = List });

        }
        #endregion  GetOutStandingInvoices

        #region InsertUpdatePayments

        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "W")]
        [HttpPost]
        public string InsertUpdatePayments(CustomerPaymentsViewModel _customerObj)
        {
            try
            {
                if (_customerObj.TotalRecdAmt == 0 && _customerObj.Type == "C" || _customerObj.hdfType == "C")
                {
                    _customerObj.TotalRecdAmt = Decimal.Parse(_customerObj.hdfCreditAmount);
                    _customerObj.AdvanceAmount = 0;
                }
                AppUA _appUA = Session["AppUA"] as AppUA;
                if (_customerObj.paymentDetailhdf!=null)
                _customerObj.CustomerPaymentsDetail = JsonConvert.DeserializeObject<List<CustomerPaymentsDetailViewModel>>(_customerObj.paymentDetailhdf);
                _customerObj.commonObj = new CommonViewModel();
                _customerObj.commonObj.CreatedBy = _appUA.UserName;
                _customerObj.commonObj.CreatedDate = _appUA.DateTime;
                _customerObj.commonObj.UpdatedBy = _appUA.UserName;
                _customerObj.commonObj.UpdatedDate = _appUA.DateTime;
                CustomerPaymentsViewModel CPVM = Mapper.Map<CustomerPayments, CustomerPaymentsViewModel>(_CustPaymentBusiness.InsertUpdatePayments(Mapper.Map<CustomerPaymentsViewModel, CustomerPayments>(_customerObj)));
                if (_customerObj.ID != null && _customerObj.ID != Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = CPVM });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CPVM });
                }
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion InsertUpdatePayments
      
        #region DeletePayments 
        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "D")]
        [HttpPost]
        public string DeletePayments(CustomerPaymentsViewModel _customerpayObj)
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            object result = null;
            try
            {
                result = _CustPaymentBusiness.DeletePayments(_customerpayObj.ID, _appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess, Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion DeletePayments
        
        #region InsertPaymentAdjustment
        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "W")]
        [HttpPost]
        public string InsertPaymentAdjustment(CustomerPaymentsViewModel _custpayObj)
        {
            try
            {               
               AppUA _appUA = Session["AppUA"] as AppUA;
               _custpayObj.commonObj = new CommonViewModel();
               _custpayObj.commonObj.CreatedBy = _appUA.UserName;
               _custpayObj.commonObj.CreatedDate = _appUA.DateTime; 
               CustomerPaymentsViewModel CPVM = Mapper.Map<CustomerPayments, CustomerPaymentsViewModel>(_CustPaymentBusiness.InsertPaymentAdjustment(Mapper.Map<CustomerPaymentsViewModel, CustomerPayments>(_custpayObj)));
               return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CPVM });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdatePayments

        #region GetCreditNoteByCustomer
        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "R")]
        [HttpGet]
        public string GetCreditNoteByCustomer(string ID)
        {
            List<CustomerCreditNoteViewModel> CreditList = Mapper.Map<List<CustomerCreditNotes>, List<CustomerCreditNoteViewModel>>(_customerCreditNotesBusiness.GetCreditNoteByCustomer(Guid.Parse(ID)));

            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditList });
        }
        #endregion GetCreditNoteByCustomer

        #region GetCreditNoteAmount
        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "R")]
        [HttpGet]
        public string GetCreditNoteAmount(string CreditID,string CustomerID)
        {
            CustomerCreditNoteViewModel CreditNote = Mapper.Map<CustomerCreditNotes,CustomerCreditNoteViewModel>(_customerCreditNotesBusiness.GetCreditNoteAmount(Guid.Parse(CreditID), Guid.Parse(CustomerID)));

            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNote });

        }
        #endregion GetCreditNoteAmount



        #region GetOutstandingAmountByCustomer
        [AuthSecurityFilter(ProjectObject = "CustomerPayments", Mode = "R")]
        [HttpGet]
        public string GetOutstandingAmountByCustomer(string CreditID, string CustomerID)
        {
            CustomerPaymentsViewModel Cus_pay = Mapper.Map<CustomerPayments, CustomerPaymentsViewModel>(_CustPaymentBusiness.GetOutstandingAmountByCustomer(CustomerID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = Cus_pay });
        }
        #endregion GetOutstandingAmountByCustomer

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

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeletePayments();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "savePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";


                    break;
                case "Add":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeletePayments();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "savePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion

    }
}