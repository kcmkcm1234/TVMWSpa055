using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }
        #endregion Index 
    }
}