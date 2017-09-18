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
        public string GetSupplierOutstandingInvoicesForMobile(SupplierInvoices SupObj)
        {
            try
            {
                SupplierSummaryforMobileViewModel invoiceObj = Mapper.Map<SupplierSummaryforMobile, SupplierSummaryforMobileViewModel>(_supplierInvoicesBusiness.GetOutstandingSupplierInvoices(SupObj));
                return JsonConvert.SerializeObject(new { Result = true, Records = new { OutstandingList = invoiceObj.SupInv, Summary = invoiceObj.supInvSumObj } });
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
    

    #region GetPurchaseBydate
    [HttpPost]
    public string GetSupplierPurchaseByDateWiseForMobile(SupplierInvoices SupObj)
    {
        try
        {
                if (SupObj.FromDate == null && SupObj.ToDate == null)
                {
                    SupObj.commonObj = new SPAccounts.DataAccessObject.DTO.Common();
                    SupObj.FromDate = SupObj.commonObj.GetCurrentDateTime().ToString();
                    SupObj.ToDate = SupObj.commonObj.GetCurrentDateTime().ToString();
                }
                SupplierSummaryforMobileViewModel invoiceObj = Mapper.Map<SupplierSummaryforMobile, SupplierSummaryforMobileViewModel>(_supplierInvoicesBusiness.GetSupplierPurchasesByDateWise(SupObj));
            return JsonConvert.SerializeObject(new { Result = true, Records = new { List = invoiceObj.SupInv, Summary = invoiceObj.supInvSumObj } });

        }
        catch (Exception ex)
        {

            return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
        }
    }
    #endregion GetPurchaseBydate

}
}
