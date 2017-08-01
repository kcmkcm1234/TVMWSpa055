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
        public string GetOutstandingInvoicesForMobile()
        {
            try
            {
                List<CustomerInvoicesViewModel> invoiceObj = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetOutstandingCustomerInvoices());
                  return JsonConvert.SerializeObject(new { Result = true, Records = invoiceObj });
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
                List<CustomerInvoicesViewModel> invoiceObj = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetOpeningCustomerInvoices());
                return JsonConvert.SerializeObject(new { Result = true, Records = invoiceObj });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion GetOpenInvoices
    }
}

   
