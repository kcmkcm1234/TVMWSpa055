using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ICustomerInvoicesRepository
    {
         List<CustomerInvoice> GetAllCustomerInvoices(DateTime? FromDate, DateTime? ToDate, string Customer, string InvoiceType, string Company, string Status, string Search);
        CustomerInvoiceSummary GetCustomerInvoicesSummary(bool IsInternal);
        CustomerInvoice GetCustomerInvoiceDetails(Guid ID);
        CustomerInvoice InsertInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua);
        CustomerInvoice UpdateInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua);
        List<CustomerInvoice> GetOutStandingInvoices(Guid PaymentID, Guid CustID);
        CustomerInvoice GetCustomerAdvances(string ID);
        object DeleteInvoices(Guid PaymentID, string UserName);
        List<CustomerInvoice> GetOutstandingCustomerInvoices(CustomerInvoice CusObj);
        List<CustomerInvoice> GetOpeningCustomerInvoices(CustomerInvoice CusObj);
        List<CustomerInvoice> GetCustomerInvoicesByDateWise(CustomerInvoice CusmObj);

        CustomerInvoiceSummary GetCustomerInvoicesSummaryForSA();

        //SpecialPayments
        List<CustomerInvoice> GetAllSpecialPayments(Guid InvoiceID);
        CustomerInvoice SpecialPaymentSummary(Guid InvoiceID);
        CustomerInvoice GetSpecialPaymentsDetails(Guid ID);
        CustomerInvoice InsertSpecialPayments(CustomerInvoice _customerInvoicesObj, AppUA ua);
        CustomerInvoice UpdateSpecialPayments(CustomerInvoice _customerInvoicesObj, AppUA ua);
        object DeleteSpecialPayments(Guid ID);
        CustomerInvoiceAgeingSummary GetCustomerInvoicesAgeingSummary();
    }
}
