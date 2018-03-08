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
    public class AccountHeadGroupController : Controller
    {
        #region Constructor Injection
        // GET: OtherExpenses
        AppConst c = new AppConst();
        IOtherExpenseBusiness _otherExpenseBusiness;
        IAccountHeadGroupBusiness _accountHeadGroupBusiness;
        ICommonBusiness _commonBusiness;
        SPAccounts.DataAccessObject.DTO.Common common = new SPAccounts.DataAccessObject.DTO.Common();
        SecurityFilter.ToolBarAccess _tool;
        public AccountHeadGroupController(IOtherExpenseBusiness otherExpenseBusiness, ICommonBusiness commonBusiness, IAccountHeadGroupBusiness accountHeadGroupBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _otherExpenseBusiness = otherExpenseBusiness;
            _accountHeadGroupBusiness = accountHeadGroupBusiness;
            _commonBusiness = commonBusiness;
            _tool = tool;
        }
        #endregion Constructor Injection


        public ActionResult Index()
        {
            AccountHeadGroupViewModel accountHeadGroupVM = new AccountHeadGroupViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<ChartOfAccountsViewModel> chartOfAccountList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_otherExpenseBusiness.GetAllAccountTypesForAccountHeadGroup());
            foreach (ChartOfAccountsViewModel cav in chartOfAccountList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = cav.TypeDesc,
                    Value = cav.Code,

                    Selected = false,


                });
            }
            accountHeadGroupVM.AccountTypes = selectListItem;
            selectListItem = null;
            return View(accountHeadGroupVM);
        }

        #region Get All Account Head Group
        [AuthSecurityFilter(ProjectObject = "AccountHeadGroup", Mode = "R")]
        public string GetAllAccountHeadGroup()
        {
            try
            {
                List<AccountHeadGroupViewModel> accountHeadGroupList = Mapper.Map<List<AccountHeadGroup>, List<AccountHeadGroupViewModel>>(_accountHeadGroupBusiness.GetAllAccountHeadGroup());
                     return JsonConvert.SerializeObject(new { Result = "OK", Records = accountHeadGroupList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion Get All Account Head Group


        #region Get Disabled Account Head Code
        [AuthSecurityFilter(ProjectObject = "AccountHeadGroup", Mode = "R")]
        public string GetDisabledCodeForAccountHeadGroup(string ID)
        {
            try
            {
                List<AccountHeadGroupViewModel> disabledAccountHeadGroupList = Mapper.Map<List<AccountHeadGroup>, List<AccountHeadGroupViewModel>>(_accountHeadGroupBusiness.GetDisabledCodeForAccountHeadGroup(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = disabledAccountHeadGroupList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion Get Disabled Account Head Code

        #region InsertUpdateAccountHeadGroup

        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "AccountHeadGroup", Mode = "R")]
        public string InsertUpdateAccountHeadGroup(AccountHeadGroupViewModel accountHeadGroup)
         {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                accountHeadGroup.commonObj = new CommonViewModel();
                accountHeadGroup.commonObj.CreatedBy = appUA.UserName;
                accountHeadGroup.commonObj.CreatedDate = common.GetCurrentDateTime();
                accountHeadGroup.commonObj.UpdatedBy = appUA.UserName;
                accountHeadGroup.commonObj.UpdatedDate = common.GetCurrentDateTime();
                AccountHeadGroupViewModel CIVM = Mapper.Map<AccountHeadGroup, AccountHeadGroupViewModel>(_accountHeadGroupBusiness.InsertUpdateAccountHeadGroup(Mapper.Map<AccountHeadGroupViewModel, AccountHeadGroup>(accountHeadGroup), appUA));
                if (accountHeadGroup.ID != null && accountHeadGroup.ID != Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = CIVM });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CIVM });
                }
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateAccountHeadGroup


        #region GetAccountHeadGroupDetailsByID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AccountHeadGroup", Mode = "R")]
        public string GetAccountHeadGroupDetailsByID(string ID)
        {
            try
            {
                AccountHeadGroupViewModel accountHeadGroup = Mapper.Map<AccountHeadGroup, AccountHeadGroupViewModel>(_accountHeadGroupBusiness.GetAccountHeadGroupDetailsByID(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = accountHeadGroup });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAccountHeadGroupDetailsByID


        #region DeleteAccountHeadGroup
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AccountHeadGroup", Mode = "R")]
        public string DeleteAccountHeadGroup(AccountHeadGroupViewModel accountHeadGroupVM)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            object result = null;
            try
            {
                result = _accountHeadGroupBusiness.DeleteAccountHeadGroup(accountHeadGroupVM.ID);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess, Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion DeleteAccountHeadGroup

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AccountHeadGroup", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew();";

                    //----added for export button--------------

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    //---------------------------------------

                    break;
                case "Edit":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Account Head Group";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Account Head Group";
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

                    ToolboxViewModelObj.resetbtn.Visible = false;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    ToolboxViewModelObj.deletebtn.Visible = false;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Account Head Group";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.addbtn.Visible = false;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew();";

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