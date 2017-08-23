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
    public class EmployeeCategoryController : Controller
    {

        
        // GET: EmployeeCategory
         AppConst c = new AppConst();
        IEmployeeBusiness _employeeBusiness;
        public EmployeeCategoryController(IEmployeeBusiness employeeBusiness)
        {
            _employeeBusiness = employeeBusiness;
        }

     
        public ActionResult Index()
        {
            return View();
        }
        #region GetAllEmployeeCategories
        [HttpGet]

         [AuthSecurityFilter(ProjectObject = "EmployeeCategory", Mode = "R")]
        public string GetAllEmployeeCategories()
        {
            try
            {
                List<EmployeeCategoryViewModel> empCategoryList = Mapper.Map<List<EmployeeCategory>, List<EmployeeCategoryViewModel>>(_employeeBusiness.GetAllEmployeeCategories());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = empCategoryList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllEmployeeCategories

        #region GetEmployeeCategories
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "EmployeeCategory", Mode = "R")]
        public string GetEmployeeCategories(string Code)
        {
            try
            {

                EmployeeCategoryViewModel EmployeeCategoryObj = Mapper.Map<EmployeeCategory, EmployeeCategoryViewModel>(_employeeBusiness.GetEmployeeCategoryDetails(Code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = EmployeeCategoryObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetEmployeeCategories

        #region InsertUpdateEmployeeCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "EmployeeCategory", Mode = "W")]
        public string InsertUpdateEmployeeCategory(EmployeeCategoryViewModel employeeCategoryViewModel)
        {
            object result = null;
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                employeeCategoryViewModel.commonObj = new CommonViewModel();
                employeeCategoryViewModel.commonObj.CreatedBy = _appUA.UserName;
                employeeCategoryViewModel.commonObj.CreatedDate = _appUA.DateTime;
                employeeCategoryViewModel.commonObj.UpdatedBy = employeeCategoryViewModel.commonObj.CreatedBy;
                employeeCategoryViewModel.commonObj.UpdatedDate = employeeCategoryViewModel.commonObj.CreatedDate;
                switch (employeeCategoryViewModel.Operation)
                {
                    case "Insert":
                        result = _employeeBusiness.InsertEmployeeCategory(Mapper.Map<EmployeeCategoryViewModel, EmployeeCategory>(employeeCategoryViewModel));
                        break;
                    case "Update":
                        result = _employeeBusiness.UpdateEmployeeCategory(Mapper.Map<EmployeeCategoryViewModel, EmployeeCategory>(employeeCategoryViewModel));
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
        #endregion InsertUpdateEmployeeCategory

        #region DeleteEmployeeCategory
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "EmployeeCategory", Mode = "D")]
        public string DeleteEmployeeCategory(string Code)
        {

            try
            {
                object result = null;

                result = _employeeBusiness.DeleteEmployeeCategory(Code);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteEmployeeCategory
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "EmployeeCategory", Mode = "R")]
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
                    ToolboxViewModelObj.savebtn.Title = "Save Category";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Category";
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
                    ToolboxViewModelObj.deletebtn.Title = "Delete Category";
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