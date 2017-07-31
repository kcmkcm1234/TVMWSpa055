﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomerDetails(Guid ID);
        Customer InsertCustomer(Customer _customerObj);
        object UpdateCustomer(Customer _customerObj);
        object DeleteCustomer(Guid ID);
        List<Titles> GetAllTitles();
    }
}