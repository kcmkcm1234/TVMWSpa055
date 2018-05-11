using AutoMapper;
using Newtonsoft.Json;
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
    public class SpecialInvPaymentsController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
       ICustomerBusiness _customerBusiness;
        IPaymentModesBusiness _paymentmodesBusiness;
        ISpecialInvPaymentsBusiness _SpecialInvPaymentsBusiness;
        ICompaniesBusiness _CompaniesBusiness;

       public SpecialInvPaymentsController(ICustomerBusiness customerBusiness,IPaymentModesBusiness PaymentmodesBusiness, ISpecialInvPaymentsBusiness SpecialInvPaymentsBusiness, ICompaniesBusiness CompaniesBusiness)
        {
            _customerBusiness = customerBusiness;
            _paymentmodesBusiness = PaymentmodesBusiness;
            _SpecialInvPaymentsBusiness = SpecialInvPaymentsBusiness;
            _CompaniesBusiness = CompaniesBusiness;
        }


        #endregion Constructor_Injection 

        // GET: CustomerPbPayments
        #region Index
        // GET: CustomerPayments
        [AuthSecurityFilter(ProjectObject = "CustomerPBPayments", Mode = "R")]
        [HttpGet]
        public ActionResult Index(string id)
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
         //   ViewBag.Currentdate = _appUA.DateTime.ToString("dd-MMM-yyyy");
            ViewBag.value = id;

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            SpecialInvPaymentsViewModel CP = new SpecialInvPaymentsViewModel();
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
            //-------------4.CompanyList-------------------//
            CP.companiesObj = new CompaniesViewModel();
            CP.companiesObj.CompanyList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<CompaniesViewModel> CompaniesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_CompaniesBusiness.GetAllCompanies());
            foreach (CompaniesViewModel BL in CompaniesList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = BL.Name,
                    Value = BL.Code,
                    Selected = false
                });
            }
            CP.companiesObj.CompanyList = selectListItem;
            return View(CP);
        }
        #endregion Index

        #region  GetSpecialInvPayments
        [AuthSecurityFilter(ProjectObject = "CustomerPBPayments", Mode = "R")]
        [HttpGet]
        public string GetSpecialInvPayments(string PaymentID,string ID)
        {
            List<SpecialInvPaymentsViewModel> List = Mapper.Map<List<SpecialInvPayments>, List<SpecialInvPaymentsViewModel>>(_SpecialInvPaymentsBusiness.GetSpecialInvPayments(Guid.Parse(PaymentID), Guid.Parse(ID)));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = List });

        }
        #endregion  GetSpecialInvPayments

        #region GetOutstandingSpecialAmountByCustomer
        [AuthSecurityFilter(ProjectObject = "CustomerPBPayments", Mode = "R")]
        [HttpGet]
        public string GetOutstandingSpecialAmountByCustomer(string ID)
        {
            SpecialInvPaymentsViewModel SpecialInv = Mapper.Map<SpecialInvPayments, SpecialInvPaymentsViewModel>(_SpecialInvPaymentsBusiness.GetOutstandingSpecialAmountByCustomer(ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = SpecialInv });

        }
        #endregion GetOutstandingSpecialAmountByCustomer

        #region CheckReferenceNo
       
        [HttpPost]
        public string Validate(SpecialInvPaymentsViewModel SpecialInvObj)
        {
            AppUA _appUA = Session["AppUA"] as  AppUA;
            object result = null;
            try
            {
                result = _SpecialInvPaymentsBusiness.Validate(Mapper.Map<SpecialInvPaymentsViewModel, SpecialInvPayments>(SpecialInvObj));
                return JsonConvert.SerializeObject(new { Result="OK",Message="",Records=result});
            }
            catch(Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Meassage = cm.Message, status = -1 });
            }
        }
        #endregion CheckReferenceNo

        #region InsertUpdatePayments

        [AuthSecurityFilter(ProjectObject = "CustomerPBPayments", Mode = "W")]
        [HttpPost]
        public string InsertUpdatePayments(SpecialInvPaymentsViewModel SpecialInvObj)
        {
            try
            {
                if (SpecialInvObj.specialDetailObj.PaidAmount == 0)
                {
                    throw new Exception("Please Enter Amount");
                }
                AppUA _appUA = Session["AppUA"] as AppUA;
                if (SpecialInvObj.hdfpaymentDetail != null)
                SpecialInvObj.specialList = JsonConvert.DeserializeObject<List<SpecialInvPaymentsDetailViewModel>>(SpecialInvObj.hdfpaymentDetail);
                SpecialInvObj.commonObj = new CommonViewModel();
                SpecialInvObj.commonObj.CreatedBy = _appUA.UserName;
                SpecialInvObj.commonObj.CreatedDate = _appUA.DateTime;
                SpecialInvObj.commonObj.UpdatedBy = _appUA.UserName;
                SpecialInvObj.commonObj.UpdatedDate = _appUA.DateTime; ;
                SpecialInvPaymentsViewModel SPIN = Mapper.Map<SpecialInvPayments, SpecialInvPaymentsViewModel>(_SpecialInvPaymentsBusiness.InsertUpdateSpecialInvPayments(Mapper.Map<SpecialInvPaymentsViewModel, SpecialInvPayments>(SpecialInvObj)));
     
                if (SpecialInvObj.ID != null && SpecialInvObj.ID != Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = SPIN });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = SPIN });
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
        [AuthSecurityFilter(ProjectObject = "CustomerPBPayments", Mode = "D")]
        [HttpPost]
        public string DeleteSpecialPayments(SpecialInvPaymentsViewModel specialObj)
        {
            object result = null;
            try
            {
                result = _SpecialInvPaymentsBusiness.DeleteSpecialPayments(specialObj.GroupID);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess, Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion DeletePayments 

        #region GetSpecialPaymentsDetails
        [AuthSecurityFilter(ProjectObject = "CustomerPBPayments", Mode = "R")]
        [HttpGet]
        public string GetSpecialPaymentsDetails(string GroupID)
        {
            try
            {
                
                List<SpecialInvPaymentsViewModel> InvoiceObjList = Mapper.Map <List<SpecialInvPayments>, List<SpecialInvPaymentsViewModel>>(_SpecialInvPaymentsBusiness.GetSpecialInvPaymentsDetails(GroupID != null && GroupID != "" ? Guid.Parse(GroupID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = InvoiceObjList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetSpecialPaymentsDetails

        #region GetAllSpecialInvPayments
        [AuthSecurityFilter(ProjectObject = "CustomerPBPayments", Mode = "R")]
        public string GetAllSpecialInvPayments(string SpecialPaymentsSearchObject)
        {
            try
            {
                SpecialPaymentsSearch SpecialPaymentsAdvancedSearchObj = SpecialPaymentsSearchObject != null ? JsonConvert.DeserializeObject<SpecialPaymentsSearch>(SpecialPaymentsSearchObject) : new SpecialPaymentsSearch();
                List<SpecialInvPaymentsViewModel> SpecialList = Mapper.Map<List<SpecialInvPayments>, List<SpecialInvPaymentsViewModel>>(_SpecialInvPaymentsBusiness.GetAllSpecialInvPayments(SpecialPaymentsAdvancedSearchObj));
             
                return JsonConvert.SerializeObject(new { Result = "OK", Records = SpecialList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllSpecialInvPayments

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

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";


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
                    ToolboxViewModelObj.savebtn.Event = "SavePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

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
                    ToolboxViewModelObj.savebtn.Event = "SavePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion ButtonStyling
    }
}