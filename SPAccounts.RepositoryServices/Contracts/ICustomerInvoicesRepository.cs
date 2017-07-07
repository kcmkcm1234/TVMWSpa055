using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ICustomerInvoicesRepository
    {
         List<CustomerInvoice> GetAllCustomerInvoices();
        CustomerInvoiceSummary GetCustomerInvoicesSummary();
        CustomerInvoice GetCustomerInvoiceDetails(Guid ID);
        CustomerInvoice InsertInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua);
        CustomerInvoice UpdateInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua);
    }
}
