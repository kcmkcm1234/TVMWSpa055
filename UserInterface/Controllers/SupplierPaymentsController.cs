using AutoMapper;
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
    public class SupplierPaymentsController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        ISupplierPaymentsBusiness _supplierPaymentsBusiness;
        ISupplierBusiness _supplierBusiness;
        IBankBusiness _bankBusiness;
        ICompaniesBusiness _companiesBusiness;
        ISupplierCreditNotesBusiness _supplierCreditNotesBusiness;
        IPaymentModesBusiness _paymentmodesBusiness;
        ISupplierInvoicesBusiness _supplierInvoicesBusiness;

        public SupplierPaymentsController(ISupplierPaymentsBusiness supplierPaymentsBusiness,
            IPaymentModesBusiness paymentmodeBusiness,
            ISupplierBusiness supplierBusiness, IBankBusiness bankBusiness, ICompaniesBusiness companiesBusiness,
            ISupplierInvoicesBusiness supplierInvoicesBusiness, ISupplierCreditNotesBusiness supplierCreditNotesBusiness)
        {
            _supplierPaymentsBusiness = supplierPaymentsBusiness;
            _paymentmodesBusiness = paymentmodeBusiness;
            _supplierInvoicesBusiness = supplierInvoicesBusiness;
            _supplierBusiness = supplierBusiness;
            _bankBusiness = bankBusiness;
            _supplierCreditNotesBusiness = supplierCreditNotesBusiness;
            _companiesBusiness = companiesBusiness;
        }
        #endregion Constructor_Injection
         
        #region Index 
        // GET: SupplierPayments
        public ActionResult Index()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
           // ViewBag.Currentdate = _appUA.DateTime.ToString("dd-MMM-yyyy");

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            SupplierPaymentsViewModel SP = new SupplierPaymentsViewModel();
            //-------------1.Supplier List-------------------//
            SP.supplierObj = new SuppliersViewModel();
            SP.supplierObj.SupplierList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<SuppliersViewModel> CustList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_supplierBusiness.GetAllSuppliers());
            foreach (SuppliersViewModel Cust in CustList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Cust.CompanyName,
                    Value = Cust.ID.ToString(),
                    Selected = false
                });
            }
            SP.supplierObj.SupplierList = selectListItem;
            //-------------2.PaymentModes-------------------//
            SP.PaymentModesObj = new PaymentModesViewModel();
            SP.PaymentModesObj.PaymentModesList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<PaymentModesViewModel> PaymentModeList = Mapper.Map<List<PaymentModes>, List<PaymentModesViewModel>>(_paymentmodesBusiness.GetAllPaymentModes());
            foreach (PaymentModesViewModel PMVM in PaymentModeList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = PMVM.Description,
                    Value = PMVM.Code,
                    Selected = false
                });
            }
            SP.PaymentModesObj.PaymentModesList = selectListItem;
            //-------------3.BanksList-------------------//
            SP.bankObj = new BankViewModel();
            SP.bankObj.BanksList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<BankViewModel> BankList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
            foreach (BankViewModel BL in BankList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = BL.Name,
                    Value = BL.Code,
                    Selected = false
                });
            }
            SP.bankObj.BanksList = selectListItem;
            //-------------4.CompanyList-------------------//
            SP.CompanyObj = new CompaniesViewModel();
            SP.CompanyObj.CompanyList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<CompaniesViewModel> CompaniesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            foreach (CompaniesViewModel BL in CompaniesList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = BL.Name,
                    Value = BL.Code,
                    Selected = false
                });
            }
            SP.CompanyObj.CompanyList = selectListItem;
            return View(SP);
        }
        #endregion Index 


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
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    break;
                case "Edit":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeletePayments();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "savePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";


                    break;
                case "Add":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeletePayments();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "savePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}