using System;
using System.Collections.Generic;
using System.Web.Http;
using SPAccounts.BusinessService.Contracts;
using Newtonsoft.Json;
using AutoMapper;
using SAMTool.DataAccessObject.DTO;
using UserInterface.Models;
using SPAccounts.DataAccessObject.DTO;

namespace UserInterface.API
{
    public class InvoiceSummaryController : ApiController
    {
        #region Constructor_Injection

        ICustomerInvoicesBusiness _customerInvoicesBusiness;


        public InvoiceSummaryController(ICustomerInvoicesBusiness customerInvoicesBusiness)
        {
            _customerInvoicesBusiness = customerInvoicesBusiness;

        }
        #endregion Constructor_Injection

        Const messages = new Const();

        #region GetOutstandingInvoices
        [HttpPost]
        public string GetOutstandingInvoicesForMobile(CustomerInvoice CusObj)
        {
            try
            {
                CustomerInvoicesSummaryForMobileViewModel invoiceObj = Mapper.Map<CustomerInvoicesSummaryForMobile, CustomerInvoicesSummaryForMobileViewModel>(_customerInvoicesBusiness.GetOutstandingCustomerInvoices(CusObj));
                  return JsonConvert.SerializeObject(new { Result = true, Records = new {OutstandingList= invoiceObj.CustInv, Summary = invoiceObj.CustInvSumObj } });
                
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion GetOutstandingInvoices


        #region GetOpenInvoices
        [HttpPost]
        public string GetOpenInvoicesForMobile()
        {
            try
            {
                CustomerInvoicesSummaryForMobileViewModel invoiceObj = Mapper.Map<CustomerInvoicesSummaryForMobile, CustomerInvoicesSummaryForMobileViewModel>(_customerInvoicesBusiness.GetOpeningCustomerInvoices());
                return JsonConvert.SerializeObject(new { Result = true, Records = new { OpeningList = invoiceObj.CustInv, Summary = invoiceObj.CustInvSumObj } });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion GetOpenInvoices
    }
}

   
