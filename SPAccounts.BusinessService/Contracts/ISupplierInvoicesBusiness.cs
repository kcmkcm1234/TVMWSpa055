using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ISupplierInvoicesBusiness
    {
        List<SupplierInvoices> GetAllSupplierInvoices(DateTime? FromDate, DateTime? ToDate,string Supplier, string InvoiceType, string Company, string Status, string Search);
        SupplierInvoices GetSupplierInvoiceDetails(Guid ID);
        SupplierInvoiceSummary GetSupplierInvoicesSummary(bool IsInternal);
        SupplierInvoices InsertUpdateInvoice(SupplierInvoices _supplierInvoicesObj);
        SupplierSummaryforMobile GetOutstandingSupplierInvoices(SupplierInvoices SupObj);
        List<SupplierInvoices> GetOutStandingInvoicesBySupplier(Guid PaymentID,Guid supplierID);
        SupplierSummaryforMobile GetOpeningSupplierInvoices(SupplierInvoices SupObj);
        SupplierSummaryforMobile GetSupplierPurchasesByDateWise(SupplierInvoices SupObj);
        object DeleteSupplierInvoice(Guid ID, string userName);
        SupplierInvoices GetSupplierAdvances(string ID);
        SupplierInvoiceAgeingSummary GetSupplierInvoicesAgeingSummary();
    }
}