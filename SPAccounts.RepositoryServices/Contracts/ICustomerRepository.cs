using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        List<Customer> GetAllCustomersForMobile();
        Customer GetCustomerDetails(Guid ID);
        Customer GetCustomerDetailsForMobile(Guid ID);
        Customer InsertCustomer(Customer _customerObj, AppUA ua);
        object UpdateCustomer(Customer _customerObj, AppUA ua);
        object DeleteCustomer(Guid ID);
        List<Titles> GetAllTitles();
    }
}