using SPAccounts.DataAccessObject.DTO;
using System.Collections.Generic;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ICustomerInvoicesBusiness
    {
        List<CustomerInvoice> GetAllCustomerInvoices();
        CustomerInvoiceSummary GetCustomerInvoicesSummary();
        CustomerInvoice InsertUpdateInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua);
    }
}
