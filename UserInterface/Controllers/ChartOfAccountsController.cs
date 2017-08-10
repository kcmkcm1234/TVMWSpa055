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
    public class ChartOfAccountsController : Controller
    {
        #region Constructor_Injection 

        AppConst c = new AppConst();
        IChartOfAccountsBusiness _chartOfAccountsBusiness;
       
        public ChartOfAccountsController(IChartOfAccountsBusiness chartOfAccountsBusiness)
        {
            _chartOfAccountsBusiness = chartOfAccountsBusiness;           
        }
        #endregion Constructor_Injection 

        // GET: ChartOfAccounts
        public ActionResult Index()
        {
            return View();
        }


        #region GetAllChartOfAccounts
        [HttpGet]
        public string GetAllChartOfAccounts()
        {
            try
            {

                List<ChartOfAccountsViewModel> ChartOfAccountsList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_chartOfAccountsBusiness.GetAllChartOfAccounts());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ChartOfAccountsList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllChartOfAccounts

        #region GetChartOfAccountDetails
        [HttpGet]
        public string GetChartOfAccountDetails(string Code)
        {
            try
            {

                ChartOfAccountsViewModel ChartOfAccountsList = Mapper.Map<ChartOfAccounts, ChartOfAccountsViewModel>(_chartOfAccountsBusiness.GetChartOfAccountDetails(Code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ChartOfAccountsList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetChartOfAccountDetails


        #region InsertUpdateChartOfAccounts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateChartOfAccounts(ChartOfAccountsViewModel _chartOfAccountsObj)
        {
            try
            {

                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                _chartOfAccountsObj.commonObj = new CommonViewModel();
                _chartOfAccountsObj.commonObj.CreatedBy = _appUA.UserName;
                _chartOfAccountsObj.commonObj.CreatedDate = _appUA.DateTime;
                _chartOfAccountsObj.commonObj.UpdatedBy = _appUA.UserName;
                _chartOfAccountsObj.commonObj.UpdatedDate = _appUA.DateTime;
                if (!string.IsNullOrEmpty(_chartOfAccountsObj.hdnCode))
                {
                    _chartOfAccountsObj.Code = _chartOfAccountsObj.hdnCode;
                }

                result = _chartOfAccountsBusiness.InsertUpdateChartOfAccounts(Mapper.Map<ChartOfAccountsViewModel, ChartOfAccounts>(_chartOfAccountsObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateChartOfAccounts

        #region DeleteChartOfAccounts
        [HttpGet]
        public string DeleteChartOfAccounts(string code)
        {

            try
            {
                object result = null;

                result = _chartOfAccountsBusiness.DeleteChartOfAccounts(code);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteChartOfAccounts

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "ChartOfAccounts", Mode = "R")]
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


                    break;
                case "Edit":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bank";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bank";
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
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bank";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.addbtn.Visible = false;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

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