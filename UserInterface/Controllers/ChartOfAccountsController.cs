﻿using AutoMapper;
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
    public class ChartOfAccountsController : Controller
    {
        #region Constructor_Injection 

        AppConst c = new AppConst();
        IChartOfAccountsBusiness _chartOfAccountsBusiness;
        ICommonBusiness _commonBusiness;
        SecurityFilter.ToolBarAccess _tool;

        public ChartOfAccountsController(IChartOfAccountsBusiness chartOfAccountsBusiness, ICommonBusiness commonBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _chartOfAccountsBusiness = chartOfAccountsBusiness;
            _commonBusiness = commonBusiness;
            _tool = tool;
        }
        #endregion Constructor_Injection 

        // GET: ChartOfAccounts
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ChartOfAccounts", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }


        #region GetAllChartOfAccounts
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ChartOfAccounts", Mode = "R")]
        public string GetAllChartOfAccounts(string type)
        {
            try
            {

                List<ChartOfAccountsViewModel> ChartOfAccountsList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_chartOfAccountsBusiness.GetAllChartOfAccounts(type));
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
        [AuthSecurityFilter(ProjectObject = "ChartOfAccounts", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "ChartOfAccounts", Mode = "W")]
        public string InsertUpdateChartOfAccounts(ChartOfAccountsViewModel _chartOfAccountsObj)
        {
            try
            {

                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                _chartOfAccountsObj.CommonObj = new CommonViewModel();
                _chartOfAccountsObj.CommonObj.CreatedBy = _appUA.UserName;
                _chartOfAccountsObj.CommonObj.CreatedDate = _appUA.DateTime;
                _chartOfAccountsObj.CommonObj.UpdatedBy = _appUA.UserName;
                _chartOfAccountsObj.CommonObj.UpdatedDate = _appUA.DateTime;
                if (!string.IsNullOrEmpty(_chartOfAccountsObj.hdnCode))
                {
                    _chartOfAccountsObj.Code = _chartOfAccountsObj.hdnCode;
                    _chartOfAccountsObj.Type = _chartOfAccountsObj.hdnType;
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
        [AuthSecurityFilter(ProjectObject = "ChartOfAccounts", Mode = "D")]
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


        public string UpdateAssignments(string code)
        {
            try
            {
                object result = null;
                result = _chartOfAccountsBusiness.UpdateAssignments(code);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }


        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ChartOfAccounts", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            Permission _permission = Session["UserRights"] as Permission;
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    //----added for export button--------------

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    //---------------------------------------
                    ToolboxViewModelObj.AssignBtn.Visible = true;
                    ToolboxViewModelObj.AssignBtn.Text = "Assign";
                    ToolboxViewModelObj.AssignBtn.Title = "Assign";
                    ToolboxViewModelObj.AssignBtn.Event = "ShowAssignModal();";

                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);

                    break;
                case "Edit":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Chart Of Accounts";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Chart Of Accounts";
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