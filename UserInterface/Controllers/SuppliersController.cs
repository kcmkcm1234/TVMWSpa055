using AutoMapper;
using Newtonsoft.Json;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        SecurityFilter.ToolBarAccess _tool;
        SPAccounts.DataAccessObject.DTO.Common common = new SPAccounts.DataAccessObject.DTO.Common();
        //string Key = "X-Api-Key";
        //string Value = "JyFgHsICUOgloskIMuyM6PH4GYxyU30p";

        public SuppliersController(ISupplierBusiness supplierBusiness, ICustomerBusiness customerBusiness, IPaymentTermsBusiness paymentTermsBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _SupplierBusiness = supplierBusiness;
            _customerBusiness = customerBusiness;
            _paymentTermsBusiness = paymentTermsBusiness;
            _tool = tool;
        }
        #endregion Constructor_Injection 
        // GET: Suppliers
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Suppliers", Mode = "R")]
        public ActionResult Index(string id)
        {
            SuppliersViewModel supplierViewModel = null;
            ViewBag.value = id;
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
        [AuthSecurityFilter(ProjectObject = "Suppliers", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "Suppliers", Mode = "R")]
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
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Suppliers", Mode = "W")]
        public string InsertUpdateSupplier(SuppliersViewModel _supplierObj)
        {
            try
            {

                object result = null;
                AppUA appUA = Session["AppUA"] as AppUA;
                _supplierObj.commonObj = new CommonViewModel();
                _supplierObj.commonObj.CreatedBy = appUA.UserName;
                _supplierObj.commonObj.CreatedDate = common.GetCurrentDateTime();
                _supplierObj.commonObj.UpdatedBy = appUA.UserName;
                _supplierObj.commonObj.UpdatedDate = common.GetCurrentDateTime();

                result = _SupplierBusiness.InsertUpdateSupplier(Mapper.Map<SuppliersViewModel, Supplier>(_supplierObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
                //  string Status = result.GetType().GetProperty("Status").GetValue(result, null).ToString();

                //if (Status == "1")
                //    {

                //    if (_supplierObj.ID == Guid.Empty)
                //    {

                //        string ID = result.GetType().GetProperty("ID").GetValue(result, null).ToString();

                //        string json_data;
                //        string response;

                //        SuppliersAPI supplier = new SuppliersAPI();
                //        supplier.id = ID;
                //        supplier.name = _supplierObj.CompanyName;
                //        supplier.gst_number = _supplierObj.TaxRegNo;
                //        supplier.address = _supplierObj.BillingAddress;
                //        supplier.phone = _supplierObj.LandLine;
                //        supplier.email = _supplierObj.ContactEmail;
                //        supplier.contact_person_name = _supplierObj.ContactPerson;
                //        supplier.contact_person_email = _supplierObj.ContactEmail;
                //        supplier.contact_person_number = _supplierObj.Mobile;


                //        json_data = "{ \"supplier\" :" + JsonConvert.SerializeObject(supplier) + " } ";
                //        response = InvokePostRequest("http://secure.appdeal.in/sp2/rest/web/supplier/save-supplier#", json_data);
                //        ResponseAPI res = new ResponseAPI();
                //        res = JsonConvert.DeserializeObject<ResponseAPI>(response);
                //        if (res.status == "0")
                //        {
                //            result = _SupplierBusiness.DeleteSupplier(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty);
                //        }
                //        // dynamic stuff = JsonConvert.DeserializeObject(response);

                //    }
                //    else
                //    {
                //        string json_data;
                //        string response;
                //        string updateID = result.GetType().GetProperty("ID").GetValue(result, null).ToString();
                //        SuppliersAPI supplier = new SuppliersAPI();
                //        supplier.id = updateID;
                //        supplier.name = _supplierObj.CompanyName;
                //        supplier.gst_number = _supplierObj.TaxRegNo;
                //        supplier.address = _supplierObj.BillingAddress;
                //        supplier.phone = _supplierObj.LandLine;
                //        supplier.email = _supplierObj.ContactEmail;
                //        supplier.contact_person_name = _supplierObj.ContactPerson;
                //        supplier.contact_person_email = _supplierObj.ContactEmail;
                //        supplier.contact_person_number = _supplierObj.Mobile;


                //        json_data = "{ \"supplier\" :" + JsonConvert.SerializeObject(supplier) + " } ";
                //        response = InvokePostRequest("http://secure.appdeal.in/sp2/rest/web/supplier/save-supplier#", json_data);


                //    }
                //    }



            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateSupplier

        //public string InvokePostRequest(string requestUrl, string requestBody)
        //{


        //    try
        //    {
        //        var request = WebRequest.Create(requestUrl) as HttpWebRequest;
        //        //  request.Headers.Add("Key", Key);
        //        request.Headers.Add(Key, Value);
        //        request.ContentType = "application/json";
        //        request.Method = @"POST";

        //        var requestWriter = new StreamWriter(request.GetRequestStream());
        //        requestWriter.Write(requestBody);
        //        requestWriter.Close();
        //        var webResponse = (HttpWebResponse)request.GetResponse();

        //        var responseReader = new StreamReader(webResponse.GetResponseStream());
        //        return responseReader.ReadToEnd();
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Response != null)
        //        {
        //            //reading the custom messages sent by the server
        //            using (var reader = new StreamReader(ex.Response.GetResponseStream()))
        //            {
        //                return reader.ReadToEnd();
        //            }
        //        }
        //        return ex.Message + ex.InnerException + "--error. " + "RequestString:::::::::::: " + requestBody;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message + ex.InnerException + "--error. " + "RequestString:::::::::::: " + requestBody;
        //    }


        //}




        #region DeleteSupplier
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Suppliers", Mode = "D")]
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

        #region  UpdateMaxLimit
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Suppliers", Mode = "W")]
        public string UpdateMaxLimit(SuppliersViewModel _supplierObj)
        {
            try
            {

                object result = null;
                AppUA appUA = Session["AppUA"] as AppUA;
                _supplierObj.commonObj = new CommonViewModel();
                _supplierObj.commonObj.CreatedBy = appUA.UserName;
                _supplierObj.commonObj.CreatedDate = common.GetCurrentDateTime();
                _supplierObj.commonObj.UpdatedBy = appUA.UserName;
                _supplierObj.commonObj.UpdatedDate = common.GetCurrentDateTime();

                result = _SupplierBusiness.UpdateMaxLimit(Mapper.Map<SuppliersViewModel, Supplier>(_supplierObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result,Message="Updated Maximum Limit on Amount" });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateSupplier


        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Suppliers", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            Permission _permission = Session["UserRights"] as Permission;

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

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.LimitBtn.Visible = true;
                    ToolboxViewModelObj.LimitBtn.Text = "Limit";
                    ToolboxViewModelObj.LimitBtn.Title = "Max Limit On Amount";
                    ToolboxViewModelObj.LimitBtn.Event = "openLimitModal();";

                       ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);

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