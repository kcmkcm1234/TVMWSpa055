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
    public class SupplierCreditNoteController : Controller
    {
        ISupplierCreditNotesBusiness _supplierCreditBusines;
        ISupplierBusiness _supplierBusiness;
        ICompaniesBusiness _companiesBusiness;
        AppConst c = new AppConst();
        public SupplierCreditNoteController(ISupplierCreditNotesBusiness supplierCreditBusines,ISupplierBusiness supplierBusiness,ICompaniesBusiness companiesBusiness)
        {
            _supplierCreditBusines = supplierCreditBusines;
            _supplierBusiness = supplierBusiness;
            _companiesBusiness = companiesBusiness;
        }

        // GET: SupplierCredit
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNotes", Mode = "R")]
        public ActionResult Index(string id)
        {
            SupplierCreditNoteViewModel scn = null;
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                ViewBag.Currentdate = _appUA.DateTime.ToString("dd-MMM-yyyy");
                ViewBag.value = id;

                scn = new SupplierCreditNoteViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<SuppliersViewModel> suppList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_supplierBusiness.GetAllSuppliers());
                foreach (SuppliersViewModel supp in suppList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = supp.CompanyName,
                        Value = supp.ID.ToString(),
                        Selected = false
                    });
                }
                scn.SupplierList = selectListItem;

                scn.CompaniesList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
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
                scn.CompaniesList = selectListItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(scn);
        }


        #region GetAllSupplierCreditNotes
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNotes", Mode = "R")]
        public string GetAllSupplierCreditNotes()
        {
            try
            {

                List<SupplierCreditNoteViewModel> SupplierCreditNoteList = Mapper.Map<List<SupplierCreditNote>, List<SupplierCreditNoteViewModel>>(_supplierCreditBusines.GetAllSupplierCreditNotes());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = SupplierCreditNoteList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllSupplierCreditNotes

        #region GetSupplierCreditNoteDetails
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNotes", Mode = "R")]
        public string GetSupplierCreditNoteDetails(string ID)
        {
            try
            {

                SupplierCreditNoteViewModel supplierCreditNoteObj = Mapper.Map<SupplierCreditNote, SupplierCreditNoteViewModel>(_supplierCreditBusines.GetSupplierCreditNoteDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierCreditNoteObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetSupplierCreditNoteDetails

        #region InsertUpdateSupplierCreditNote
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNotes", Mode = "W")]
        public string InsertUpdateSupplierCreditNote(SupplierCreditNoteViewModel _supplierCreditNoteObj)
        {
            try
            {

                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                _supplierCreditNoteObj.commonObj = new CommonViewModel();
                _supplierCreditNoteObj.commonObj.CreatedBy = _appUA.UserName;
                _supplierCreditNoteObj.commonObj.CreatedDate = _appUA.DateTime;
                _supplierCreditNoteObj.commonObj.UpdatedBy = _appUA.UserName;
                _supplierCreditNoteObj.commonObj.UpdatedDate = _appUA.DateTime;

                result = _supplierCreditBusines.InsertUpdateSupplierCreditNote(Mapper.Map<SupplierCreditNoteViewModel, SupplierCreditNote>(_supplierCreditNoteObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateSupplierCreditNote

        #region DeleteSupplierCreditNote
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNotes", Mode = "D")]
        public string DeleteSupplierCreditNote(string ID)
        {

            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                result = _supplierCreditBusines.DeleteSupplierCreditNote(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty,_appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteSupplierCreditNote

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNotes", Mode = "R")]
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
                    ToolboxViewModelObj.savebtn.Title = "Save Credit Note";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Credit Note";
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