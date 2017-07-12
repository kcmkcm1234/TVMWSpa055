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
    public class SuppliersController : Controller
    {
        #region Constructor_Injection 

        AppConst c = new AppConst();
        ICustomerBusiness _customerBusiness;
        ISupplierBusiness _SupplierBusiness;
        IPaymentTermsBusiness _paymentTermsBusiness;

        public SuppliersController(ISupplierBusiness supplierBusiness, ICustomerBusiness customerBusiness, IPaymentTermsBusiness paymentTermsBusiness)
        {
            _SupplierBusiness = supplierBusiness;
            _customerBusiness = customerBusiness;
            _paymentTermsBusiness = paymentTermsBusiness;
        }
        #endregion Constructor_Injection 
        // GET: Suppliers
        public ActionResult Index()
        {
            SuppliersViewModel supplierViewModel = null;
            try
            {
                supplierViewModel = new SuppliersViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                //Technician Drop down bind
                List<TitlesViewModel> titlesList = Mapper.Map<List<Titles>, List<TitlesViewModel>>(_customerBusiness.GetAllTitles());
                titlesList = titlesList == null ? null : titlesList.OrderBy(attset => attset.Title).ToList();
                foreach (TitlesViewModel tvm in titlesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = tvm.Title,
                        Value = tvm.Title,
                        Selected = false
                    });
                }
                supplierViewModel.TitlesList = selectListItem;

                supplierViewModel.DefaultPaymentTermList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<PaymentTermsViewModel> PayTermList = Mapper.Map<List<PaymentTerms>, List<PaymentTermsViewModel>>(_paymentTermsBusiness.GetAllPayTerms());
                foreach (PaymentTermsViewModel PayT in PayTermList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = PayT.Description,
                        Value = PayT.Code,
                        Selected = false
                    });
                }
                supplierViewModel.DefaultPaymentTermList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(supplierViewModel);
        }

        #region GetAllSuppliers
        [HttpGet]
        public string GetAllSuppliers()
        {
            try
            {

                List<SuppliersViewModel> suppliersList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_SupplierBusiness.GetAllSuppliers());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = suppliersList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllSuppliers

        #region GetSupplierDetails
        [HttpGet]
        public string GetSupplierDetails(string ID)
        {
            try
            {

                SuppliersViewModel supplierObj = Mapper.Map<Supplier, SuppliersViewModel>(_SupplierBusiness.GetSupplierDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetSupplierDetails

        #region InsertUpdateSupplier
        [HttpPost]
        public string InsertUpdateSupplier(SuppliersViewModel _supplierObj)
        {
            try
            {

                object result = null;
                AppUA ua = new AppUA();

                result = _SupplierBusiness.InsertUpdateSupplier(Mapper.Map<SuppliersViewModel, Supplier>(_supplierObj), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateSupplier

        #region DeleteSupplier
        public string DeleteSupplier(string ID)
        {

            try
            {
                object result = null;

                result = _SupplierBusiness.DeleteSupplier(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteSupplier

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
                    ToolboxViewModelObj.addbtn.Event = "openNav();";



                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    break;
                case "Edit":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Supplier";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Supplier";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

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