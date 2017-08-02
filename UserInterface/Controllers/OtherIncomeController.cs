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
        ICommonBusiness _commonBusiness;

        public OtherIncomeController(IOtherIncomeBusiness otherIncomeBusiness, ICustomerBusiness customerBusiness, IChartOfAccountsBusiness chartOfAccountsBusiness,IBankBusiness bankBusiness, ICompaniesBusiness companiesBusiness, IPaymentModesBusiness paymentModeBusiness,ICommonBusiness commonBusiness)
        {
            _otherIncomeBusiness = otherIncomeBusiness;
            _customerBusiness = customerBusiness;
            _chartOfAccountsBusiness = chartOfAccountsBusiness;
            _bankBusiness = bankBusiness;
            _companiesBusiness = companiesBusiness;
            _paymentModeBusiness = paymentModeBusiness;
            _commonBusiness = commonBusiness;
        }
        #endregion Constructor_Injection 

        // GET: OtherIncome
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
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
                List<PaymentModesViewModel> PaymentList = Mapper.Map<List<PaymentModes>, List<PaymentModesViewModel>>(_paymentModeBusiness.GetAllPaymentModes());
                foreach (PaymentModesViewModel PMVM in PaymentList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = PMVM.Description,
                        Value = PMVM.Code,
                        Selected = false
                    });
                }
                otherIncomeViewModalObj.paymentModeList = selectListItem;

                otherIncomeViewModalObj.accountCodeList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<ChartOfAccountsViewModel> AccountList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_chartOfAccountsBusiness.GetChartOfAccountsByType("OI"));
                foreach (ChartOfAccountsViewModel COAVM in AccountList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = COAVM.TypeDesc,
                        Value = COAVM.Code,
                        Selected = false
                    });
                }
                otherIncomeViewModalObj.accountCodeList = selectListItem;

            }
            catch(Exception ex)
            {
                throw ex;
            }           

            return View(otherIncomeViewModalObj);
        }

        #region GetAllOtherIncome
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public string GetAllOtherIncome(string IncomeDate,string DefaultDate)
        {
            try
            {

                List<OtherIncomeViewModel> otherIncomeList = Mapper.Map<List<OtherIncome>, List<OtherIncomeViewModel>>(_otherIncomeBusiness.GetAllOtherIncome(IncomeDate,DefaultDate));
                var totalAmt= otherIncomeList.Sum(amt => amt.Amount);              
                string totalAmtFormatted = _commonBusiness.ConvertCurrency(totalAmt, 2);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherIncomeList,TotalAmt= totalAmtFormatted });
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
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
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
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "W")]
        public string InsertUpdateOtherIncome(OtherIncomeViewModel _otherIncomeObj)
        {
            try
            {

                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                _otherIncomeObj.commonObj = new CommonViewModel();
                _otherIncomeObj.commonObj.CreatedBy = _appUA.UserName;
                _otherIncomeObj.commonObj.CreatedDate = _appUA.DateTime;
                _otherIncomeObj.commonObj.UpdatedBy = _appUA.UserName;
                _otherIncomeObj.commonObj.UpdatedDate = _appUA.DateTime;

                result = _otherIncomeBusiness.InsertUpdateOtherIncome(Mapper.Map<OtherIncomeViewModel, OtherIncome>(_otherIncomeObj));
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
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "D")]
        public string DeleteOtherIncome(string ID)
        {

            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                result = _otherIncomeBusiness.DeleteOtherIncome(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty,_appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteOtherIncome

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
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