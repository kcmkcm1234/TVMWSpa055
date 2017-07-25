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
    public class OtherIncomeController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        ICustomerBusiness _customerBusiness;
        IOtherIncomeBusiness _otherIncomeBusiness;
        IChartOfAccountsBusiness _chartOfAccountsBusiness;
        IBankBusiness _bankBusiness;
        ICompaniesBusiness _companiesBusiness;
        IPaymentModesBusiness _paymentModeBusiness;

        public OtherIncomeController(IOtherIncomeBusiness otherIncomeBusiness, ICustomerBusiness customerBusiness, IChartOfAccountsBusiness chartOfAccountsBusiness,IBankBusiness bankBusiness, ICompaniesBusiness companiesBusiness, IPaymentModesBusiness paymentModeBusiness)
        {
            _otherIncomeBusiness = otherIncomeBusiness;
            _customerBusiness = customerBusiness;
            _chartOfAccountsBusiness = chartOfAccountsBusiness;
            _bankBusiness = bankBusiness;
            _companiesBusiness = companiesBusiness;
            _paymentModeBusiness = paymentModeBusiness;
        }
        #endregion Constructor_Injection 

        // GET: OtherIncome
        public ActionResult Index()
        {
            OtherIncomeViewModel otherIncomeViewModalObj = null;
            try
            {
                otherIncomeViewModalObj = new OtherIncomeViewModel();
                otherIncomeViewModalObj.accountCodeList = new List<SelectListItem>();


                List<SelectListItem> selectListItem = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<BankViewModel> PayTermList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
                foreach (BankViewModel bvm in PayTermList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = bvm.Name,
                        Value = bvm.Code,
                        Selected = false
                    });
                }
                otherIncomeViewModalObj.bankList = selectListItem;

                otherIncomeViewModalObj.companiesList = new List<SelectListItem>();
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
                otherIncomeViewModalObj.companiesList = selectListItem;

                otherIncomeViewModalObj.paymentModeList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<PaymentModesViewModel> PaymentModeList = Mapper.Map<List<PaymentModes>, List<PaymentModesViewModel>>(_paymentModeBusiness.GetAllPaymentModes());
                foreach (PaymentModesViewModel PMVM in PaymentModeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = PMVM.Description,
                        Value = PMVM.Code,
                        Selected = false
                    });
                }
                otherIncomeViewModalObj.paymentModeList = selectListItem;

            }
            catch(Exception ex)
            {
                throw ex;
            }           

            return View(otherIncomeViewModalObj);
        }

        #region GetAllOtherIncome
        [HttpGet]
        public string GetAllOtherIncome(string IncomeDate)
        {
            try
            {

                List<OtherIncomeViewModel> otherIncomeList = Mapper.Map<List<OtherIncome>, List<OtherIncomeViewModel>>(_otherIncomeBusiness.GetAllOtherIncome(IncomeDate));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherIncomeList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllOtherIncome

        #region GetOtherIncomeDetails
        [HttpGet]
        public string GetOtherIncomeDetails(string ID)
        {
            try
            {

                OtherIncomeViewModel otherIncomeObj = Mapper.Map<OtherIncome, OtherIncomeViewModel>(_otherIncomeBusiness.GetOtherIncomeDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherIncomeObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetOtherIncomeDetails

        #region InsertUpdateOtherIncome
        [HttpPost]
        public string InsertUpdateOtherIncome(OtherIncomeViewModel _otherIncomeObj)
        {
            try
            {

                object result = null;
                AppUA ua = new AppUA();

                result = _otherIncomeBusiness.InsertUpdateOtherIncome(Mapper.Map<OtherIncomeViewModel, OtherIncome>(_otherIncomeObj), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateOtherIncome

        #region DeleteOtherIncome
        public string DeleteOtherIncome(string ID)
        {

            try
            {
                object result = null;

                result = _otherIncomeBusiness.DeleteOtherIncome(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteOtherIncome

        #region GetChartOfAccountsByType
        [HttpGet]
        public string GetChartOfAccountsByType(string type)
        {
            OtherIncomeViewModel otherIncomeViewModel = null;
            try
            {
                otherIncomeViewModel = new OtherIncomeViewModel();
                List<ChartOfAccountsViewModel> accountCodeList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_chartOfAccountsBusiness.GetChartOfAccountsByType(type));
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                foreach (ChartOfAccountsViewModel cavm in accountCodeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cavm.TypeDesc,
                        Value = cavm.Code,
                        Selected = false
                    });
                }
                otherIncomeViewModel.accountCodeList = selectListItem;
                
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherIncomeViewModel });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetChartOfAccountsByType

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
                    ToolboxViewModelObj.addbtn.Event = "ShowModal();";



                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    break;
                case "Edit":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "ShowModal();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Other Income";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Other Income";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

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