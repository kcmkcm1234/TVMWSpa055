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
    public class DepositAndWithdrawalsController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();       
        IDepositAndWithdrawalsBusiness _depositAndWithdrawalsBusiness;       
        IBankBusiness _bankBusiness;       
        ICommonBusiness _commonBusiness;
        IPaymentModesBusiness _paymentModesBusiness;

        public DepositAndWithdrawalsController(IDepositAndWithdrawalsBusiness depositAndWithdrawalsBusiness,IBankBusiness bankBusiness, ICommonBusiness commonBusiness,IPaymentModesBusiness paymentModesBusiness)
        {
            _depositAndWithdrawalsBusiness = depositAndWithdrawalsBusiness;          
            _bankBusiness = bankBusiness;            
            _commonBusiness = commonBusiness;
            _paymentModesBusiness = paymentModesBusiness;
        }
        #endregion Constructor_Injection 

        // GET: DepositAndWithdrawals
        public ActionResult Index()
        {
            DepositAndWithdrwalViewModel    depositAndWithdrwalViewModelObj = null;
            try
            {
                depositAndWithdrwalViewModelObj = new DepositAndWithdrwalViewModel();
                depositAndWithdrwalViewModelObj.bankList = new List<SelectListItem>();


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
                depositAndWithdrwalViewModelObj.bankList = selectListItem;

                depositAndWithdrwalViewModelObj.paymentModeList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<PaymentModesViewModel> PaymentList = Mapper.Map<List<PaymentModes>, List<PaymentModesViewModel>>(_paymentModesBusiness.GetAllPaymentModes());
                foreach (PaymentModesViewModel PMVM in PaymentList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = PMVM.Description,
                        Value = PMVM.Code,
                        Selected = false
                    });
                }
                depositAndWithdrwalViewModelObj.paymentModeList = selectListItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(depositAndWithdrwalViewModelObj);
        }

        #region GetAllDepositAndWithdrawals
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public string GetAllDepositAndWithdrawals(string FromDate, string ToDate, string DepositOrWithdrawal,string chqclr)
        {
            try
            {

                List<DepositAndWithdrwalViewModel> depositAndWithdrwalsList = Mapper.Map<List<DepositAndWithdrawals>, List<DepositAndWithdrwalViewModel>>(_depositAndWithdrawalsBusiness.GetAllDepositAndWithdrawals(FromDate, ToDate, DepositOrWithdrawal, chqclr));
                var totalAmt = depositAndWithdrwalsList.Sum(amt => amt.Amount);
                string totalAmtFormatted = _commonBusiness.ConvertCurrency(totalAmt, 2);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = depositAndWithdrwalsList, TotalAmt = totalAmtFormatted });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllDepositAndWithdrawals

        #region GetDepositAndWithdrawalDetails
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public string GetDepositAndWithdrawalDetails(string ID)
        {
            try
            {

                DepositAndWithdrwalViewModel depositAndWithdrwalObj = Mapper.Map<DepositAndWithdrawals, DepositAndWithdrwalViewModel>(_depositAndWithdrawalsBusiness.GetDepositAndWithdrawalDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = depositAndWithdrwalObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetDepositAndWithdrawalDetails

        #region InsertUpdateDepositAndWithdrawals
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "W")]
        public string InsertUpdateDepositAndWithdrawals(DepositAndWithdrwalViewModel _depositAndWithdrwalObj)
        {
            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                _depositAndWithdrwalObj.CommonObj = new CommonViewModel();
                _depositAndWithdrwalObj.CommonObj.CreatedBy = _appUA.UserName;
                _depositAndWithdrwalObj.CommonObj.CreatedDate = _appUA.DateTime;
                _depositAndWithdrwalObj.CommonObj.UpdatedBy = _appUA.UserName;
                _depositAndWithdrwalObj.CommonObj.UpdatedDate = _appUA.DateTime;
               
                if (_depositAndWithdrwalObj.DepositRowValues != null && _depositAndWithdrwalObj.DepositRowValues!="0")
                {
                    _depositAndWithdrwalObj.CheckedRows  =  JsonConvert.DeserializeObject<List<DepositAndWithdrwalViewModel>>(_depositAndWithdrwalObj.DepositRowValues);//.Select(c => { c.Password = null; return c; }).ToList();
                    _depositAndWithdrwalObj.CheckedRows = _depositAndWithdrwalObj.CheckedRows == null ? null : _depositAndWithdrwalObj.CheckedRows.Select(x => { x.CommonObj = new CommonViewModel { CreatedBy = _appUA.UserName, CreatedDate = _appUA.DateTime };return x;}).ToList();
                  
                }
                result = _depositAndWithdrawalsBusiness.InsertUpdateDepositAndWithdrawals(Mapper.Map<DepositAndWithdrwalViewModel, DepositAndWithdrawals>(_depositAndWithdrwalObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateDepositAndWithdrawals

        #region ClearCheque
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "W")]
        public string ClearCheque(List<Guid> data)
        {
            try
            {

                object result = null;
             
                result = _depositAndWithdrawalsBusiness.ClearCheque(data);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion ClearCheque

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Deposit";
                    ToolboxViewModelObj.addbtn.Title = "Deposit";
                    ToolboxViewModelObj.addbtn.Event = "ShowDepositModal();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "WDL";
                    ToolboxViewModelObj.savebtn.Title = "WithDrawal";
                    ToolboxViewModelObj.savebtn.Event = "ShowWithDrawal();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "ChqClr";
                    ToolboxViewModelObj.resetbtn.Title = "ChqClr";
                    ToolboxViewModelObj.resetbtn.Event = "ShowChequeClear();";

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