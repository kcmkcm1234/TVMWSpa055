﻿using AutoMapper;
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
    public class SubTypeController : Controller
    {
        // GET: SubType
        #region Constructor_Injection 

        AppConst c = new AppConst();
        IEmployeeBusiness _employeeBusiness;
        ICompaniesBusiness _companiesBusiness;

        public SubTypeController(IEmployeeBusiness employeeBusiness, ICompaniesBusiness companiesBusiness)
        {
            _employeeBusiness = employeeBusiness;
            _companiesBusiness = companiesBusiness;
        }
        #endregion Constructor_Injection 


        [HttpGet]
        // GET: SubTypeNarration
        [AuthSecurityFilter(ProjectObject = "SubType", Mode = "R")]
        public ActionResult Index()
        {
            SubTypeNarrationViewModel evm = null;
            try
            {
                evm = new SubTypeNarrationViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
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
                evm.CompaniesList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(evm);
        }

        #region GetAllEmployees
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SubType", Mode = "R")]
        public string GetAllEmployees(string filter)
        {
            try
            {

                List<EmployeeViewModel> employeesList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetAllOtherEmployees(filter));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = employeesList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllEmployees

        #region GetEmployeeDetails
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SubType", Mode = "R")]
        public string GetEmployeeDetails(string ID)
        {
            try
            {

                EmployeeViewModel employeeObj = Mapper.Map<Employee, EmployeeViewModel>(_employeeBusiness.GetEmployeeDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = employeeObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetEmployeeDetails

        #region InsertUpdateEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "SubType", Mode = "W")]
        public string InsertUpdateEmployee(SubTypeNarrationViewModel _employeeObj)
        {
            object result = null;
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                _employeeObj.commonObj = new CommonViewModel();
                _employeeObj.commonObj.CreatedBy = _appUA.UserName;
                _employeeObj.commonObj.CreatedDate = _appUA.DateTime;
                _employeeObj.commonObj.UpdatedBy = _appUA.UserName;
                _employeeObj.commonObj.UpdatedDate = _appUA.DateTime;
                result = _employeeBusiness.InsertUpdateEmployee(Mapper.Map<SubTypeNarrationViewModel, Employee>(_employeeObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateEmployee

        #region DeleteEmployee
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SubType", Mode = "D")]
        public string DeleteEmployee(string ID)
        {

            try
            {
                object result = null;

                result = _employeeBusiness.DeleteEmployee(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteEmployee

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SubType", Mode = "R")]
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
        #endregion ButtonStyling
    }
}