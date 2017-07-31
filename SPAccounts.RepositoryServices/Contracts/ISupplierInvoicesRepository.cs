using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ISupplierInvoicesRepository
    {
        List<SupplierInvoices> GetAllSupplierInvoices();
        SupplierInvoices GetSupplierInvoiceDetails(Guid ID);
        SupplierInvoiceSummary GetSupplierInvoicesSummary();
        SupplierInvoices InsertInvoice(SupplierInvoices _supplierInvoicesObj);
        SupplierInvoices UpdateInvoice(SupplierInvoices _supplierInvoicesObj);
        List<SupplierInvoices> GetOutstandingSupplierInvoices();
        List<SupplierInvoices> GetOpeningSupplierInvoices();
    }
}