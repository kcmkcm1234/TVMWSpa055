using SPAccounts.DataAccessObject.DTO;
using System.Collections.Generic;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ICustomerInvoicesRepository
    {
         List<CustomerInvoice> GetAllCustomerInvoices();
        CustomerInvoiceSummary GetCustomerInvoicesSummary();
    }
}
