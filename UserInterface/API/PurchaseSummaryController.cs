using AutoMapper;
using Newtonsoft.Json;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserInterface.Models;

namespace UserInterface.API
{
    public class PurchaseSummaryController : ApiController
    {
        #region Constructor_Injection

        ISupplierInvoicesBusiness _supplierInvoicesBusiness;


        public PurchaseSummaryController(ISupplierInvoicesBusiness supplierInvoicesBusiness)
        {
            _supplierInvoicesBusiness = supplierInvoicesBusiness;

        }
        #endregion Constructor_Injection

        Const messages = new Const();

        #region GetSupplierOutstandingInvoices
        [HttpPost]
        public string GetSupplierOutstandingInvoicesForMobile()
        {
            try
            {
                SupplierSummaryforMobileViewModel invoiceObj = Mapper.Map<SupplierSummaryforMobile, SupplierSummaryforMobileViewModel>(_supplierInvoicesBusiness.GetOutstandingSupplierInvoices());
                return JsonConvert.SerializeObject(new { Result = true, Records = new { OutStandingList = invoiceObj.SupInv, Summary = invoiceObj.supInvSumObj } });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion GetSupplierOutstandingInvoices


        #region GetSupplierOpeningInvoices
        [HttpPost]
        public string GetSupplierOpeningInvoicesForMobile()
        {
            try
            {
                SupplierSummaryforMobileViewModel invoiceObj = Mapper.Map<SupplierSummaryforMobile, SupplierSummaryforMobileViewModel>(_supplierInvoicesBusiness.GetOpeningSupplierInvoices());
                return JsonConvert.SerializeObject(new { Result = true, Records = new { OpeningList = invoiceObj.SupInv, Summary = invoiceObj.supInvSumObj } });
           
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion GetSupplierOpeningInvoices
    }
}
