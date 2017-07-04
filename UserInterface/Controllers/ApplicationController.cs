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
    public class ApplicationController : Controller
    {
        private IApplicationBusiness _applicationBusiness;


        public ApplicationController(IApplicationBusiness applicationBusiness)
        {
            _applicationBusiness = applicationBusiness;
        }


        // GET: Application
        public ActionResult Index()
        {
            return View();
        }


        #region InsertUpdateApplication

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateApplication(ApplicationViewModel appObj)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                if (appObj.ID == Guid.Empty)
                {
                    try
                    {
                        //UserObj.commonObj = new LogDetailsViewModel();
                        //UserObj.commonObj.CreatedBy = _commonBusiness.GetUA().UserName;
                        //UserObj.commonObj.CreatedDate = _commonBusiness.GetCurrentDateTime();
                        result = _applicationBusiness.InsertApplication(Mapper.Map<ApplicationViewModel, Application>(appObj));
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
                        result = _applicationBusiness.UpdateApplication(Mapper.Map<ApplicationViewModel, Application>(appObj));
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

        #region GetAllApplication
        [HttpGet]
        public string GetAllApplication()
        {
            try
            {

                List<ApplicationViewModel> appList = Mapper.Map<List<Application>, List<ApplicationViewModel>>(_applicationBusiness.GetAllApplication());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = appList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllApplication

        #region DeleteApplication

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeleteApplication(ApplicationViewModel appObj)
        {
            object result = null;

            if (appObj.ID != Guid.Empty)
            {
                try
                {
                    result = _applicationBusiness.DeleteApplication(Mapper.Map<ApplicationViewModel, Application>(appObj));
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

        #endregion DeleteApplication

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