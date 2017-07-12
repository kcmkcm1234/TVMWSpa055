using AutoMapper;
using Newtonsoft.Json;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class PrivilegesController : Controller
    {
        private IPrivilegesBusiness _privillegesBusiness;
        private IApplicationBusiness _applicationBusiness;
        private IRolesBusiness _rolesBusiness;
        public PrivilegesController(IPrivilegesBusiness privillegesBusiness, IApplicationBusiness applicationBusiness, IRolesBusiness rolesBusiness)
        {
            _privillegesBusiness = privillegesBusiness;
            _applicationBusiness = applicationBusiness;
            _rolesBusiness = rolesBusiness;
        }

        // GET: Privileges
        public ActionResult Index()
        {
            PrivilegesViewModel _privillegesObj = new PrivilegesViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();

            selectListItem = new List<SelectListItem>();
            List<ApplicationViewModel> ApplicationList = Mapper.Map<List<Application>, List<ApplicationViewModel>>(_applicationBusiness.GetAllApplication());
            foreach (ApplicationViewModel Appl in ApplicationList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Appl.Name,
                    Value = Appl.ID.ToString(),
                    Selected = false
                });
            }
            _privillegesObj.ApplicationList = selectListItem;

            selectListItem = new List<SelectListItem>();
            List<RolesViewModel> RoleList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(null));
            foreach (RolesViewModel Appl in RoleList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Appl.RoleName,
                    Value = Appl.ID.ToString(),
                    Selected = false
                });
            }
            _privillegesObj.RoleList = selectListItem;
            return View(_privillegesObj);
        }

        public ActionResult PrivilegesView()
        {
            return View();
        }


        #region InsertUpdatePrivileges

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdatePrivileges(PrivilegesViewModel PrivilegesObj)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                if (PrivilegesObj.ID == Guid.Empty)
                {
                    try
                    {
                        //UserObj.commonObj = new LogDetailsViewModel();
                        //UserObj.commonObj.CreatedBy = _commonBusiness.GetUA().UserName;
                        //UserObj.commonObj.CreatedDate = _commonBusiness.GetCurrentDateTime();
                        result = _privillegesBusiness.InsertPrivileges(Mapper.Map<PrivilegesViewModel, Privileges>(PrivilegesObj));

                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                    }
                }
                else
                {
                    try
                    {
                        //UserObj.commonObj = new LogDetailsViewModel();
                        //UserObj.commonObj.UpdatedBy = _commonBusiness.GetUA().UserName;
                        //UserObj.commonObj.UpdatedDate = _commonBusiness.GetCurrentDateTime();
                        result = _privillegesBusiness.UpdatePrivileges(Mapper.Map<PrivilegesViewModel, Privileges>(PrivilegesObj));
                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                    }
                }
            }
            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }

        #endregion InsertUpdatePrivileges

        #region GetAllPrivileges

        [HttpGet]
        public string GetAllPrivileges()
        {
            try
            {

                List<PrivilegesViewModel> List = Mapper.Map<List<Privileges>, List<PrivilegesViewModel>>(_privillegesBusiness.GetAllPrivileges());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = List });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllPrivileges

        #region GetPrivilegesDetailsByID
        [HttpGet]
        public string GetPrivilegesDetailsByID(string Id)
        {
            try
            {

                PrivilegesViewModel List = Mapper.Map<Privileges, PrivilegesViewModel>(_privillegesBusiness.GetPrivilegesDetailsByID(Id));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = List });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetPrivilegesDetailsByID


        #region DeletePrivileges

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeletePrivileges(PrivilegesViewModel privilegesObj)
        {
            object result = null;

            if (privilegesObj.ID != Guid.Empty)
            {
                try
                {
                    result = _privillegesBusiness.DeletePrivileges(Mapper.Map<PrivilegesViewModel, Privileges>(privilegesObj));
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {

            }

            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }

        #endregion DeletePrivileges




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
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goHome()";

                    break;
                case "Edit":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "Back()";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
                case "Add":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "Back()";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick()";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
                case "GoBack":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goHome()";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}