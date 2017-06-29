using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ICustomerInvoicesBusiness
    {
        List<CustomerInvoice> GetAllCustomerInvoices();
        CustomerInvoiceSummary GetCustomerInvoicesSummary();
    }
}
