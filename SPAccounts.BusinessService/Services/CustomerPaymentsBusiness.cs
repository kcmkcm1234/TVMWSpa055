using SPAccounts.BusinessService.Contracts;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;

namespace SPAccounts.BusinessService.Services
{
    public class CustomerPaymentsBusiness: ICustomerPaymentsBusiness
    { 
        private ICustomerPaymentsRepository _customerPaymentsRepository;
        public CustomerPaymentsBusiness(ICustomerPaymentsRepository custPaymentRepository)
        {
            _customerPaymentsRepository = custPaymentRepository;
        }

        public List<CustomerPayments> GetAllCustomerPayments()
        {
            List<CustomerPayments> custPayObj = null;
            custPayObj = _customerPaymentsRepository.GetAllCustomerPayments();
            return custPayObj;
        }

        public CustomerPayments GetCustomerPaymentsByID(string ID)
        {
            CustomerPayments custPayObj = null;
            custPayObj = _customerPaymentsRepository.GetCustomerPaymentsByID(ID);
            return custPayObj;

        }
    }
}