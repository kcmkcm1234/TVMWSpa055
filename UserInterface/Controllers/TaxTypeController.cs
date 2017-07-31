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
    public class TaxTypeController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        ITaxTypesBusiness _taxTypeBusiness;

        public TaxTypeController(ITaxTypesBusiness taxTypesBusiness)
        {
            _taxTypeBusiness = taxTypesBusiness;
        }
        #endregion Constructor_Injection 
        // GET: TaxType
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        #region GetAllTaxTypes
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public string GetAllTaxTypes()
        {
            try
            {

                List<TaxTypesViewModel> TaxTypesList = Mapper.Map<List<TaxTypes>, List<TaxTypesViewModel>>(_taxTypeBusiness.GetAllTaxTypes());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = TaxTypesList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllTaxTypes

        #region GetTaxTypeDetailsByCode
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public string GetTaxTypeDetailsByCode(string Code)
        {
            try
            {

                TaxTypesViewModel taxTypesObj = Mapper.Map<TaxTypes, TaxTypesViewModel>(_taxTypeBusiness.GetTaxTypeDetailsByCode(Code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = taxTypesObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetTaxTypeDetailsByCode

        #region InsertUpdateTaxType
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "W")]
        public string InsertUpdateTaxType(TaxTypesViewModel _taxTypesObj)
        {
            try
            {

                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                _taxTypesObj.commonObj = new CommonViewModel();
                _taxTypesObj.commonObj.CreatedBy = _appUA.UserName;
                _taxTypesObj.commonObj.CreatedDate = _appUA.DateTime;
                _taxTypesObj.commonObj.UpdatedBy = _appUA.UserName;
                _taxTypesObj.commonObj.UpdatedDate = _appUA.DateTime;
                if (!string.IsNullOrEmpty(_taxTypesObj.hdnCode))
                {
                    _taxTypesObj.Code = _taxTypesObj.hdnCode;
                }

                result = _taxTypeBusiness.InsertUpdateTaxType(Mapper.Map<TaxTypesViewModel, TaxTypes>(_taxTypesObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateTaxType

        #region DeleteTaxType
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "D")]
        public string DeleteTaxType(string code)
        {

            try
            {
                object result = null;

                result = _taxTypeBusiness.DeleteTaxType(code);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteTaxType

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
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
                    ToolboxViewModelObj.savebtn.Title = "Save TaxType";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete TaxType";
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