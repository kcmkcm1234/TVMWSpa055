﻿using SPAccounts.DataAccessObject.DTO;
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
        List<CustomerInvoice> GetOutStandingInvoices(Guid PaymentID,Guid CustID);
        CustomerInvoice GetCustomerAdvances(string ID);
        object DeleteInvoices(Guid ID, string UserName);
        CustomerInvoicesSummaryForMobile GetOutstandingCustomerInvoices();
        List<CustomerInvoice> GetOpeningCustomerInvoices();

    }
}
