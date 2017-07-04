using SPAccounts.BusinessService.Contracts;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class CustomerPaymentsBusiness: ICustomerPaymentsBusiness
    { 
        private ICustomerPaymentsRepository _customerPaymentsRepository;
        public CustomerPaymentsBusiness(ICustomerPaymentsRepository custPaymentRepository)
        {
            _customerPaymentsRepository = custPaymentRepository;
        } 
    }
}