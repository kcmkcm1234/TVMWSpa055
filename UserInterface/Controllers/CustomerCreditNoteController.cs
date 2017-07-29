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
        ICompaniesBusiness _companiesBusiness;

        public CustomerCreditNoteController(ICustomerCreditNotesBusiness customerCreditNotesBusiness, ICustomerBusiness customerBusiness, ICompaniesBusiness companiesBusiness)
        {
            _CustomerCreditNotesBusiness = customerCreditNotesBusiness;
            _customerBusiness = customerBusiness;
            _companiesBusiness = companiesBusiness;
        }
        #endregion Constructor_Injection 

        // GET: CustomerCreditNote
        public ActionResult Index()
        {
            CustomerCreditNoteViewModel ccn = null;
            try
            {
                ccn = new CustomerCreditNoteViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
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
                ccn.CustomerList = selectListItem;

                ccn.CompaniesList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<CompaniesViewModel> companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
                foreach (CompaniesViewModel cvm in companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
                ccn.CompaniesList = selectListItem;

            }
            catch(Exception ex)
            {
                throw ex;
            }
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

        #region GetCustomerCreditNoteDetails
        [HttpGet]
        public string GetCustomerCreditNoteDetails(string ID)
        {
            try
            {

                CustomerCreditNoteViewModel customerCreditNoteObj = Mapper.Map<CustomerCreditNotes, CustomerCreditNoteViewModel>(_CustomerCreditNotesBusiness.GetCustomerCreditNoteDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = customerCreditNoteObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetCustomerCreditNoteDetails

        #region InsertUpdateCustomerCreditNote
        [HttpPost]
        public string InsertUpdateCustomerCreditNote(CustomerCreditNoteViewModel _customersCreditNoteObj)
        {
            try
            {

                object result = null;
                AppUA ua = new AppUA();

                result = _CustomerCreditNotesBusiness.InsertUpdateCustomerCreditNote(Mapper.Map<CustomerCreditNoteViewModel, CustomerCreditNotes>(_customersCreditNoteObj), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateCustomerCreditNote

        #region DeleteCustomerCreditNote
        public string DeleteCustomerCreditNote(string ID)
        {

            try
            {
                object result = null;

                result = _CustomerCreditNotesBusiness.DeleteCustomerCreditNote(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteCustomerCreditNote

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
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    break;
                case "Edit":
                    
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Credit Note";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Credit Note";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";
                    break;
                case "Add":

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

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