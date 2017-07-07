using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ICustomerInvoicesBusiness
    {
        List<CustomerInvoice> GetAllCustomerInvoices();
        CustomerInvoice GetCustomerInvoiceDetails(Guid ID);
        CustomerInvoiceSummary GetCustomerInvoicesSummary();
        CustomerInvoice InsertUpdateInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua);
    }
}
