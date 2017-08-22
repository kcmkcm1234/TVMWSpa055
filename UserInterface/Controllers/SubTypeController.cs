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

                evm.EmployeeTypeList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<EmployeeTypeViewModel> employeeTypeList = Mapper.Map<List<EmployeeType>, List<EmployeeTypeViewModel>>(_employeeBusiness.GetAllEmployeeTypes());
                foreach (EmployeeTypeViewModel etvm in employeeTypeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = etvm.Name,
                        Value = etvm.Code.ToString(),
                        Selected = false
                    });
                }
                evm.EmployeeTypeList = selectListItem;

                evm.DepartmentList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<EmployeeTypeViewModel> departmentList = Mapper.Map<List<EmployeeType>, List<EmployeeTypeViewModel>>(_employeeBusiness.GetAllDepartment());
                foreach (EmployeeTypeViewModel dpm in departmentList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = dpm.Name,
                        Value = dpm.Code.ToString(),
                        Selected = false
                    });
                }
                evm.DepartmentList = selectListItem;

                evm.CategoryList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<EmployeeTypeViewModel> categoryTypeList = Mapper.Map<List<EmployeeType>, List<EmployeeTypeViewModel>>(_employeeBusiness.GetAllCategory());
                foreach (EmployeeTypeViewModel ctvm in categoryTypeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = ctvm.Name,
                        Value = ctvm.Code.ToString(),
                        Selected = false
                    });
                }
                evm.CategoryList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(evm);
        }

        #region GetAllEmployees
        [HttpGet]
        // [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
        public string GetAllEmployees()
        {
            try
            {

                List<EmployeeViewModel> employeesList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetAllOtherEmployees());
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
        //[AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
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
        //[AuthSecurityFilter(ProjectObject = "Employee", Mode = "W")]
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
        // [AuthSecurityFilter(ProjectObject = "Employee", Mode = "D")]
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
        // [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
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
        #endregion ButtonStyling
    }
}