using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class CustomerBusiness:ICustomerBusiness
    {
        private ICustomerRepository _customerRepository;

        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }
        public Customer GetCustomerDetails(Guid ID)
        {
            return _customerRepository.GetCustomerDetails(ID);
        }
    }
}