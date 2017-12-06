using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ISupplierInvoicesRepository
    {
        List<SupplierInvoices> GetAllSupplierInvoices(DateTime? FromDate, DateTime? ToDate, string Supplier, string InvoiceType, string Company, string Status, string Search, string AccountCode, string EmpID);
        SupplierInvoices GetSupplierInvoiceDetails(Guid ID);
        SupplierInvoiceSummary GetSupplierInvoicesSummary(bool IsInternal);
        SupplierInvoices InsertInvoice(SupplierInvoices _supplierInvoicesObj);
        SupplierInvoices UpdateInvoice(SupplierInvoices _supplierInvoicesObj);
        List<SupplierInvoices> GetOutstandingSupplierInvoices(SupplierInvoices SupObj);
        List<SupplierInvoices> GetSupplierPurchasesByDateWise(SupplierInvoices SupObj);
        List<SupplierInvoices> GetOpeningSupplierInvoices(SupplierInvoices SupObj);
        object DeleteSupplierInvoice(Guid ID, string userName);
        List<SupplierInvoices> GetOutStandingInvoicesBySupplier(Guid PaymentID, Guid supplierID);
        SupplierInvoices GetSupplierAdvances(string ID);
        SupplierInvoiceAgeingSummary GetSupplierInvoicesAgeingSummary();

    }
}