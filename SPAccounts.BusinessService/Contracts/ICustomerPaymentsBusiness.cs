﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ICustomerPaymentsBusiness
    {
        List<CustomerPayments> GetAllCustomerPayments();
        CustomerPayments GetCustomerPaymentsByID(string ID);
        CustomerPayments InsertUpdatePayments(CustomerPayments _custPayObj);
        object DeletePayments(Guid PaymentID,string UserName);




    }
}
