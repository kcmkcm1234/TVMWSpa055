using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ICustomerBusiness
    {
        List<Customer> GetAllCustomers();
        List<Customer> GetAllCustomersForMobile(Customer cusObj);
        Customer GetCustomerDetails(Guid ID);
        object InsertUpdateCustomer(Customer _customerObj);
        Customer GetCustomerDetailsForMobile(Guid ID);
        object DeleteCustomer(Guid ID);
        List<Titles> GetAllTitles();
    }
}