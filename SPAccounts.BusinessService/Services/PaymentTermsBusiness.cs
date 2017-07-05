using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class PaymentTermsBusiness:IPaymentTermsBusiness
    {
        private IPaymentTermsRepository _paymentTermsRepository;

        public PaymentTermsBusiness(IPaymentTermsRepository paymentTermsRepository)
        {
            _paymentTermsRepository = paymentTermsRepository;
        }
        public List<PaymentTerms> GetAllPayTerms()
        {
            return _paymentTermsRepository.GetAllPayTerms();
        }
    }
}