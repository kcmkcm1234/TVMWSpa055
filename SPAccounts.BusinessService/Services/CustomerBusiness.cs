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

        public List<Customer> GetAllCustomersForMobile()
        {
            return _customerRepository.GetAllCustomersForMobile();
        }
        public Customer GetCustomerDetails(Guid ID)
        {
            return _customerRepository.GetCustomerDetails(ID);
        }
        public Customer GetCustomerDetailsForMobile(Guid ID)
        {
            return _customerRepository.GetCustomerDetailsForMobile(ID);
        }
        public object InsertUpdateCustomer(Customer _customerObj, AppUA ua)
        {
            object result = null;
            try
            {
                if (_customerObj.ID==Guid.Empty)
                {
                    result = _customerRepository.InsertCustomer(_customerObj, ua);
                }
                else
                {
                    result = _customerRepository.UpdateCustomer(_customerObj, ua);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public object DeleteCustomer(Guid ID)
        {
            object result = null;
            try
            {
                result = _customerRepository.DeleteCustomer(ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public List<Titles> GetAllTitles()
        {
            return _customerRepository.GetAllTitles();
        }
    }
}