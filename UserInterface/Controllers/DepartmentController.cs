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
    public class DepartmentController : Controller
    {
        AppConst c = new AppConst();
        IDepartmentBusiness _departmentBusiness;
        public DepartmentController(IDepartmentBusiness departmentBusiness)
        {
            _departmentBusiness = departmentBusiness;
        }

        //GET: Department
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Department", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region GetAllDepartments
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Department", Mode = "R")]
        public string GetAllDepartments()
        {
            try
            {
                List<DepartmentViewModel> departmentList = Mapper.Map<List<Department>, List<DepartmentViewModel>>(_departmentBusiness.GetAllDetpartments());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = departmentList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllDepartments

        #region GetDepartmentDetails
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Department", Mode = "R")]
        public string GetDepartmentDetails(string Code)
        {
            try
            {

                DepartmentViewModel departmentObj = Mapper.Map<Department, DepartmentViewModel>(_departmentBusiness.GetDepartmentDetails(Code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = departmentObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetDepartmentDetails

        #region InsertUpdateDepartment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Department", Mode = "W")]
        public string InsertUpdateDepartment(DepartmentViewModel departmentViewModel)
        {
            object result = null;
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                departmentViewModel.commonObj = new CommonViewModel();
                departmentViewModel.commonObj.CreatedBy = _appUA.UserName;
                departmentViewModel.commonObj.CreatedDate = _appUA.DateTime;
                departmentViewModel.commonObj.UpdatedBy = departmentViewModel.commonObj.CreatedBy;
                departmentViewModel.commonObj.UpdatedDate = departmentViewModel.commonObj.CreatedDate;
                switch(departmentViewModel.Operation)
                {
                    case "Insert":
                        result = _departmentBusiness.InsertDepartment(Mapper.Map<DepartmentViewModel, Department>(departmentViewModel));
                        break;
                    case "Update":
                        result = _departmentBusiness.UpdateDepartment(Mapper.Map<DepartmentViewModel, Department>(departmentViewModel));
                        break;
                }
              
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateDepartment

        #region DeleteEmployee
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Department", Mode = "D")]
        public string DeleteDepartment(string Code)
        {

            try
            {
                object result = null;

                result = _departmentBusiness.DeleteDepartment(Code);
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
        [AuthSecurityFilter(ProjectObject = "Department", Mode = "R")]
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
                    ToolboxViewModelObj.savebtn.Title = "Save Department";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Department";
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
                    ToolboxViewModelObj.deletebtn.Title = "Delete Department";
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