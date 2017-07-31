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
        [HttpGet]
        public string GetOutstandingInvoicesForMobile()
        {
            try
            {
                List<CustomerInvoicesViewModel> invoiceObj = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetOutstandingCustomerInvoices());
                  return JsonConvert.SerializeObject(new { Result = "OK", Records = invoiceObj });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetOutstandingInvoices


        #region GetOpeningInvoices
        [HttpGet]
        public string GetOpeningInvoicesForMobile()
        {
            try
            {
                List<CustomerInvoicesViewModel> invoiceObj = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetOpeningCustomerInvoices());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = invoiceObj });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetOpeningInvoices
    }
}

   
