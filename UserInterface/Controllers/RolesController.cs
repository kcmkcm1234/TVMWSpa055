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
    public class RolesController : Controller
    {
        private IRolesBusiness _rolesBusiness;
        private IApplicationBusiness _applicationBusiness;

        public RolesController(IRolesBusiness rolesBusiness, IApplicationBusiness applicationBusiness)
        {
            _rolesBusiness = rolesBusiness;
            _applicationBusiness = applicationBusiness;
        }


        // GET: Roles
        public ActionResult Index()
        {
            RolesViewModel _rolesObj = new RolesViewModel();
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
            _rolesObj.ApplicationList = selectListItem;
            return View(_rolesObj);
        }

        //#region GetAllRolesByApplication
        //[HttpGet] 
        //public string GetAllRolesByAppID(string AppId)
        //{
        //    try
        //    {
        //        List<RolesViewModel> rolesVMList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllRoles());
        //        return JsonConvert.SerializeObject(new { Result = "OK", Records = rolesVMList });
        //    }
        //    catch (Exception ex)
        //    {
        //        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        //#endregion GetAllRolesByApplication


        #region InsertUpdateRoles

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateRoles(RolesViewModel rolesObj)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                if (rolesObj.ID == Guid.Empty)
                {
                    try
                    {
                        //UserObj.commonObj = new LogDetailsViewModel();
                        //UserObj.commonObj.CreatedBy = _commonBusiness.GetUA().UserName;
                        //UserObj.commonObj.CreatedDate = _commonBusiness.GetCurrentDateTime();
                        result = _rolesBusiness.InsertRoles(Mapper.Map<RolesViewModel, Roles>(rolesObj));
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
                        result = _rolesBusiness.UpdateRoles(Mapper.Map<RolesViewModel, Roles>(rolesObj));
                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                    }
                }
            }
            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }

        #endregion InsertUpdateEvent

        #region GetAllRoles
        [HttpGet]
        public string GetAllRoles()
        {
            try
            {

                List<RolesViewModel> rolesList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(null));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = rolesList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllRoles

        #region GetRolesDetailsByID
        [HttpGet]
        public string GetRolesDetailsByID(string Id)
        {
            try
            {

                RolesViewModel roleList = Mapper.Map<Roles, RolesViewModel>(_rolesBusiness.GetRolesDetailsByID(Id));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = roleList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetRolesDetailsByID 

        #region DeleteRoles

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeleteRoles(RolesViewModel RolesObj)
        {
            object result = null;

            if (RolesObj.ID != Guid.Empty)
            {
                try
                {
                    result = _rolesBusiness.DeleteRoles(Mapper.Map<RolesViewModel, Roles>(RolesObj));
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

        #endregion DeleteRoles


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
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}